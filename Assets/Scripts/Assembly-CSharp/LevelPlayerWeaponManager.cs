using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A39 RID: 2617
public class LevelPlayerWeaponManager : AbstractLevelPlayerComponent
{
	// Token: 0x17000564 RID: 1380
	// (get) Token: 0x06003E49 RID: 15945 RVA: 0x00224055 File Offset: 0x00222455
	// (set) Token: 0x06003E4A RID: 15946 RVA: 0x0022405D File Offset: 0x0022245D
	public bool IsShooting { get; set; }

	// Token: 0x17000565 RID: 1381
	// (get) Token: 0x06003E4B RID: 15947 RVA: 0x00224066 File Offset: 0x00222466
	// (set) Token: 0x06003E4C RID: 15948 RVA: 0x0022406E File Offset: 0x0022246E
	public bool FreezePosition { get; set; }

	// Token: 0x17000566 RID: 1382
	// (get) Token: 0x06003E4D RID: 15949 RVA: 0x00224077 File Offset: 0x00222477
	public Vector2 ExPosition
	{
		get
		{
			return this.exRoot.position;
		}
	}

	// Token: 0x17000567 RID: 1383
	// (get) Token: 0x06003E4E RID: 15950 RVA: 0x00224089 File Offset: 0x00222489
	public AbstractLevelWeapon CurrentWeapon
	{
		get
		{
			return this.weaponPrefabs.GetWeapon(this.currentWeapon);
		}
	}

	// Token: 0x14000095 RID: 149
	// (add) Token: 0x06003E4F RID: 15951 RVA: 0x0022409C File Offset: 0x0022249C
	// (remove) Token: 0x06003E50 RID: 15952 RVA: 0x002240D4 File Offset: 0x002224D4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event LevelPlayerWeaponManager.OnWeaponChangeHandler OnWeaponChangeEvent;

	// Token: 0x14000096 RID: 150
	// (add) Token: 0x06003E51 RID: 15953 RVA: 0x0022410C File Offset: 0x0022250C
	// (remove) Token: 0x06003E52 RID: 15954 RVA: 0x00224144 File Offset: 0x00222544
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBasicStart;

	// Token: 0x14000097 RID: 151
	// (add) Token: 0x06003E53 RID: 15955 RVA: 0x0022417C File Offset: 0x0022257C
	// (remove) Token: 0x06003E54 RID: 15956 RVA: 0x002241B4 File Offset: 0x002225B4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExStart;

	// Token: 0x14000098 RID: 152
	// (add) Token: 0x06003E55 RID: 15957 RVA: 0x002241EC File Offset: 0x002225EC
	// (remove) Token: 0x06003E56 RID: 15958 RVA: 0x00224224 File Offset: 0x00222624
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperStart;

	// Token: 0x14000099 RID: 153
	// (add) Token: 0x06003E57 RID: 15959 RVA: 0x0022425C File Offset: 0x0022265C
	// (remove) Token: 0x06003E58 RID: 15960 RVA: 0x00224294 File Offset: 0x00222694
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExFire;

	// Token: 0x1400009A RID: 154
	// (add) Token: 0x06003E59 RID: 15961 RVA: 0x002242CC File Offset: 0x002226CC
	// (remove) Token: 0x06003E5A RID: 15962 RVA: 0x00224304 File Offset: 0x00222704
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnWeaponFire;

	// Token: 0x1400009B RID: 155
	// (add) Token: 0x06003E5B RID: 15963 RVA: 0x0022433C File Offset: 0x0022273C
	// (remove) Token: 0x06003E5C RID: 15964 RVA: 0x00224374 File Offset: 0x00222774
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExEnd;

	// Token: 0x1400009C RID: 156
	// (add) Token: 0x06003E5D RID: 15965 RVA: 0x002243AC File Offset: 0x002227AC
	// (remove) Token: 0x06003E5E RID: 15966 RVA: 0x002243E4 File Offset: 0x002227E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperEnd;

	// Token: 0x1400009D RID: 157
	// (add) Token: 0x06003E5F RID: 15967 RVA: 0x0022441C File Offset: 0x0022281C
	// (remove) Token: 0x06003E60 RID: 15968 RVA: 0x00224454 File Offset: 0x00222854
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperInterrupt;

