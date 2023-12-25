using System;
using UnityEngine;

// Token: 0x02000AAB RID: 2731
public abstract class AbstractPlaneSuper : AbstractCollidableObject
{
	// Token: 0x170005BC RID: 1468
	// (get) Token: 0x06004182 RID: 16770 RVA: 0x00237C85 File Offset: 0x00236085
	public PlanePlayerWeaponManager.States.Super State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x170005BD RID: 1469
	// (get) Token: 0x06004183 RID: 16771 RVA: 0x00237C8D File Offset: 0x0023608D
	protected override bool allowCollisionPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x00237C90 File Offset: 0x00236090
	protected override void Awake()
	{
		base.tag = "PlayerProjectile";
		base.Awake();
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x00237CA3 File Offset: 0x002360A3
	protected virtual void Start()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x00237CCC File Offset: 0x002360CC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x00237CE4 File Offset: 0x002360E4
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06004188 RID: 16776 RVA: 0x00237D08 File Offset: 0x00236108
	public AbstractPlaneSuper Create(PlanePlayerController player)
	{
		AbstractPlaneSuper abstractPlaneSuper = this.InstantiatePrefab<AbstractPlaneSuper>();
		abstractPlaneSuper.player = player;
		if (player.stats.isChalice)
		{
			abstractPlaneSuper.spriteRenderer = this.chalice;
			abstractPlaneSuper.chalice.gameObject.SetActive(true);
			if (abstractPlaneSuper.cuphead)
			{
				abstractPlaneSuper.cuphead.gameObject.SetActive(false);
			}
			if (abstractPlaneSuper.mugman)
			{
				abstractPlaneSuper.mugman.gameObject.SetActive(false);
			}
		}
		else
		{
			PlayerId id = player.id;
			if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
			{
				abstractPlaneSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.cuphead : this.mugman);
				abstractPlaneSuper.cuphead.gameObject.SetActive(!PlayerManager.player1IsMugman);
				abstractPlaneSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman);
			}
			else
			{
				abstractPlaneSuper.spriteRenderer = ((!PlayerManager.player1IsMugman) ? this.mugman : this.cuphead);
				abstractPlaneSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman);
				abstractPlaneSuper.mugman.gameObject.SetActive(!PlayerManager.player1IsMugman);
			}
		}
		abstractPlaneSuper.StartSuper();
		return abstractPlaneSuper;
	}

	// Token: 0x06004189 RID: 16777 RVA: 0x00237E60 File Offset: 0x00236260
	protected virtual void StartSuper()
	{
		this.animHelper = base.GetComponent<AnimationHelper>();
		this.animHelper.IgnoreGlobal = true;
		PauseManager.Pause();
		this.player.PauseAll();
		this.player.SetSpriteVisible(false);
		AudioManager.SnapshotTransition(new string[]
		{
			"Super",
			"Unpaused",
			"Unpaused_1920s"
		}, new float[]
		{
			1f,
			0f,
			0f
		}, 0.1f);
		AudioManager.ChangeBGMPitch(1.3f, 1.5f);
		AudioManager.Play("player_super_beam_start");
		base.transform.SetScale(new float?(this.player.transform.localScale.x), new float?(this.player.transform.localScale.y), new float?(1f));
		base.transform.position = this.player.transform.position;
	}

	// Token: 0x0600418A RID: 16778 RVA: 0x00237F6E File Offset: 0x0023636E
	protected virtual void Fire()
	{
		this.state = PlanePlayerWeaponManager.States.Super.Ending;
	}

	// Token: 0x0600418B RID: 16779 RVA: 0x00237F78 File Offset: 0x00236378
	protected void SnapshotAudio()
	{
		string[] array = new string[2];
		array[0] = "Super";
		if (SettingsData.Data.vintageAudioEnabled)
		{
			array[1] = "Unpaused_1920s";
		}
		else
		{
			array[1] = "Unpaused";
		}
		AudioManager.SnapshotTransition(array, new float[]
		{
			0f,
			1f
		}, 4f);
		AudioManager.ChangeBGMPitch(1f, 4f);
	}

	// Token: 0x0600418C RID: 16780 RVA: 0x00237FEA File Offset: 0x002363EA
	protected virtual void StartCountdown()
	{
		this.SnapshotAudio();
		PauseManager.Unpause();
		this.player.UnpauseAll(false);
		this.player.SetSpriteVisible(true);
		this.animHelper.IgnoreGlobal = false;
		this.state = PlanePlayerWeaponManager.States.Super.Countdown;
	}

	// Token: 0x04004807 RID: 18439
	protected PlanePlayerWeaponManager.States.Super state = PlanePlayerWeaponManager.States.Super.Intro;

	// Token: 0x04004808 RID: 18440
	[SerializeField]
	[Header("Player Sprites")]
	private SpriteRenderer cuphead;

	// Token: 0x04004809 RID: 18441
	[SerializeField]
	private SpriteRenderer mugman;

	// Token: 0x0400480A RID: 18442
	[SerializeField]
	protected SpriteRenderer chalice;

	// Token: 0x0400480B RID: 18443
	protected SpriteRenderer spriteRenderer;

	// Token: 0x0400480C RID: 18444
	protected PlanePlayerController player;

	// Token: 0x0400480D RID: 18445
	protected DamageDealer damageDealer;

	// Token: 0x0400480E RID: 18446
	protected AnimationHelper animHelper;
}
