using System;
using UnityEngine;

// Token: 0x02000A1D RID: 2589
public class LevelPlayerController : AbstractPlayerController
{
	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06003D57 RID: 15703 RVA: 0x0021E92B File Offset: 0x0021CD2B
	public bool Ducking
	{
		get
		{
			return this.motor.Ducking;
		}
	}

	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06003D58 RID: 15704 RVA: 0x0021E938 File Offset: 0x0021CD38
	public LevelPlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<LevelPlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x06003D59 RID: 15705 RVA: 0x0021E95D File Offset: 0x0021CD5D
	public LevelPlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<LevelPlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x06003D5A RID: 15706 RVA: 0x0021E982 File Offset: 0x0021CD82
	public LevelPlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<LevelPlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x06003D5B RID: 15707 RVA: 0x0021E9A7 File Offset: 0x0021CDA7
	public LevelPlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<LevelPlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06003D5C RID: 15708 RVA: 0x0021E9CC File Offset: 0x0021CDCC
	public LevelPlayerColliderManager colliderManager
	{
		get
		{
			if (this._colliderManager == null)
			{
				this._colliderManager = base.GetComponent<LevelPlayerColliderManager>();
			}
			return this._colliderManager;
		}
	}

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06003D5D RID: 15709 RVA: 0x0021E9F4 File Offset: 0x0021CDF4
	public override Vector3 center
	{
		get
		{
			if (base.transform == null)
			{
				return Vector3.zero;
			}
			return base.transform.position + new Vector3(base.collider.offset.x, base.collider.offset.y * this.motor.GravityReversalMultiplier, 0f);
		}
	}

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06003D5E RID: 15710 RVA: 0x0021EA64 File Offset: 0x0021CE64
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || Level.IsChessBoss || !this.motor.Dashing) && (!base.stats.isChalice || !this.motor.Dashing || !this.motor.ChaliceDuckDashed) && (!base.stats.isChalice || !this.motor.Dashing || true);
		}
	}

	// Token: 0x17000539 RID: 1337
	// (get) Token: 0x06003D5F RID: 15711 RVA: 0x0021EB20 File Offset: 0x0021CF20
	public override Vector3 CameraCenter
	{
		get
		{
			if (Level.Current.LevelType == Level.Type.Platforming)
			{
				this.cameraCenterPosition = Mathf.Lerp(this.cameraCenterPosition, 250f * (float)this._motor.TrueLookDirection.x.Value, 1.2f * CupheadTime.Delta);
				return this.center + new Vector3(this.cameraCenterPosition, 0f);
			}
			return base.CameraCenter;
		}
	}

	// Token: 0x06003D60 RID: 15712 RVA: 0x0021EBA0 File Offset: 0x0021CFA0
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x06003D61 RID: 15713 RVA: 0x0021EBD4 File Offset: 0x0021CFD4
	public void UnpauseAll(bool forced = false)
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			if (forced)
			{
				abstractPausableComponent.preEnabled = true;
			}
			abstractPausableComponent.enabled = true;
		}
	}

	// Token: 0x06003D62 RID: 15714 RVA: 0x0021EC14 File Offset: 0x0021D014
	public void OnPitKnockUp(float y, float velocityScale = 1f)
	{
		if (base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && base.stats.Loadout.charm != Charm.charm_float)
		{
			base.stats.OnPitKnockUp();
		}
		this.motor.OnPitKnockUp(y, velocityScale);
	}

	// Token: 0x06003D63 RID: 15715 RVA: 0x0021EC63 File Offset: 0x0021D063
	protected override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.weaponManager.LevelInit(id);
		if (base.stats.Health == 0)
		{
			this.StartDead();
		}
	}

	// Token: 0x06003D64 RID: 15716 RVA: 0x0021EC9C File Offset: 0x0021D09C
	protected override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		Vector3 position = base.transform.position;
		if (this.motor.GravityReversed)
		{
			position.y += (this.center.y - base.transform.position.y) * 2f;
		}
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
		this.weaponManager.OnDeath();
	}

	// Token: 0x06003D65 RID: 15717 RVA: 0x0021ED74 File Offset: 0x0021D174
	public override void OnLeave(PlayerId playerId)
	{
		if (!base.IsDead)
		{
			Vector3 position = base.transform.position;
			if (this.motor.GravityReversed)
			{
				position.y += (this.center.y - base.transform.position.y) * 2f;
			}
			this.deathEffect.CreateExplosionOnly(playerId, position, PlayerMode.Level);
		}
		base.OnLeave(playerId);
	}

	// Token: 0x06003D66 RID: 15718 RVA: 0x0021EDF8 File Offset: 0x0021D1F8
	private void StartDead()
	{
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		position.y += 1000f;
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003D67 RID: 15719 RVA: 0x0021EE81 File Offset: 0x0021D281
	public void DisableInput()
	{
		this.motor.DisableInput();
		this.weaponManager.DisableInput();
		AudioManager.Stop("player_default_fire_loop");
	}

	// Token: 0x06003D68 RID: 15720 RVA: 0x0021EEA3 File Offset: 0x0021D2A3
	public void EnableInput()
	{
		this.motor.EnableInput();
		this.weaponManager.EnableInput();
	}

	// Token: 0x06003D69 RID: 15721 RVA: 0x0021EEBB File Offset: 0x0021D2BB
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x06003D6A RID: 15722 RVA: 0x0021EECE File Offset: 0x0021D2CE
	public void OnLevelWinPause()
	{
		this.PauseAll();
		base.collider.enabled = false;
	}

	// Token: 0x06003D6B RID: 15723 RVA: 0x0021EEE4 File Offset: 0x0021D2E4
	public override void OnLevelWin()
	{
		this.UnpauseAll(false);
		this.weaponManager.DisableInput();
		base.collider.enabled = false;
		AudioManager.Stop("player_default_fire_loop");
		if (Level.Current.LevelType == Level.Type.Platforming)
		{
			this.motor.OnPlatformingLevelExit();
		}
	}

	// Token: 0x06003D6C RID: 15724 RVA: 0x0021EF34 File Offset: 0x0021D334
	public void ReverseControls(float reverseTime)
	{
		base.stats.ReverseControls(reverseTime);
	}

	// Token: 0x06003D6D RID: 15725 RVA: 0x0021EF42 File Offset: 0x0021D342
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.DrawCube(this.CameraCenter, Vector3.one * 50f);
		}
	}

	// Token: 0x06003D6E RID: 15726 RVA: 0x0021EF6E File Offset: 0x0021D36E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.deathEffect = null;
	}

	// Token: 0x040044A5 RID: 17573
	private const float PLATFORMING_CAMERA_DISTANCE_RUNNING = 250f;

	// Token: 0x040044A6 RID: 17574
	private const float PLATFORMING_CAMERA_DISTANCE_STATIC = 50f;

	// Token: 0x040044A7 RID: 17575
	private const float PLATFORMING_CAMERA_TIME_RUNNING = 1.2f;

	// Token: 0x040044A8 RID: 17576
	private const float PLATFORMING_CAMERA_TIME_STATIC = 6f;

	// Token: 0x040044A9 RID: 17577
	private bool initialized;

	// Token: 0x040044AA RID: 17578
	private LevelPlayerMotor _motor;

	// Token: 0x040044AB RID: 17579
	private LevelPlayerAnimationController _animationController;

	// Token: 0x040044AC RID: 17580
	private LevelPlayerWeaponManager _weaponManager;

	// Token: 0x040044AD RID: 17581
	private LevelPlayerParryController _parryController;

	// Token: 0x040044AE RID: 17582
	private LevelPlayerColliderManager _colliderManager;

	// Token: 0x040044AF RID: 17583
	[SerializeField]
	private PlayerDeathEffect deathEffect;

	// Token: 0x040044B0 RID: 17584
	private float cameraCenterPosition;
}
