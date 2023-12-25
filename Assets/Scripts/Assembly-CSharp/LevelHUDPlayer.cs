using System;
using System.Collections;
using RektTransform;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x0200048C RID: 1164
public class LevelHUDPlayer : AbstractPausableComponent
{
	// Token: 0x170002D2 RID: 722
	// (get) Token: 0x06001242 RID: 4674 RVA: 0x000A92C5 File Offset: 0x000A76C5
	// (set) Token: 0x06001243 RID: 4675 RVA: 0x000A92CD File Offset: 0x000A76CD
	public AbstractPlayerController player { get; private set; }

	// Token: 0x06001244 RID: 4676 RVA: 0x000A92D8 File Offset: 0x000A76D8
	public void Init(AbstractPlayerController player, bool startAtOneHealth = false)
	{
		this.player = player;
		if (player.id == PlayerId.PlayerTwo)
		{
			this.SetupPlayerTwo();
		}
		player.stats.OnHealthChangedEvent += this.OnHealthChanged;
		player.stats.OnSuperChangedEvent += this.OnSuperChanged;
		player.stats.OnWeaponChangedEvent += this.OnWeaponChanged;
		this.health.Init(this);
		if (startAtOneHealth)
		{
			this.health.OnHealthChanged(1);
		}
		this.super.Init(this);
		if (player as PlanePlayerController != null)
		{
			this.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchSHMUPWeapon && !Level.IsTowerOfPowerMain);
		}
		else
		{
			this.weaponSwitchNotification.gameObject.SetActive(PlayerData.Data.Loadouts.GetPlayerLoadout(player.id).MustNotifySwitchRegularWeapon && !Level.IsTowerOfPowerMain);
		}
		this.weaponSwitchNotification.alpha = 1f;
		this.weaponSwitchTransform = this.weaponSwitchNotification.GetComponent<RectTransform>();
		this.weaponSwitchStartPosition = this.weaponSwitchTransform.anchoredPosition;
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x000A942C File Offset: 0x000A782C
	private void SetupPlayerTwo()
	{
		base.gameObject.name = "Mugman";
		Vector3 localPosition = base.transform.localPosition;
		localPosition.x *= -1f;
		base.rectTransform.SetAnchors(new RektTransform.MinMax(new Vector2(1f, 0f), new Vector2(1f, 0f)));
		base.rectTransform.pivot = new Vector2(1f, 0f);
		base.transform.localPosition = localPosition;
		localPosition = this.health.rectTransform.localPosition;
		localPosition.x *= -1f;
		this.health.rectTransform.localPosition = localPosition;
		this.weaponRoot.localPosition = localPosition;
		localPosition = this.super.rectTransform.localPosition;
		localPosition.x *= -1f;
		this.super.rectTransform.SetScale(new float?(-1f), null, null);
		this.super.rectTransform.localPosition = localPosition;
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x000A955E File Offset: 0x000A795E
	private void OnHealthChanged(int health, PlayerId playerId)
	{
		this.health.OnHealthChanged(health);
	}

	// Token: 0x06001247 RID: 4679 RVA: 0x000A956C File Offset: 0x000A796C
	private void OnSuperChanged(float super, PlayerId playerId, bool playEffect)
	{
		this.super.OnSuperChanged(super);
	}

	// Token: 0x06001248 RID: 4680 RVA: 0x000A957C File Offset: 0x000A797C
	private void OnWeaponChanged(Weapon weapon)
	{
		this.weaponIconPrefab.Create(this.weaponRoot, weapon);
		if (this.weaponSwitchNotification.gameObject.activeSelf)
		{
			bool flag = PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchSHMUPWeapon || PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchRegularWeapon;
			if (this.player as PlanePlayerController != null)
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchSHMUPWeapon = false;
			}
			else
			{
				PlayerData.Data.Loadouts.GetPlayerLoadout(this.player.id).MustNotifySwitchRegularWeapon = false;
			}
			if (flag)
			{
				PlayerData.SaveCurrentFile();
			}
			base.StartCoroutine(this.FadeOutSwitchNotification(0.4f));
		}
	}

	// Token: 0x06001249 RID: 4681 RVA: 0x000A9670 File Offset: 0x000A7A70
	private void Update()
	{
		if (this.weaponSwitchNotification.gameObject.activeSelf)
		{
			this.weaponSwitchTransform.anchoredPosition = this.weaponSwitchStartPosition + Vector2.up * Mathf.Sin(Time.time * this.weaponSwitchWobbleSpeed) * this.weaponSwitchWobbleScale;
		}
	}

	// Token: 0x0600124A RID: 4682 RVA: 0x000A96CE File Offset: 0x000A7ACE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.health = null;
		this.super = null;
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x000A96E4 File Offset: 0x000A7AE4
	private IEnumerator FadeOutSwitchNotification(float overTime)
	{
		if (overTime > 0f)
		{
			float startAlpha = this.weaponSwitchNotification.alpha;
			float timeSpent = 0f;
			while (timeSpent < overTime)
			{
				this.weaponSwitchNotification.alpha = Mathf.Lerp(startAlpha, 0f, timeSpent / overTime);
				timeSpent += Time.deltaTime;
				yield return null;
			}
			this.weaponSwitchNotification.gameObject.SetActive(false);
		}
		yield break;
	}

	// Token: 0x04001BB1 RID: 7089
	[SerializeField]
	private LevelHUDPlayerHealth health;

	// Token: 0x04001BB2 RID: 7090
	[SerializeField]
	private LevelHUDPlayerSuper super;

	// Token: 0x04001BB3 RID: 7091
	[Space(10f)]
	[SerializeField]
	private RectTransform weaponRoot;

	// Token: 0x04001BB4 RID: 7092
	[SerializeField]
	[FormerlySerializedAs("weaponPrefab")]
	private LevelHUDWeapon weaponIconPrefab;

	// Token: 0x04001BB5 RID: 7093
	[SerializeField]
	private CanvasGroup weaponSwitchNotification;

	// Token: 0x04001BB6 RID: 7094
	public float weaponSwitchWobbleSpeed = 1f;

	// Token: 0x04001BB7 RID: 7095
	public float weaponSwitchWobbleScale = 1f;

	// Token: 0x04001BB9 RID: 7097
	private RectTransform weaponSwitchTransform;

	// Token: 0x04001BBA RID: 7098
	private Vector2 weaponSwitchStartPosition;
}
