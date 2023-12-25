using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000454 RID: 1108
public class AchievementToastManager : AbstractMonoBehaviour
{
	// Token: 0x060010B7 RID: 4279 RVA: 0x000A057F File Offset: 0x0009E97F
	private void OnEnable()
	{
		LocalAchievementsManager.AchievementUnlockedEvent += this.UnlockAchievement;
	}

	// Token: 0x060010B8 RID: 4280 RVA: 0x000A0592 File Offset: 0x0009E992
	private void OnDisable()
	{
		LocalAchievementsManager.AchievementUnlockedEvent -= this.UnlockAchievement;
	}

	// Token: 0x060010B9 RID: 4281 RVA: 0x000A05A8 File Offset: 0x0009E9A8
	private void Start()
	{
		this.uiCamera = UnityEngine.Object.Instantiate<GameObject>(this.uiCameraPrefab);
		this.uiCamera.transform.SetParent(base.transform);
		this.uiCamera.transform.ResetLocalTransforms();
		Camera component = this.uiCamera.GetComponent<Camera>();
		component.cullingMask = 65536;
		component.depth = (float)AchievementToastManager.CameraDepth;
		Canvas componentInChildren = base.GetComponentInChildren<Canvas>(true);
		componentInChildren.worldCamera = component;
		componentInChildren.sortingLayerName = SpriteLayer.AchievementToast.ToString();
		this.uiCamera.SetActive(false);
	}

	// Token: 0x060010BA RID: 4282 RVA: 0x000A0640 File Offset: 0x0009EA40
	public void UnlockAchievement(LocalAchievementsManager.Achievement achievement)
	{
		if (this.currentAnimation != null)
		{
			this.queuedAchievements.Add(achievement);
			return;
		}
		this.currentAnimation = base.StartCoroutine(this.showUnlock(achievement));
	}

	// Token: 0x060010BB RID: 4283 RVA: 0x000A0670 File Offset: 0x0009EA70
	private IEnumerator showUnlock(LocalAchievementsManager.Achievement achievement)
	{
		AudioManager.Play("achievement_unlocked");
		string achievementName = achievement.ToString();
		string titleKey = "Achievement" + achievementName + "Toast";
		this.titleLocalization.ApplyTranslation(Localization.Find(titleKey), null);
		string spriteName = achievementName + "_toast";
		Sprite sprite = this.getAtlas(achievement).GetSprite(spriteName);
		this.icon.sprite = sprite;
		this.toastTransform.position = AchievementToastManager.InitialPosition;
		this.visual.SetActive(true);
		this.uiCamera.SetActive(true);
		Vector2 displacement = AchievementToastManager.FinalPosition - AchievementToastManager.InitialPosition;
		float elapsed = 0f;
		while (elapsed < AchievementToastManager.AnimationDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			float factor = this.easeOutBack(elapsed, 0f, 1f, AchievementToastManager.AnimationDuration);
			this.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor * displacement;
			yield return null;
		}
		this.toastTransform.localPosition = AchievementToastManager.FinalPosition;
		yield return new AchievementToastManager.WaitForSecondsRealtime(AchievementToastManager.HoldDuration);
		elapsed = 0f;
		while (elapsed < AchievementToastManager.AnimationDuration)
		{
			elapsed += Time.unscaledDeltaTime;
			float factor2 = this.easeInBack(elapsed, 1f, -1f, AchievementToastManager.AnimationDuration);
			this.toastTransform.localPosition = AchievementToastManager.InitialPosition + factor2 * displacement;
			yield return null;
		}
		if (this.queuedAchievements.Count > 0)
		{
			LocalAchievementsManager.Achievement achievement2 = this.queuedAchievements[0];
			this.queuedAchievements.RemoveAt(0);
			this.currentAnimation = base.StartCoroutine(this.showUnlock(achievement2));
		}
		else
		{
			this.currentAnimation = null;
			this.visual.SetActive(false);
			this.uiCamera.SetActive(false);
		}
		yield break;
	}

	// Token: 0x060010BC RID: 4284 RVA: 0x000A0694 File Offset: 0x0009EA94
	private float easeOutBack(float t, float initial, float change, float duration)
	{
		float num = 1.70158f;
		return change * ((t = t / duration - 1f) * t * ((num + 1f) * t + num) + 1f) + initial;
	}

