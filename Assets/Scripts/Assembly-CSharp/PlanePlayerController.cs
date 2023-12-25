using System;
using UnityEngine;

// Token: 0x02000A99 RID: 2713
public class PlanePlayerController : AbstractPlayerController
{
	// Token: 0x170005A6 RID: 1446
	// (get) Token: 0x060040F7 RID: 16631 RVA: 0x0023541D File Offset: 0x0023381D
	public bool Shrunk
	{
		get
		{
			return this.animationController.ShrinkState == PlanePlayerAnimationController.ShrinkStates.Shrunk;
		}
	}

	// Token: 0x170005A7 RID: 1447
	// (get) Token: 0x060040F8 RID: 16632 RVA: 0x0023542D File Offset: 0x0023382D
	public bool Parrying
	{
		get
		{
			return this.parryController.State == PlanePlayerParryController.ParryState.Parrying;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (get) Token: 0x060040F9 RID: 16633 RVA: 0x0023543D File Offset: 0x0023383D
	public bool WeaponBusy
	{
		get
		{
			return this.weaponManager.state != PlanePlayerWeaponManager.State.Ready || !this.weaponManager.CanInterupt;
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (get) Token: 0x060040FA RID: 16634 RVA: 0x00235461 File Offset: 0x00233861
	public PlanePlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<PlanePlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (get) Token: 0x060040FB RID: 16635 RVA: 0x00235486 File Offset: 0x00233886
	public PlanePlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<PlanePlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x170005AB RID: 1451
	// (get) Token: 0x060040FC RID: 16636 RVA: 0x002354AB File Offset: 0x002338AB
	public PlanePlayerAudioController audioController
	{
		get
		{
			if (this._audioController == null)
			{
				this._audioController = base.GetComponent<PlanePlayerAudioController>();
			}
			return this._audioController;
		}
	}

	// Token: 0x170005AC RID: 1452
	// (get) Token: 0x060040FD RID: 16637 RVA: 0x002354D0 File Offset: 0x002338D0
	public PlanePlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<PlanePlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x170005AD RID: 1453
	// (get) Token: 0x060040FE RID: 16638 RVA: 0x002354F5 File Offset: 0x002338F5
	public PlanePlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<PlanePlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x170005AE RID: 1454
	// (get) Token: 0x060040FF RID: 16639 RVA: 0x0023551C File Offset: 0x0023391C
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || !this.animationController.Shrinking);
		}
	}

	// Token: 0x06004100 RID: 16640 RVA: 0x00235578 File Offset: 0x00233978
	private void Start()
	{
		if (!Level.Current.Started)
		{
			this.motor.enabled = false;
		}
	}

	// Token: 0x06004101 RID: 16641 RVA: 0x00235595 File Offset: 0x00233995
	public override void PlayIntro()
	{
		base.PlayIntro();
		this.animationController.PlayIntro();
	}

	// Token: 0x06004102 RID: 16642 RVA: 0x002355A8 File Offset: 0x002339A8
	protected override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.audioController.LevelInit();
		if (base.stats.Health == 0)
		{
			this.StartDead();
		}
	}

	// Token: 0x06004103 RID: 16643 RVA: 0x002355DD File Offset: 0x002339DD
	public override void LevelStart()
	{
		base.LevelStart();
		this.motor.enabled = true;
	}

	// Token: 0x06004104 RID: 16644 RVA: 0x002355F1 File Offset: 0x002339F1
	public void GetStoned(float stoneTime)
	{
		base.stats.GetStoned(stoneTime);
	}

	// Token: 0x06004105 RID: 16645 RVA: 0x00235600 File Offset: 0x00233A00
	protected override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, base.transform.position, base.stats.Deaths, PlayerMode.Plane, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
		if (PauseManager.state == PauseManager.State.Paused)
		{
			PauseManager.Unpause();
		}
	}

	// Token: 0x06004106 RID: 16646 RVA: 0x00235684 File Offset: 0x00233A84
	public override void OnLeave(PlayerId playerId)
	{
		if (!base.IsDead)
		{
			this.deathEffect.CreateExplosionOnly(base.id, base.transform.position, PlayerMode.Plane);
		}
		base.OnLeave(playerId);
	}

	// Token: 0x06004107 RID: 16647 RVA: 0x002356BC File Offset: 0x00233ABC
	private void StartDead()
	{
		base.gameObject.SetActive(false);
		Vector3 position = base.transform.position;
		position.y += 1000f;
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, position, base.stats.Deaths, PlayerMode.Plane, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06004108 RID: 16648 RVA: 0x00235748 File Offset: 0x00233B48
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x06004109 RID: 16649 RVA: 0x0023577C File Offset: 0x00233B7C
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

	// Token: 0x0600410A RID: 16650 RVA: 0x002357BC File Offset: 0x00233BBC
	public void SetSpriteVisible(bool visibility)
	{
		this.animationController.SetSpriteVisible(visibility);
	}

	// Token: 0x0600410B RID: 16651 RVA: 0x002357CA File Offset: 0x00233BCA
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x0400479E RID: 18334
	public const float INTRO_TIME = 1f;

	// Token: 0x0400479F RID: 18335
	private PlanePlayerMotor _motor;

	// Token: 0x040047A0 RID: 18336
	private PlanePlayerAnimationController _animationController;

	// Token: 0x040047A1 RID: 18337
	private PlanePlayerAudioController _audioController;

	// Token: 0x040047A2 RID: 18338
	private PlanePlayerWeaponManager _weaponManager;

	// Token: 0x040047A3 RID: 18339
	private PlanePlayerParryController _parryController;

	// Token: 0x040047A4 RID: 18340
	[SerializeField]
	private PlayerDeathEffect deathEffect;
}
