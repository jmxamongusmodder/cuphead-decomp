using System;
using UnityEngine;

// Token: 0x020009DD RID: 2525
public class ArcadePlayerController : AbstractPlayerController
{
	// Token: 0x170004F2 RID: 1266
	// (get) Token: 0x06003B82 RID: 15234 RVA: 0x00215347 File Offset: 0x00213747
	public ArcadePlayerMotor motor
	{
		get
		{
			if (this._motor == null)
			{
				this._motor = base.GetComponent<ArcadePlayerMotor>();
			}
			return this._motor;
		}
	}

	// Token: 0x170004F3 RID: 1267
	// (get) Token: 0x06003B83 RID: 15235 RVA: 0x0021536C File Offset: 0x0021376C
	public ArcadePlayerAnimationController animationController
	{
		get
		{
			if (this._animationController == null)
			{
				this._animationController = base.GetComponent<ArcadePlayerAnimationController>();
			}
			return this._animationController;
		}
	}

	// Token: 0x170004F4 RID: 1268
	// (get) Token: 0x06003B84 RID: 15236 RVA: 0x00215391 File Offset: 0x00213791
	public ArcadePlayerWeaponManager weaponManager
	{
		get
		{
			if (this._weaponManager == null)
			{
				this._weaponManager = base.GetComponent<ArcadePlayerWeaponManager>();
			}
			return this._weaponManager;
		}
	}

	// Token: 0x170004F5 RID: 1269
	// (get) Token: 0x06003B85 RID: 15237 RVA: 0x002153B6 File Offset: 0x002137B6
	public ArcadePlayerParryController parryController
	{
		get
		{
			if (this._parryController == null)
			{
				this._parryController = base.GetComponent<ArcadePlayerParryController>();
			}
			return this._parryController;
		}
	}

	// Token: 0x170004F6 RID: 1270
	// (get) Token: 0x06003B86 RID: 15238 RVA: 0x002153DB File Offset: 0x002137DB
	public ArcadePlayerColliderManager colliderManager
	{
		get
		{
			if (this._colliderManager == null)
			{
				this._colliderManager = base.GetComponent<ArcadePlayerColliderManager>();
			}
			return this._colliderManager;
		}
	}

	// Token: 0x170004F7 RID: 1271
	// (get) Token: 0x06003B87 RID: 15239 RVA: 0x00215400 File Offset: 0x00213800
	// (set) Token: 0x06003B88 RID: 15240 RVA: 0x00215408 File Offset: 0x00213808
	public ArcadePlayerController.ControlScheme controlScheme { get; private set; }

	// Token: 0x170004F8 RID: 1272
	// (get) Token: 0x06003B89 RID: 15241 RVA: 0x00215414 File Offset: 0x00213814
	public override bool CanTakeDamage
	{
		get
		{
			return base.damageReceiver.state == PlayerDamageReceiver.State.Vulnerable && ((base.stats.Loadout.charm != Charm.charm_smoke_dash && !base.stats.CurseSmokeDash) || !this.motor.Dashing);
		}
	}

	// Token: 0x06003B8A RID: 15242 RVA: 0x00215470 File Offset: 0x00213870
	private void Start()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Normal;
	}

	// Token: 0x06003B8B RID: 15243 RVA: 0x00215479 File Offset: 0x00213879
	public void ChangeToRocket()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Rocket;
		this.weaponManager.ChangeToRocket();
		this.animationController.ChangeToRocket();
	}

	// Token: 0x06003B8C RID: 15244 RVA: 0x00215498 File Offset: 0x00213898
	public void ChangeToJetpack()
	{
		this.controlScheme = ArcadePlayerController.ControlScheme.Jetpack;
		this.weaponManager.ChangeToJetPack();
		this.animationController.ChangeToJetpack();
		base.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x06003B8D RID: 15245 RVA: 0x002154EC File Offset: 0x002138EC
	public void PauseAll()
	{
		foreach (AbstractPausableComponent abstractPausableComponent in base.GetComponents<AbstractPausableComponent>())
		{
			abstractPausableComponent.enabled = false;
		}
	}

	// Token: 0x06003B8E RID: 15246 RVA: 0x00215520 File Offset: 0x00213920
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

	// Token: 0x06003B8F RID: 15247 RVA: 0x00215560 File Offset: 0x00213960
	protected override void LevelInit(PlayerId id)
	{
		base.LevelInit(id);
		this.animationController.LevelInit();
		this.weaponManager.LevelInit(id);
	}

	// Token: 0x06003B90 RID: 15248 RVA: 0x00215580 File Offset: 0x00213980
	protected override void OnDeath(PlayerId playerId)
	{
		base.OnDeath(base.id);
		PlayerDeathEffect playerDeathEffect = this.deathEffect.Create(base.id, base.input, base.transform.position, base.stats.Deaths, PlayerMode.Level, true);
		playerDeathEffect.OnPreReviveEvent += this.OnPreRevive;
		playerDeathEffect.OnReviveEvent += this.OnRevive;
	}

	// Token: 0x06003B91 RID: 15249 RVA: 0x002155F4 File Offset: 0x002139F4
	public void DisableInput()
	{
		this.motor.DisableInput();
		this.weaponManager.DisableInput();
	}

	// Token: 0x06003B92 RID: 15250 RVA: 0x0021560C File Offset: 0x00213A0C
	public void OnLevelWinPause()
	{
		this.PauseAll();
		base.collider.enabled = false;
	}

	// Token: 0x06003B93 RID: 15251 RVA: 0x00215620 File Offset: 0x00213A20
	public override void OnLevelWin()
	{
		this.UnpauseAll(false);
		this.weaponManager.DisableInput();
		base.collider.enabled = false;
	}

	// Token: 0x06003B94 RID: 15252 RVA: 0x00215640 File Offset: 0x00213A40
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Application.isPlaying)
		{
			Gizmos.DrawCube(this.CameraCenter, Vector3.one * 50f);
		}
	}

	// Token: 0x06003B95 RID: 15253 RVA: 0x0021566C File Offset: 0x00213A6C
	public override void BufferInputs()
	{
		base.BufferInputs();
		this.motor.BufferInputs();
	}

	// Token: 0x04004311 RID: 17169
	private bool initialized;

	// Token: 0x04004312 RID: 17170
	private ArcadePlayerMotor _motor;

	// Token: 0x04004313 RID: 17171
	private ArcadePlayerAnimationController _animationController;

	// Token: 0x04004314 RID: 17172
	private ArcadePlayerWeaponManager _weaponManager;

	// Token: 0x04004315 RID: 17173
	private ArcadePlayerParryController _parryController;

	// Token: 0x04004316 RID: 17174
	private ArcadePlayerColliderManager _colliderManager;

	// Token: 0x04004317 RID: 17175
	[SerializeField]
	private PlayerDeathEffect deathEffect;

	// Token: 0x020009DE RID: 2526
	public enum ControlScheme
	{
		// Token: 0x0400431A RID: 17178
		Normal,
		// Token: 0x0400431B RID: 17179
		Rocket,
		// Token: 0x0400431C RID: 17180
		Jetpack
	}
}