	// Token: 0x17000568 RID: 1384
	// (get) Token: 0x06003E61 RID: 15969 RVA: 0x0022448A File Offset: 0x0022288A
	// (set) Token: 0x06003E62 RID: 15970 RVA: 0x00224492 File Offset: 0x00222892
	public AbstractPlayerSuper activeSuper { get; private set; }

	// Token: 0x06003E63 RID: 15971 RVA: 0x0022449C File Offset: 0x0022289C
	protected override void OnAwake()
	{
		base.OnAwake();
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnLevelEndEvent += this.OnLevelEnd;
		base.player.motor.OnDashStartEvent += this.OnDash;
		this.basic = new LevelPlayerWeaponManager.WeaponState();
		this.ex = new LevelPlayerWeaponManager.ExState();
		this.weaponsRoot = new GameObject("Weapons").transform;
		this.weaponsRoot.parent = base.transform;
		this.weaponsRoot.localPosition = Vector3.zero;
		this.weaponsRoot.localEulerAngles = Vector3.zero;
		this.weaponsRoot.localScale = Vector3.one;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x06003E64 RID: 15972 RVA: 0x0022459C File Offset: 0x0022299C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (Level.Current != null)
		{
			Level.Current.OnLevelEndEvent -= this.OnLevelEnd;
		}
		if (base.player != null && base.player.motor != null)
		{
			base.player.motor.OnDashStartEvent -= this.OnDash;
		}
		this.weaponPrefabs.OnDestroy();
		this.superPrefabs.OnDestroy();
		this.exDustEffect = null;
		this.exChargeEffect = null;
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06003E65 RID: 15973 RVA: 0x00224644 File Offset: 0x00222A44
	private void FixedUpdate()
	{
		if (!base.player.levelStarted || !this.allowInput)
		{
			return;
		}
		this.HandleWeaponSwitch();
		this.HandleWeaponFiring();
		if (base.player.motor.Grounded)
		{
			this.ex.airAble = true;
		}
	}

	// Token: 0x06003E66 RID: 15974 RVA: 0x0022469A File Offset: 0x00222A9A
	private void OnEnable()
	{
		this.EnableInput();
	}

	// Token: 0x06003E67 RID: 15975 RVA: 0x002246A2 File Offset: 0x00222AA2
	public void ForceStopWeaponFiring()
	{
		this.EndBasic();
	}

	// Token: 0x06003E68 RID: 15976 RVA: 0x002246AA File Offset: 0x00222AAA
	public override void OnLevelEnd()
	{
		this.EndBasic();
		base.OnLevelEnd();
	}

	// Token: 0x06003E69 RID: 15977 RVA: 0x002246B8 File Offset: 0x00222AB8
	private void OnDash()
	{
		this.EndBasic();
	}

	// Token: 0x06003E6A RID: 15978 RVA: 0x002246C0 File Offset: 0x00222AC0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.ex.firing && !base.player.stats.SuperInvincible)
		{
			this.ex.firing = false;
		}
	}

	// Token: 0x06003E6B RID: 15979 RVA: 0x002246F3 File Offset: 0x00222AF3
	public void AbortEX()
	{
		this.ex.firing = false;
	}

	// Token: 0x06003E6C RID: 15980 RVA: 0x00224701 File Offset: 0x00222B01
	public void ParrySuccess()
	{
	}

