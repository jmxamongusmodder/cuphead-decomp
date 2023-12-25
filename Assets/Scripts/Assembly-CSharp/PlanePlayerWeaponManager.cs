using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AA3 RID: 2723
public class PlanePlayerWeaponManager : AbstractPlanePlayerComponent
{
	// Token: 0x170005B6 RID: 1462
	// (get) Token: 0x0600414C RID: 16716 RVA: 0x002369A2 File Offset: 0x00234DA2
	// (set) Token: 0x0600414D RID: 16717 RVA: 0x002369AA File Offset: 0x00234DAA
	public PlanePlayerWeaponManager.State state { get; protected set; }

	// Token: 0x170005B7 RID: 1463
	// (get) Token: 0x0600414E RID: 16718 RVA: 0x002369B3 File Offset: 0x00234DB3
	public AbstractPlaneWeapon CurrentWeapon
	{
		get
		{
			return this.weapons.GetWeapon(this.currentWeapon);
		}
	}

	// Token: 0x170005B8 RID: 1464
	// (get) Token: 0x0600414F RID: 16719 RVA: 0x002369C6 File Offset: 0x00234DC6
	// (set) Token: 0x06004150 RID: 16720 RVA: 0x002369CE File Offset: 0x00234DCE
	public PlanePlayerWeaponManager.States states { get; private set; }

	// Token: 0x170005B9 RID: 1465
	// (get) Token: 0x06004151 RID: 16721 RVA: 0x002369D7 File Offset: 0x00234DD7
	// (set) Token: 0x06004152 RID: 16722 RVA: 0x002369DF File Offset: 0x00234DDF
	public bool CanInterupt { get; private set; }

	// Token: 0x06004153 RID: 16723 RVA: 0x002369E8 File Offset: 0x00234DE8
	private void Start()
	{
		this.weapons.Init(this);
		this.states = new PlanePlayerWeaponManager.States();
		base.player.animationController.OnExFireAnimEvent += this.OnExAnimFire;
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnPlayerDeathEvent += this.StopSound;
		this.CanInterupt = true;
	}

	// Token: 0x06004154 RID: 16724 RVA: 0x00236A62 File Offset: 0x00234E62
	private void FixedUpdate()
	{
		if (this.state == PlanePlayerWeaponManager.State.Inactive || this.state == PlanePlayerWeaponManager.State.Busy)
		{
			return;
		}
		this.CheckBasic();
		this.CheckEx();
		this.HandleWeaponSwitch();
	}