	// Token: 0x060010BD RID: 4285 RVA: 0x000A06CC File Offset: 0x0009EACC
	private float easeInBack(float t, float initial, float change, float duration)
	{
		float num = 1.70158f;
		float num2;
		t = (num2 = t / duration);
		return change * num2 * t * ((num + 1f) * t - num) + initial;
	}

	// Token: 0x060010BE RID: 4286 RVA: 0x000A06FA File Offset: 0x0009EAFA
	private SpriteAtlas getAtlas(LocalAchievementsManager.Achievement achievement)
	{
		if (Array.IndexOf<LocalAchievementsManager.Achievement>(LocalAchievementsManager.DLCAchievements, achievement) >= 0)
		{
			return this.dlcAtlas;
		}
		return this.defaultAtlas;
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x060010BF RID: 4287 RVA: 0x000A071A File Offset: 0x0009EB1A
	private SpriteAtlas defaultAtlas
	{
		get
		{
			if (!this._defaultAtlasCached)
			{
				this._defaultAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements");
				this._defaultAtlasCached = true;
			}
			return this._defaultAtlas;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060010C0 RID: 4288 RVA: 0x000A0744 File Offset: 0x0009EB44
	private SpriteAtlas dlcAtlas
	{
		get
		{
			if (!this._dlcAtlasCached && DLCManager.DLCEnabled())
			{
				this._dlcAtlas = AssetLoader<SpriteAtlas>.GetCachedAsset("Achievements_DLC");
				this._dlcAtlasCached = true;
			}
			return this._dlcAtlas;
		}
	}

	// Token: 0x040019FC RID: 6652
	private static readonly int CameraDepth = 91;

	// Token: 0x040019FD RID: 6653
	private static readonly Vector2 InitialPosition = new Vector2(0f, -460f);

	// Token: 0x040019FE RID: 6654
	private static readonly Vector2 FinalPosition = new Vector2(0f, -280f);

	// Token: 0x040019FF RID: 6655
	public static readonly float AnimationDuration = 0.34f;

	// Token: 0x04001A00 RID: 6656
	private static readonly float HoldDuration = 2f;

	// Token: 0x04001A01 RID: 6657
	[SerializeField]
	private GameObject uiCameraPrefab;

	// Token: 0x04001A02 RID: 6658
	[SerializeField]
	private GameObject visual;

	// Token: 0x04001A03 RID: 6659
	[SerializeField]
	private RectTransform toastTransform;

	// Token: 0x04001A04 RID: 6660
	[SerializeField]
	private LocalizationHelper titleLocalization;

	// Token: 0x04001A05 RID: 6661
	[SerializeField]
	private Image icon;

	// Token: 0x04001A06 RID: 6662
	private List<LocalAchievementsManager.Achievement> queuedAchievements = new List<LocalAchievementsManager.Achievement>();

	// Token: 0x04001A07 RID: 6663
	private Coroutine currentAnimation;

	// Token: 0x04001A08 RID: 6664
	private GameObject uiCamera;

	// Token: 0x04001A09 RID: 6665
	private bool _defaultAtlasCached;

	// Token: 0x04001A0A RID: 6666
	private SpriteAtlas _defaultAtlas;

	// Token: 0x04001A0B RID: 6667
	private bool _dlcAtlasCached;

	// Token: 0x04001A0C RID: 6668
	private SpriteAtlas _dlcAtlas;

	// Token: 0x02000455 RID: 1109
	public class WaitForSecondsRealtime : CustomYieldInstruction
	{
		// Token: 0x060010C2 RID: 4290 RVA: 0x000A07C8 File Offset: 0x0009EBC8
		public WaitForSecondsRealtime(float time)
		{
			this.waitTime = time;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060010C3 RID: 4291 RVA: 0x000A07E2 File Offset: 0x0009EBE2
		// (set) Token: 0x060010C4 RID: 4292 RVA: 0x000A07EA File Offset: 0x0009EBEA
		public float waitTime { get; set; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060010C5 RID: 4293 RVA: 0x000A07F4 File Offset: 0x0009EBF4
		public override bool keepWaiting
		{
			get
			{
				if (this.m_WaitUntilTime < 0f)
				{
					this.m_WaitUntilTime = Time.realtimeSinceStartup + this.waitTime;
				}
				bool flag = Time.realtimeSinceStartup < this.m_WaitUntilTime;
				if (!flag)
				{
					this.m_WaitUntilTime = -1f;
				}
				return flag;
			}
		}

		// Token: 0x04001A0E RID: 6670
		private float m_WaitUntilTime = -1f;
	}
}