	// Token: 0x06003E6D RID: 15981 RVA: 0x00224704 File Offset: 0x00222B04
	public void LevelInit(PlayerId id)
	{
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id);
		if (playerLoadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1)
		{
			int[] availableWeaponIDs = WeaponProperties.CharmCurse.availableWeaponIDs;
			this.currentWeapon = (Weapon)availableWeaponIDs[UnityEngine.Random.Range(0, availableWeaponIDs.Length)];
		}
		else
		{
			this.currentWeapon = playerLoadout.primaryWeapon;
		}
		this.weaponPrefabs.Init(this, this.weaponsRoot);
		this.superPrefabs.Init(base.player);
	}

	// Token: 0x06003E6E RID: 15982 RVA: 0x0022479D File Offset: 0x00222B9D
	public void OnDeath()
	{
		this.EndBasic();
	}

	// Token: 0x06003E6F RID: 15983 RVA: 0x002247A5 File Offset: 0x00222BA5
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x06003E70 RID: 15984 RVA: 0x002247AE File Offset: 0x00222BAE
	public void DisableInput()
	{
		this.allowInput = false;
		this.IsShooting = false;
	}

	// Token: 0x06003E71 RID: 15985 RVA: 0x002247BE File Offset: 0x00222BBE
	public void EnableSuper(bool value)
	{
		this.allowSuper = value;
	}

	// Token: 0x06003E72 RID: 15986 RVA: 0x002247C7 File Offset: 0x00222BC7
	private void _WeaponFireEx()
	{
		this.FireEx();
	}

	// Token: 0x06003E73 RID: 15987 RVA: 0x002247CF File Offset: 0x00222BCF
	private void _WeaponEndEx()
	{
		this.EndEx();
	}

	// Token: 0x06003E74 RID: 15988 RVA: 0x002247D7 File Offset: 0x00222BD7
	private void StartBasic()
	{
		this.UpdateAim();
		if (!Level.IsChessBoss)
		{
			this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginBasic();
		}
		if (this.OnBasicStart != null)
		{
			this.OnBasicStart();
		}
	}

	// Token: 0x06003E75 RID: 15989 RVA: 0x00224815 File Offset: 0x00222C15
	private void EndBasic()
	{
		if (this.currentWeapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.basic.firing = false;
	}

	// Token: 0x06003E76 RID: 15990 RVA: 0x0022484A File Offset: 0x00222C4A
	public void TriggerWeaponFire()
	{
		this.OnWeaponFire();
	}

	// Token: 0x06003E77 RID: 15991 RVA: 0x00224857 File Offset: 0x00222C57
	public void InterruptSuper()
	{
		if (this.OnSuperInterrupt != null)
		{
			this.OnSuperInterrupt();
		}
	}

	// Token: 0x06003E78 RID: 15992 RVA: 0x00224870 File Offset: 0x00222C70
	private void StartEx()
	{
		this.EndBasic();
		this.UpdateAim();
		this.ex.firing = true;
		this.ex.airAble = false;
		base.player.stats.OnEx();
		this.exChargeEffect.Create(base.player.center);
		if (this.OnExStart != null)
		{
			this.OnExStart();
		}
	}

	// Token: 0x06003E79 RID: 15993 RVA: 0x002248DE File Offset: 0x00222CDE
	private void FireEx()
	{
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFire != null)
		{
			this.OnExFire();
		}
	}

	// Token: 0x06003E7A RID: 15994 RVA: 0x0022490C File Offset: 0x00222D0C
	private void EndEx()
	{
		this.ex.firing = false;
		if (this.OnExEnd != null)
		{
			this.OnExEnd();
		}
	}

	// Token: 0x06003E7B RID: 15995 RVA: 0x00224930 File Offset: 0x00222D30
	public void CreateExDust(Effect starsEffect)
	{
		Transform transform = new GameObject("ExRootTemp").transform;
		transform.ResetLocalTransforms();
		transform.position = this.exRoot.position;
		Vector2 v = transform.position;
		if (starsEffect != null)
		{
			Transform transform2 = starsEffect.Create(v).transform;
			transform2.SetParent(transform);
			transform2.ResetLocalTransforms();
			transform2.SetParent(null);
			transform2.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform2.localScale = this.GetBulletScale();
			transform2.AddPositionForward2D(-100f);
		}
		if (this.exDustEffect != null)
		{
			Transform transform3 = this.exDustEffect.Create(v).transform;
			transform3.SetParent(transform);
			transform3.ResetLocalTransforms();
			transform3.SetParent(null);
			transform3.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.GetBulletRotation()));
			transform3.localScale = this.GetBulletScale();
			transform3.AddPositionForward2D(-15f);
		}
		UnityEngine.Object.Destroy(transform.gameObject);
	}

	// Token: 0x06003E7C RID: 15996 RVA: 0x00224A64 File Offset: 0x00222E64
	private void StartSuper()
	{
		this.EndBasic();
		this.UpdateAim();
		base.player.stats.OnSuper();
		Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
		if (base.player.stats.isChalice)
		{
			if (super != Super.level_super_beam)
			{
				if (super != Super.level_super_ghost)
				{
					if (super == Super.level_super_invincible)
					{
						super = Super.level_super_chalice_shield;
					}
				}
				else
				{
					super = Super.level_super_chalice_iii;
				}
			}
			else
			{
				super = Super.level_super_chalice_vert_beam;
			}
		}
		AbstractPlayerSuper abstractPlayerSuper = this.superPrefabs.GetPrefab(super).Create(base.player);
		abstractPlayerSuper.OnEndedEvent += this.EndSuperFromSuper;
		this.activeSuper = abstractPlayerSuper;
		if (this.OnSuperStart != null)
		{
			this.OnSuperStart();
		}
	}

	// Token: 0x06003E7D RID: 15997 RVA: 0x00224B50 File Offset: 0x00222F50
	private void EndSuper()
	{
		if (this.OnSuperEnd != null)
		{
			this.OnSuperEnd();
		}
	}

	// Token: 0x06003E7E RID: 15998 RVA: 0x00224B68 File Offset: 0x00222F68
	public void EndSuperFromSuper()
	{
		this.EndSuper();
	}

	// Token: 0x06003E7F RID: 15999 RVA: 0x00224B70 File Offset: 0x00222F70
	private void HandleWeaponFiring()
	{
		if (base.player.motor.Dashing || base.player.motor.IsHit)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4) || base.player.motor.HasBufferedInput(LevelPlayerMotor.BufferedInput.Super) || (base.player.stats.Loadout.charm == Charm.charm_EX && base.player.input.actions.GetButton(3) && !this.ex.firing))
		{
			base.player.motor.ClearBufferedInput();
			Super super = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).super;
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax && super != Super.None && !base.player.stats.ChaliceShieldOn && this.allowSuper && base.player.stats.Loadout.charm != Charm.charm_EX)
			{
				this.StartSuper();
				return;
			}
			if (base.player.stats.CanUseEx && this.ex.Able)
			{
				this.StartEx();
				return;
			}
		}
		if (this.ex.firing || base.player.stats.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		if (this.basic.firing != base.player.input.actions.GetButton(3))
		{
			if (base.player.input.actions.GetButton(3))
			{
				if (PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id).charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1)
				{
					int[] availableWeaponIDs = WeaponProperties.CharmCurse.availableWeaponIDs;
					int num;
					for (num = (int)this.currentWeapon; num == (int)this.currentWeapon; num = availableWeaponIDs[UnityEngine.Random.Range(0, availableWeaponIDs.Length)])
					{
					}
					this.SwitchWeapon((Weapon)num);
				}
				else
				{
					this.StartBasic();
				}
			}
			else
			{
				this.EndBasic();
			}
		}
		this.basic.firing = base.player.input.actions.GetButton(3);
	}

	// Token: 0x06003E80 RID: 16000 RVA: 0x00224E0D File Offset: 0x0022320D
	public void ResetEx()
	{
		this.ex.firing = false;
	}

	// Token: 0x06003E81 RID: 16001 RVA: 0x00224E1C File Offset: 0x0022321C
	private void HandleWeaponSwitch()
	{
		if (base.player.input.actions.GetButtonDown(5))
		{
			PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(base.player.id);
			if ((playerLoadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel > -1) || playerLoadout.secondaryWeapon == Weapon.None)
			{
				return;
			}
			if (this.currentWeapon == playerLoadout.primaryWeapon)
			{
				this.SwitchWeapon(playerLoadout.secondaryWeapon);
			}
			else
			{
				this.SwitchWeapon(playerLoadout.primaryWeapon);
			}
		}
	}

	// Token: 0x06003E82 RID: 16002 RVA: 0x00224EC4 File Offset: 0x002232C4
	private void SwitchWeapon(Weapon weapon)
	{
		if (weapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndEx();
		this.currentWeapon = weapon;
		if (this.OnWeaponChangeEvent != null)
		{
			this.OnWeaponChangeEvent(weapon);
		}
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x06003E83 RID: 16003 RVA: 0x00224F48 File Offset: 0x00223348
	private LevelPlayerWeaponManager.Pose GetCurrentPose()
	{
		if (this.ex.firing)
		{
			return LevelPlayerWeaponManager.Pose.Ex;
		}
		if (base.player.motor.Ducking)
		{
			return LevelPlayerWeaponManager.Pose.Duck;
		}
		if (!base.player.motor.Grounded)
		{
			return LevelPlayerWeaponManager.Pose.Jump;
		}
		if (base.player.motor.Locked)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return LevelPlayerWeaponManager.Pose.Up_D;
				}
				return LevelPlayerWeaponManager.Pose.Up;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return LevelPlayerWeaponManager.Pose.Down_D;
				}
				return LevelPlayerWeaponManager.Pose.Down;
			}
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				return LevelPlayerWeaponManager.Pose.Up_D_R;
			}
			return LevelPlayerWeaponManager.Pose.Forward_R;
		}
		else
		{
			if (base.player.motor.LookDirection.y < 0)
			{
				return LevelPlayerWeaponManager.Pose.Duck;
			}
			if (base.player.motor.LookDirection.y > 0)
			{
				return LevelPlayerWeaponManager.Pose.Up;
			}
		}
		return LevelPlayerWeaponManager.Pose.Forward;
	}

	// Token: 0x06003E84 RID: 16004 RVA: 0x002250DC File Offset: 0x002234DC
	public LevelPlayerWeaponManager.Pose GetDirectionPose()
	{
		if (base.player.motor.Dashing)
		{
			return LevelPlayerWeaponManager.Pose.Forward;
		}
		if (base.player.motor.LookDirection.y > 0)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				return LevelPlayerWeaponManager.Pose.Up_D;
			}
			return LevelPlayerWeaponManager.Pose.Up;
		}
		else
		{
			if (base.player.motor.LookDirection.y >= 0)
			{
				return LevelPlayerWeaponManager.Pose.Forward;
			}
			if (base.player.motor.LookDirection.x != 0)
			{
				return LevelPlayerWeaponManager.Pose.Down_D;
			}
			return LevelPlayerWeaponManager.Pose.Down;
		}
	}

	// Token: 0x06003E85 RID: 16005 RVA: 0x00225194 File Offset: 0x00223594
	public void UpdateAim()
	{
		LevelPlayerWeaponManager.Pose directionPose = this.GetDirectionPose();
		float num;
		if (base.transform.localScale.x > 0f)
		{
			switch (directionPose)
			{
			default:
				num = 0f;
				break;
			case LevelPlayerWeaponManager.Pose.Up:
				num = 90f;
				break;
			case LevelPlayerWeaponManager.Pose.Up_D:
				num = 45f;
				break;
			case LevelPlayerWeaponManager.Pose.Down:
				num = -90f;
				break;
			case LevelPlayerWeaponManager.Pose.Down_D:
				num = -45f;
				break;
			}
		}
		else
		{
			switch (directionPose)
			{
			default:
				num = 180f;
				break;
			case LevelPlayerWeaponManager.Pose.Up:
				num = 90f;
				break;
			case LevelPlayerWeaponManager.Pose.Up_D:
				num = 135f;
				break;
			case LevelPlayerWeaponManager.Pose.Down:
				num = -90f;
				break;
			case LevelPlayerWeaponManager.Pose.Down_D:
				num = -135f;
				break;
			}
		}
		num *= base.player.motor.GravityReversalMultiplier;
		this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
	}

	// Token: 0x06003E86 RID: 16006 RVA: 0x002252C4 File Offset: 0x002236C4
	public Vector2 GetBulletPosition()
	{
		Vector2 vector = base.transform.position;
		Vector2 vector2 = LevelPlayerWeaponManager.ProjectilePosition.Get(this.GetCurrentPose(), this.GetDirectionPose(), base.player.stats.isChalice);
		return new Vector2(vector.x + vector2.x * base.player.motor.TrueLookDirection.x, vector.y + vector2.y * base.player.motor.GravityReversalMultiplier);
	}

	// Token: 0x06003E87 RID: 16007 RVA: 0x00225358 File Offset: 0x00223758
	public float GetBulletRotation()
	{
		LevelPlayerWeaponManager.Pose pose = this.GetCurrentPose();
		if (pose != LevelPlayerWeaponManager.Pose.Duck)
		{
			return this.aim.eulerAngles.z;
		}
		if (base.transform.localScale.x < 0f)
		{
			return 180f;
		}
		return 0f;
	}

	// Token: 0x06003E88 RID: 16008 RVA: 0x002253B0 File Offset: 0x002237B0
	public Vector3 GetBulletScale()
	{
		return new Vector3(1f, base.player.motor.TrueLookDirection.x, 1f);
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x002253EC File Offset: 0x002237EC
	private void WORKAROUND_NullifyFields()
	{
		this.activeSuper = null;
		this.weaponPrefabs = null;
		this.superPrefabs = null;
		this.exDustEffect = null;
		this.exChargeEffect = null;
		this.exRoot = null;
		this.OnWeaponChangeEvent = null;
		this.OnBasicStart = null;
		this.OnExStart = null;
		this.OnSuperStart = null;
		this.OnExFire = null;
		this.OnWeaponFire = null;
		this.OnExEnd = null;
		this.OnSuperEnd = null;
		this.OnSuperInterrupt = null;
		this.basic = null;
		this.ex = null;
		this.weaponsRoot = null;
		this.aim = null;
	}

	// Token: 0x0400456D RID: 17773
	[SerializeField]
	private LevelPlayerWeaponManager.WeaponPrefabs weaponPrefabs;

	// Token: 0x0400456E RID: 17774
	[SerializeField]
	private LevelPlayerWeaponManager.SuperPrefabs superPrefabs;

	// Token: 0x0400456F RID: 17775
	[Space(10f)]
	[SerializeField]
	private Effect exDustEffect;

	// Token: 0x04004570 RID: 17776
	[SerializeField]
	private Effect exChargeEffect;

	// Token: 0x04004571 RID: 17777
	[SerializeField]
	private Transform exRoot;

	// Token: 0x04004574 RID: 17780
	private Weapon currentWeapon = Weapon.None;

	// Token: 0x04004575 RID: 17781
	private LevelPlayerWeaponManager.Pose currentPose;

	// Token: 0x0400457F RID: 17791
	private LevelPlayerWeaponManager.WeaponState basic;

	// Token: 0x04004580 RID: 17792
	private LevelPlayerWeaponManager.ExState ex;

	// Token: 0x04004581 RID: 17793
	private Transform weaponsRoot;

	// Token: 0x04004582 RID: 17794
	private Transform aim;

	// Token: 0x04004583 RID: 17795
	public bool allowInput = true;

	// Token: 0x04004584 RID: 17796
	private bool allowSuper = true;

	// Token: 0x02000A3A RID: 2618
	public enum Pose
	{
		// Token: 0x04004587 RID: 17799
		Forward,
		// Token: 0x04004588 RID: 17800
		Forward_R,
		// Token: 0x04004589 RID: 17801
		Up,
		// Token: 0x0400458A RID: 17802
		Up_D,
		// Token: 0x0400458B RID: 17803
		Up_D_R,
		// Token: 0x0400458C RID: 17804
		Down,
		// Token: 0x0400458D RID: 17805
		Down_D,
		// Token: 0x0400458E RID: 17806
		Duck,
		// Token: 0x0400458F RID: 17807
		Jump,
		// Token: 0x04004590 RID: 17808
		Ex
	}

	// Token: 0x02000A3B RID: 2619
	// (Invoke) Token: 0x06003E8B RID: 16011
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x02000A3C RID: 2620
	public struct ProjectilePosition
	{
		// Token: 0x06003E8E RID: 16014 RVA: 0x00225480 File Offset: 0x00223880
		public static Vector2 Get(LevelPlayerWeaponManager.Pose pose, LevelPlayerWeaponManager.Pose direction, bool isChalice)
		{
			if (pose == LevelPlayerWeaponManager.Pose.Jump)
			{
				switch (direction)
				{
				case LevelPlayerWeaponManager.Pose.Forward:
					return (!isChalice) ? new Vector2(78f, 64f) : new Vector2(85f, 70f);
				case LevelPlayerWeaponManager.Pose.Up:
					return (!isChalice) ? new Vector2(0f, 158f) : new Vector2(22f, 162f);
				case LevelPlayerWeaponManager.Pose.Up_D:
					return (!isChalice) ? new Vector2(71f, 107f) : new Vector2(66f, 117f);
				case LevelPlayerWeaponManager.Pose.Down:
					return (!isChalice) ? new Vector2(0f, -11f) : new Vector2(22f, 2f);
				case LevelPlayerWeaponManager.Pose.Down_D:
					return (!isChalice) ? new Vector2(71f, 20f) : new Vector2(66f, 31f);
				}
				return (!isChalice) ? new Vector2(0f, 0f) : new Vector2(0f, 0f);
			}
			switch (pose)
			{
			case LevelPlayerWeaponManager.Pose.Forward:
				return (!isChalice) ? new Vector2(78f, 64f) : new Vector2(100f, 63f);
			case LevelPlayerWeaponManager.Pose.Forward_R:
				return (!isChalice) ? new Vector2(70f, 46f) : new Vector2(62f, 51f);
			case LevelPlayerWeaponManager.Pose.Up:
				return (!isChalice) ? new Vector2(27f, 158f) : new Vector2(32f, 162f);
			case LevelPlayerWeaponManager.Pose.Up_D:
				return (!isChalice) ? new Vector2(71f, 107f) : new Vector2(78f, 112f);
			case LevelPlayerWeaponManager.Pose.Up_D_R:
				return (!isChalice) ? new Vector2(73f, 107f) : new Vector2(66f, 108f);
			case LevelPlayerWeaponManager.Pose.Down:
				return (!isChalice) ? new Vector2(28f, -11f) : new Vector2(32f, -6f);
			case LevelPlayerWeaponManager.Pose.Down_D:
				return (!isChalice) ? new Vector2(71f, 20f) : new Vector2(78f, 17f);
			case LevelPlayerWeaponManager.Pose.Duck:
				return (!isChalice) ? new Vector2(102f, 24f) : new Vector2(103f, 33f);
			default:
				return (!isChalice) ? new Vector2(0f, 54f) : new Vector2(0f, 54f);
			}
		}
	}

	// Token: 0x02000A3D RID: 2621
	public class WeaponState
	{
		// Token: 0x04004591 RID: 17809
		public LevelPlayerWeaponManager.WeaponState.State state;

		// Token: 0x04004592 RID: 17810
		public bool firing;

		// Token: 0x04004593 RID: 17811
		public bool holding;

		// Token: 0x02000A3E RID: 2622
		public enum State
		{
			// Token: 0x04004595 RID: 17813
			Ready,
			// Token: 0x04004596 RID: 17814
			Firing,
			// Token: 0x04004597 RID: 17815
			Fired,
			// Token: 0x04004598 RID: 17816
			Ended
		}
	}

	// Token: 0x02000A3F RID: 2623
	public class ExState
	{
		// Token: 0x17000569 RID: 1385
		// (get) Token: 0x06003E91 RID: 16017 RVA: 0x00225772 File Offset: 0x00223B72
		public bool Able
		{
			get
			{
				return this.airAble && !this.firing;
			}
		}

		// Token: 0x04004599 RID: 17817
		public bool airAble = true;

		// Token: 0x0400459A RID: 17818
		public bool firing;
	}

	// Token: 0x02000A40 RID: 2624
	[Serializable]
	public class WeaponPrefabs
	{
		// Token: 0x06003E93 RID: 16019 RVA: 0x00225794 File Offset: 0x00223B94
		public void Init(LevelPlayerWeaponManager weaponManager, Transform root)
		{
			this.weaponManager = weaponManager;
			this.root = root;
			this.weapons = new Dictionary<Weapon, AbstractLevelWeapon>();
			foreach (Weapon id in EnumUtils.GetValues<Weapon>())
			{
				if (id.ToString().ToLower().Contains("level"))
				{
					this.InitWeapon(id);
				}
			}
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x00225805 File Offset: 0x00223C05
		public AbstractLevelWeapon GetWeapon(Weapon weapon)
		{
			return this.weapons[weapon];
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00225814 File Offset: 0x00223C14
		private void InitWeapon(Weapon id)
		{
			AbstractLevelWeapon abstractLevelWeapon;
			if (id != Weapon.level_weapon_peashot)
			{
				if (id != Weapon.level_weapon_spreadshot)
				{
					if (id != Weapon.level_weapon_arc)
					{
						if (id != Weapon.level_weapon_homing)
						{
							if (id != Weapon.level_weapon_exploder)
							{
								if (id != Weapon.level_weapon_charge)
								{
									if (id != Weapon.level_weapon_boomerang)
									{
										if (id != Weapon.level_weapon_bouncer)
										{
											if (id != Weapon.level_weapon_wide_shot)
											{
												if (id != Weapon.level_weapon_upshot)
												{
													if (id != Weapon.level_weapon_crackshot)
													{
														if (id != Weapon.None)
														{
															return;
														}
														return;
													}
													else
													{
														abstractLevelWeapon = this.crackshot;
													}
												}
												else
												{
													abstractLevelWeapon = this.upShot;
												}
											}
											else
											{
												abstractLevelWeapon = this.wideShot;
											}
										}
										else
										{
											abstractLevelWeapon = this.bouncer;
										}
									}
									else
									{
										abstractLevelWeapon = this.boomerang;
									}
								}
								else
								{
									abstractLevelWeapon = this.charge;
								}
							}
							else
							{
								abstractLevelWeapon = this.exploder;
							}
						}
						else
						{
							abstractLevelWeapon = this.homing;
						}
					}
					else
					{
						abstractLevelWeapon = this.arc;
					}
				}
				else
				{
					abstractLevelWeapon = this.spread;
				}
			}
			else
			{
				abstractLevelWeapon = this.peashot;
			}
			if (abstractLevelWeapon == null)
			{
				return;
			}
			AbstractLevelWeapon abstractLevelWeapon2 = UnityEngine.Object.Instantiate<AbstractLevelWeapon>(abstractLevelWeapon);
			abstractLevelWeapon2.transform.parent = this.root.transform;
			abstractLevelWeapon2.Initialize(this.weaponManager, id);
			abstractLevelWeapon2.name = abstractLevelWeapon2.name.Replace("(Clone)", string.Empty);
			this.weapons[id] = abstractLevelWeapon2;
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x00225994 File Offset: 0x00223D94
		public void OnDestroy()
		{
			this.peashot = null;
			this.spread = null;
			this.arc = null;
			this.homing = null;
			this.exploder = null;
			this.charge = null;
			this.boomerang = null;
			this.bouncer = null;
			this.wideShot = null;
		}

		// Token: 0x0400459B RID: 17819
		[SerializeField]
		private WeaponPeashot peashot;

		// Token: 0x0400459C RID: 17820
		[SerializeField]
		private WeaponSpread spread;

		// Token: 0x0400459D RID: 17821
		[SerializeField]
		private WeaponArc arc;

		// Token: 0x0400459E RID: 17822
		[SerializeField]
		private WeaponHoming homing;

		// Token: 0x0400459F RID: 17823
		[SerializeField]
		private WeaponExploder exploder;

		// Token: 0x040045A0 RID: 17824
		[SerializeField]
		private WeaponCharge charge;

		// Token: 0x040045A1 RID: 17825
		[SerializeField]
		private WeaponBoomerang boomerang;

		// Token: 0x040045A2 RID: 17826
		[SerializeField]
		private WeaponBouncer bouncer;

		// Token: 0x040045A3 RID: 17827
		[SerializeField]
		private WeaponWideShot wideShot;

		// Token: 0x040045A4 RID: 17828
		[SerializeField]
		private WeaponUpshot upShot;

		// Token: 0x040045A5 RID: 17829
		[SerializeField]
		private WeaponCrackshot crackshot;

		// Token: 0x040045A6 RID: 17830
		private Transform root;

		// Token: 0x040045A7 RID: 17831
		private LevelPlayerWeaponManager weaponManager;

		// Token: 0x040045A8 RID: 17832
		private Dictionary<Weapon, AbstractLevelWeapon> weapons;
	}

	// Token: 0x02000A41 RID: 2625
	[Serializable]
	public class SuperPrefabs
	{
		// Token: 0x06003E98 RID: 16024 RVA: 0x002259E8 File Offset: 0x00223DE8
		public void Init(LevelPlayerController player)
		{
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x002259EC File Offset: 0x00223DEC
		public AbstractPlayerSuper GetPrefab(Super super)
		{
			if (super != Super.level_super_beam)
			{
				if (super == Super.level_super_ghost)
				{
					return this.ghost;
				}
				if (super == Super.level_super_invincible)
				{
					return this.invincible;
				}
				if (super == Super.level_super_chalice_iii)
				{
					return this.chaliceIII;
				}
				if (super == Super.level_super_chalice_vert_beam)
				{
					return this.chaliceVertBeam;
				}
				if (super == Super.level_super_chalice_shield)
				{
					return this.chaliceShield;
				}
			}
			return this.beam;
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x00225A69 File Offset: 0x00223E69
		public void OnDestroy()
		{
			this.beam = null;
			this.ghost = null;
			this.invincible = null;
		}

		// Token: 0x040045A9 RID: 17833
		[SerializeField]
		private AbstractPlayerSuper beam;

		// Token: 0x040045AA RID: 17834
		[SerializeField]
		private AbstractPlayerSuper ghost;

		// Token: 0x040045AB RID: 17835
		[SerializeField]
		private AbstractPlayerSuper invincible;

		// Token: 0x040045AC RID: 17836
		[SerializeField]
		private AbstractPlayerSuper chaliceIII;

		// Token: 0x040045AD RID: 17837
		[SerializeField]
		private AbstractPlayerSuper chaliceVertBeam;

		// Token: 0x040045AE RID: 17838
		[SerializeField]
		private AbstractPlayerSuper chaliceShield;
	}
}