	// Token: 0x06004155 RID: 16725 RVA: 0x00236A90 File Offset: 0x00234E90
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		if (base.player.stats.isChalice)
		{
			this.currentWeapon = Weapon.plane_chalice_weapon_3way;
		}
		else if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0)
		{
			int[] availableShmupWeaponIDs = WeaponProperties.CharmCurse.availableShmupWeaponIDs;
			this.currentWeapon = (Weapon)availableShmupWeaponIDs[UnityEngine.Random.Range(0, availableShmupWeaponIDs.Length)];
		}
		if (base.player.stats.StoneTime > 0f)
		{
			return;
		}
		this.state = PlanePlayerWeaponManager.State.Ready;
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x06004156 RID: 16726 RVA: 0x00236B57 File Offset: 0x00234F57
	public override void OnLevelEnd()
	{
		base.OnLevelEnd();
		this.EndBasic();
	}

	// Token: 0x06004157 RID: 16727 RVA: 0x00236B68 File Offset: 0x00234F68
	public void SwitchToWeapon(Weapon weapon)
	{
		if (weapon == Weapon.None)
		{
			return;
		}
		this.weapons.GetWeapon(this.currentWeapon).EndBasic();
		this.weapons.GetWeapon(this.currentWeapon).EndEx();
		if (this.OnWeaponChangeEvent != null)
		{
			this.OnWeaponChangeEvent(weapon);
		}
		this.currentWeapon = weapon;
		if (base.player.input.actions.GetButton(3))
		{
			this.StartBasic();
		}
	}

	// Token: 0x140000A4 RID: 164
	// (add) Token: 0x06004158 RID: 16728 RVA: 0x00236BEC File Offset: 0x00234FEC
	// (remove) Token: 0x06004159 RID: 16729 RVA: 0x00236C24 File Offset: 0x00235024
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event PlanePlayerWeaponManager.OnWeaponChangeHandler OnWeaponChangeEvent;

	// Token: 0x140000A5 RID: 165
	// (add) Token: 0x0600415A RID: 16730 RVA: 0x00236C5C File Offset: 0x0023505C
	// (remove) Token: 0x0600415B RID: 16731 RVA: 0x00236C94 File Offset: 0x00235094
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExStartEvent;

	// Token: 0x140000A6 RID: 166
	// (add) Token: 0x0600415C RID: 16732 RVA: 0x00236CCC File Offset: 0x002350CC
	// (remove) Token: 0x0600415D RID: 16733 RVA: 0x00236D04 File Offset: 0x00235104
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExFireEvent;

	// Token: 0x140000A7 RID: 167
	// (add) Token: 0x0600415E RID: 16734 RVA: 0x00236D3C File Offset: 0x0023513C
	// (remove) Token: 0x0600415F RID: 16735 RVA: 0x00236D74 File Offset: 0x00235174
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperStartEvent;

	// Token: 0x140000A8 RID: 168
	// (add) Token: 0x06004160 RID: 16736 RVA: 0x00236DAC File Offset: 0x002351AC
	// (remove) Token: 0x06004161 RID: 16737 RVA: 0x00236DE4 File Offset: 0x002351E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperCountdownEvent;

	// Token: 0x140000A9 RID: 169
	// (add) Token: 0x06004162 RID: 16738 RVA: 0x00236E1C File Offset: 0x0023521C
	// (remove) Token: 0x06004163 RID: 16739 RVA: 0x00236E54 File Offset: 0x00235254
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperFireEvent;

	// Token: 0x06004164 RID: 16740 RVA: 0x00236E8A File Offset: 0x0023528A
	private void OnRevive(Vector3 pos)
	{
		this.IsShooting = false;
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.basic = PlanePlayerWeaponManager.States.Basic.Ready;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ready;
		this.CanInterupt = true;
	}

	// Token: 0x06004165 RID: 16741 RVA: 0x00236EB9 File Offset: 0x002352B9
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.weapons.OnDestroy();
		this.super = null;
	}

	// Token: 0x06004166 RID: 16742 RVA: 0x00236ED4 File Offset: 0x002352D4
	private void CheckBasic()
	{
		if (base.player.stats.Loadout.charm == Charm.charm_EX)
		{
			return;
		}
		if ((base.player.input.actions.GetButtonDown(3) || (base.player.input.actions.GetButtonTimePressed(3) > 0f && !this.IsShooting)) && base.player.stats.StoneTime <= 0f)
		{
			if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0 && !base.player.Shrunk)
			{
				int[] availableShmupWeaponIDs = WeaponProperties.CharmCurse.availableShmupWeaponIDs;
				int num;
				for (num = (int)this.currentWeapon; num == (int)this.currentWeapon; num = availableShmupWeaponIDs[UnityEngine.Random.Range(0, availableShmupWeaponIDs.Length)])
				{
				}
				this.SwitchWeapon((Weapon)num);
			}
			else
			{
				this.StartBasic();
			}
		}
		else if (base.player.input.actions.GetButtonUp(3) || (this.IsShooting && base.player.stats.StoneTime > 0f))
		{
			this.EndBasic();
		}
		else if (!base.player.input.actions.GetButton(3) && this.IsShooting)
		{
			this.EndBasic();
		}
		else if ((!base.player.Shrunk && this.unshrunkWeapon != Weapon.None) || (this.IsShooting && base.player.Shrunk && this.currentWeapon != Weapon.plane_weapon_peashot))
		{
			this.EndBasic();
			if (base.player.input.actions.GetButton(3))
			{
				this.StartBasic();
			}
		}
	}

	// Token: 0x06004167 RID: 16743 RVA: 0x002370D8 File Offset: 0x002354D8
	private void StartBasic()
	{
		if ((this.currentWeapon == Weapon.plane_weapon_bomb || this.currentWeapon == Weapon.plane_chalice_weapon_3way || this.currentWeapon == Weapon.plane_chalice_weapon_bomb) && base.player.Shrunk)
		{
			this.unshrunkWeapon = this.currentWeapon;
			this.currentWeapon = Weapon.plane_weapon_peashot;
		}
		this.weapons.GetWeapon(this.currentWeapon).BeginBasic();
	}

	// Token: 0x06004168 RID: 16744 RVA: 0x00237154 File Offset: 0x00235554
	private void EndBasic()
	{
		this.weapons.GetWeapon(this.currentWeapon).EndBasic();
		if (this.unshrunkWeapon != Weapon.None)
		{
			this.currentWeapon = this.unshrunkWeapon;
			this.unshrunkWeapon = Weapon.None;
		}
		this.StopSound(base.player.id);
	}

	// Token: 0x06004169 RID: 16745 RVA: 0x002371B0 File Offset: 0x002355B0
	private void StopSound(PlayerId id)
	{
		if ((id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			if (AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead"))
			{
				AudioManager.Stop("player_plane_weapon_fire_loop_cuphead");
				AudioManager.Play("player_plane_weapon_fire_loop_end_cuphead");
				this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_cuphead");
			}
		}
		else if (AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman"))
		{
			AudioManager.Stop("player_plane_weapon_fire_loop_mugman");
			AudioManager.Play("player_plane_weapon_fire_loop_end_mugman");
			this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_end_mugman");
		}
	}

	// Token: 0x0600416A RID: 16746 RVA: 0x0023724C File Offset: 0x0023564C
	private void CheckEx()
	{
		if (!base.player.stats.CanUseEx || base.player.Parrying || base.player.Shrunk || base.player.stats.StoneTime > 0f)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4) || base.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Super) || (base.player.stats.Loadout.charm == Charm.charm_EX && base.player.input.actions.GetButton(3)))
		{
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax && base.player.stats.Loadout.charm != Charm.charm_EX)
			{
				this.StartSuper();
			}
			else
			{
				this.StartEx();
			}
			base.player.motor.ClearBufferedInput();
		}
	}

	// Token: 0x0600416B RID: 16747 RVA: 0x00237379 File Offset: 0x00235779
	private void StartEx()
	{
		base.StartCoroutine(this.ex_cr());
	}

	// Token: 0x0600416C RID: 16748 RVA: 0x00237388 File Offset: 0x00235788
	private void OnExAnimFire()
	{
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Fire;
	}

	// Token: 0x0600416D RID: 16749 RVA: 0x00237398 File Offset: 0x00235798
	private IEnumerator ex_cr()
	{
		AudioManager.Play("player_plane_weapon_special_fire");
		this.state = PlanePlayerWeaponManager.State.Inactive;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Intro;
		this.CanInterupt = false;
		this.EndBasic();
		base.player.stats.OnEx();
		if (this.OnExStartEvent != null)
		{
			this.OnExStartEvent();
		}
		while (this.states.ex != PlanePlayerWeaponManager.States.Ex.Fire)
		{
			if (base.player.stats.StoneTime > 0f)
			{
				this.CancelEX();
				yield return null;
			}
			yield return null;
		}
		this.weapons.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFireEvent != null)
		{
			this.OnExFireEvent();
		}
		AudioManager.Play("player_plane_up_ex_end");
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ending;
		while (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Ex_End"))
		{
			if (base.player.stats.StoneTime > 0f)
			{
				this.CancelEX();
				yield return null;
			}
			yield return null;
		}
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.ex = PlanePlayerWeaponManager.States.Ex.Ready;
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
		yield break;
	}

	// Token: 0x0600416E RID: 16750 RVA: 0x002373B3 File Offset: 0x002357B3
	private void CancelEX()
	{
		base.StopCoroutine(this.ex_cr());
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x002373E9 File Offset: 0x002357E9
	public void StartSuper()
	{
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06004170 RID: 16752 RVA: 0x002373F8 File Offset: 0x002357F8
	private IEnumerator super_cr()
	{
		this.state = PlanePlayerWeaponManager.State.Inactive;
		this.states.super = PlanePlayerWeaponManager.States.Super.Ready;
		this.CanInterupt = false;
		this.EndBasic();
		base.player.stats.OnSuper();
		AbstractPlaneSuper s;
		if (base.player.stats.isChalice)
		{
			s = this.chaliceSuper.Create(base.player);
		}
		else
		{
			s = this.super.Create(base.player);
		}
		if (this.OnSuperStartEvent != null)
		{
			this.OnSuperStartEvent();
		}
		while (this.states.super != PlanePlayerWeaponManager.States.Super.Ending && this.states.super != PlanePlayerWeaponManager.States.Super.Countdown)
		{
			this.states.super = s.State;
			yield return null;
		}
		if (this.OnSuperCountdownEvent != null)
		{
			this.OnSuperCountdownEvent();
		}
		while (this.states.super != PlanePlayerWeaponManager.States.Super.Ending)
		{
			this.states.super = s.State;
			yield return null;
		}
		if (this.OnSuperFireEvent != null)
		{
			this.OnSuperFireEvent();
		}
		base.player.stats.OnSuperEnd();
		this.state = PlanePlayerWeaponManager.State.Ready;
		this.states.super = PlanePlayerWeaponManager.States.Super.Ready;
		if (base.player.input.actions.GetButtonDown(3))
		{
			this.StartBasic();
		}
		this.CanInterupt = true;
		yield break;
	}

	// Token: 0x06004171 RID: 16753 RVA: 0x00237413 File Offset: 0x00235813
	public Vector2 GetBulletPosition()
	{
		return this.bulletRoot.position;
	}

	// Token: 0x06004172 RID: 16754 RVA: 0x00237428 File Offset: 0x00235828
	private void HandleWeaponSwitch()
	{
		if (base.player.input.actions.GetButtonDown(5))
		{
			if (base.player.stats.Loadout.charm == Charm.charm_curse && base.player.stats.CurseCharmLevel >= 0)
			{
				return;
			}
			if (!PlayerData.Data.IsUnlocked(base.player.id, Weapon.plane_weapon_bomb) && !Level.IsTowerOfPower)
			{
				return;
			}
			if (base.player.stats.isChalice)
			{
				if (this.currentWeapon == Weapon.plane_chalice_weapon_3way)
				{
					this.SwitchWeapon(Weapon.plane_chalice_weapon_bomb);
				}
				else
				{
					this.SwitchWeapon(Weapon.plane_chalice_weapon_3way);
				}
			}
			else if (this.currentWeapon == Weapon.plane_weapon_peashot)
			{
				this.SwitchWeapon(Weapon.plane_weapon_bomb);
			}
			else
			{
				this.SwitchWeapon(Weapon.plane_weapon_peashot);
			}
		}
	}

	// Token: 0x06004173 RID: 16755 RVA: 0x00237520 File Offset: 0x00235920
	private void SwitchWeapon(Weapon weapon)
	{
		this.EndBasic();
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

	// Token: 0x040047E1 RID: 18401
	[SerializeField]
	private PlanePlayerWeaponManager.Weapons weapons;

	// Token: 0x040047E2 RID: 18402
	[SerializeField]
	private AbstractPlaneSuper super;

	// Token: 0x040047E3 RID: 18403
	[SerializeField]
	private AbstractPlaneSuper chaliceSuper;

	// Token: 0x040047E4 RID: 18404
	[Space(10f)]
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x040047E5 RID: 18405
	[NonSerialized]
	public bool IsShooting;

	// Token: 0x040047E6 RID: 18406
	private Weapon currentWeapon = Weapon.plane_weapon_peashot;

	// Token: 0x040047E9 RID: 18409
	private Weapon unshrunkWeapon = Weapon.None;

	// Token: 0x02000AA4 RID: 2724
	public enum State
	{
		// Token: 0x040047F1 RID: 18417
		Inactive,
		// Token: 0x040047F2 RID: 18418
		Ready,
		// Token: 0x040047F3 RID: 18419
		Busy
	}

	// Token: 0x02000AA5 RID: 2725
	// (Invoke) Token: 0x06004175 RID: 16757
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x02000AA6 RID: 2726
	[Serializable]
	public class Weapons
	{
		// Token: 0x06004179 RID: 16761 RVA: 0x0023757C File Offset: 0x0023597C
		public void Init(PlanePlayerWeaponManager manager)
		{
			this.peashot = UnityEngine.Object.Instantiate<AbstractPlaneWeapon>(this.peashot);
			this.peashot.Initialize(manager, 0);
			this.peashot.transform.SetParent(manager.transform);
			this.peashot.transform.ResetLocalTransforms();
			this.bomb = UnityEngine.Object.Instantiate<AbstractPlaneWeapon>(this.bomb);
			this.bomb.Initialize(manager, 2);
			this.bomb.transform.SetParent(manager.transform);
			this.bomb.transform.ResetLocalTransforms();
			this.chalice3Way = UnityEngine.Object.Instantiate<AbstractPlaneWeapon>(this.chalice3Way);
			this.chalice3Way.Initialize(manager, 3);
			this.chalice3Way.transform.SetParent(manager.transform);
			this.chalice3Way.transform.ResetLocalTransforms();
			this.chaliceBomb = UnityEngine.Object.Instantiate<AbstractPlaneWeapon>(this.chaliceBomb);
			this.chaliceBomb.Initialize(manager, 4);
			this.chaliceBomb.transform.SetParent(manager.transform);
			this.chaliceBomb.transform.ResetLocalTransforms();
		}

		// Token: 0x0600417A RID: 16762 RVA: 0x0023769C File Offset: 0x00235A9C
		public AbstractPlaneWeapon GetWeapon(Weapon weapon)
		{
			if (weapon == Weapon.plane_weapon_peashot)
			{
				return this.peashot;
			}
			if (weapon == Weapon.plane_weapon_bomb)
			{
				return this.bomb;
			}
			if (weapon == Weapon.plane_chalice_weapon_3way)
			{
				return this.chalice3Way;
			}
			if (weapon != Weapon.plane_chalice_weapon_bomb)
			{
				return null;
			}
			return this.chaliceBomb;
		}

		// Token: 0x0600417B RID: 16763 RVA: 0x002376F7 File Offset: 0x00235AF7
		public void OnDestroy()
		{
			this.peashot = null;
			this.bomb = null;
		}

		// Token: 0x040047F4 RID: 18420
		public AbstractPlaneWeapon peashot;

		// Token: 0x040047F5 RID: 18421
		public AbstractPlaneWeapon bomb;

		// Token: 0x040047F6 RID: 18422
		public AbstractPlaneWeapon chalice3Way;

		// Token: 0x040047F7 RID: 18423
		public AbstractPlaneWeapon chaliceBomb;
	}

	// Token: 0x02000AA7 RID: 2727
	public class States
	{
		// Token: 0x0600417C RID: 16764 RVA: 0x00237707 File Offset: 0x00235B07
		public States()
		{
			this.basic = PlanePlayerWeaponManager.States.Basic.Ready;
			this.ex = PlanePlayerWeaponManager.States.Ex.Ready;
			this.super = PlanePlayerWeaponManager.States.Super.Ready;
		}

		// Token: 0x170005BA RID: 1466
		// (get) Token: 0x0600417D RID: 16765 RVA: 0x00237724 File Offset: 0x00235B24
		// (set) Token: 0x0600417E RID: 16766 RVA: 0x0023772C File Offset: 0x00235B2C
		public PlanePlayerWeaponManager.States.Basic basic { get; internal set; }

		// Token: 0x170005BB RID: 1467
		// (get) Token: 0x0600417F RID: 16767 RVA: 0x00237735 File Offset: 0x00235B35
		// (set) Token: 0x06004180 RID: 16768 RVA: 0x0023773D File Offset: 0x00235B3D
		public PlanePlayerWeaponManager.States.Ex ex { get; internal set; }

		// Token: 0x040047FA RID: 18426
		public PlanePlayerWeaponManager.States.Super super;

		// Token: 0x02000AA8 RID: 2728
		public enum Basic
		{
			// Token: 0x040047FC RID: 18428
			Ready
		}

		// Token: 0x02000AA9 RID: 2729
		public enum Ex
		{
			// Token: 0x040047FE RID: 18430
			Ready,
			// Token: 0x040047FF RID: 18431
			Intro,
			// Token: 0x04004800 RID: 18432
			Fire,
			// Token: 0x04004801 RID: 18433
			Ending
		}

		// Token: 0x02000AAA RID: 2730
		public enum Super
		{
			// Token: 0x04004803 RID: 18435
			Ready,
			// Token: 0x04004804 RID: 18436
			Intro,
			// Token: 0x04004805 RID: 18437
			Countdown,
			// Token: 0x04004806 RID: 18438
			Ending
		}
	}
}
