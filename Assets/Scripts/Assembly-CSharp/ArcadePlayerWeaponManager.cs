using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020009F7 RID: 2551
public class ArcadePlayerWeaponManager : AbstractArcadePlayerComponent
{
	// Token: 0x17000518 RID: 1304
	// (get) Token: 0x06003C3A RID: 15418 RVA: 0x00218C12 File Offset: 0x00217012
	// (set) Token: 0x06003C3B RID: 15419 RVA: 0x00218C1A File Offset: 0x0021701A
	public bool shotBullet { get; set; }

	// Token: 0x17000519 RID: 1305
	// (get) Token: 0x06003C3C RID: 15420 RVA: 0x00218C23 File Offset: 0x00217023
	// (set) Token: 0x06003C3D RID: 15421 RVA: 0x00218C2B File Offset: 0x0021702B
	public bool IsShooting { get; set; }

	// Token: 0x1700051A RID: 1306
	// (get) Token: 0x06003C3E RID: 15422 RVA: 0x00218C34 File Offset: 0x00217034
	// (set) Token: 0x06003C3F RID: 15423 RVA: 0x00218C3C File Offset: 0x0021703C
	public bool FreezePosition { get; set; }

	// Token: 0x1700051B RID: 1307
	// (get) Token: 0x06003C40 RID: 15424 RVA: 0x00218C45 File Offset: 0x00217045
	public Vector2 ExPosition
	{
		get
		{
			return this.exRoot.position;
		}
	}

	// Token: 0x14000080 RID: 128
	// (add) Token: 0x06003C41 RID: 15425 RVA: 0x00218C58 File Offset: 0x00217058
	// (remove) Token: 0x06003C42 RID: 15426 RVA: 0x00218C90 File Offset: 0x00217090
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnBasicStart;

	// Token: 0x14000081 RID: 129
	// (add) Token: 0x06003C43 RID: 15427 RVA: 0x00218CC8 File Offset: 0x002170C8
	// (remove) Token: 0x06003C44 RID: 15428 RVA: 0x00218D00 File Offset: 0x00217100
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExStart;

	// Token: 0x14000082 RID: 130
	// (add) Token: 0x06003C45 RID: 15429 RVA: 0x00218D38 File Offset: 0x00217138
	// (remove) Token: 0x06003C46 RID: 15430 RVA: 0x00218D70 File Offset: 0x00217170
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperStart;

	// Token: 0x14000083 RID: 131
	// (add) Token: 0x06003C47 RID: 15431 RVA: 0x00218DA8 File Offset: 0x002171A8
	// (remove) Token: 0x06003C48 RID: 15432 RVA: 0x00218DE0 File Offset: 0x002171E0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExFire;

	// Token: 0x14000084 RID: 132
	// (add) Token: 0x06003C49 RID: 15433 RVA: 0x00218E18 File Offset: 0x00217218
	// (remove) Token: 0x06003C4A RID: 15434 RVA: 0x00218E50 File Offset: 0x00217250
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnWeaponFire;

	// Token: 0x14000085 RID: 133
	// (add) Token: 0x06003C4B RID: 15435 RVA: 0x00218E88 File Offset: 0x00217288
	// (remove) Token: 0x06003C4C RID: 15436 RVA: 0x00218EC0 File Offset: 0x002172C0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExEnd;

	// Token: 0x14000086 RID: 134
	// (add) Token: 0x06003C4D RID: 15437 RVA: 0x00218EF8 File Offset: 0x002172F8
	// (remove) Token: 0x06003C4E RID: 15438 RVA: 0x00218F30 File Offset: 0x00217330
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnSuperEnd;

	// Token: 0x06003C4F RID: 15439 RVA: 0x00218F68 File Offset: 0x00217368
	protected override void OnAwake()
	{
		base.OnAwake();
		base.basePlayer.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.player.motor.OnDashStartEvent += this.OnDash;
		this.basic = new ArcadePlayerWeaponManager.WeaponState();
		this.ex = new ArcadePlayerWeaponManager.ExState();
		this.weaponsRoot = new GameObject("Weapons").transform;
		this.weaponsRoot.parent = base.transform;
		this.weaponsRoot.localPosition = Vector3.zero;
		this.weaponsRoot.localEulerAngles = Vector3.zero;
		this.weaponsRoot.localScale = Vector3.one;
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
	}

	// Token: 0x06003C50 RID: 15440 RVA: 0x00219050 File Offset: 0x00217450
	public void ChangeToRocket()
	{
		this.currentWeapon = Weapon.arcade_weapon_rocket_peashot;
		this.aim.SetLocalPosition(null, new float?(50f), null);
	}

	// Token: 0x06003C51 RID: 15441 RVA: 0x00219090 File Offset: 0x00217490
	public void ChangeToJetPack()
	{
		this.aim.SetLocalPosition(null, new float?(30f), null);
	}

	// Token: 0x06003C52 RID: 15442 RVA: 0x002190C4 File Offset: 0x002174C4
	private void FixedUpdate()
	{
		if (!base.player.levelStarted || !this.allowInput)
		{
			return;
		}
		this.HandleWeaponFiring();
		if (base.player.motor.Grounded)
		{
			this.ex.airAble = true;
		}
	}

	// Token: 0x06003C53 RID: 15443 RVA: 0x00219114 File Offset: 0x00217514
	private void OnEnable()
	{
		this.EnableInput();
	}

	// Token: 0x06003C54 RID: 15444 RVA: 0x0021911C File Offset: 0x0021751C
	public override void OnLevelEnd()
	{
		this.EndBasic();
		base.OnLevelEnd();
	}

	// Token: 0x06003C55 RID: 15445 RVA: 0x0021912A File Offset: 0x0021752A
	private void OnDash()
	{
		this.EndBasic();
	}

	// Token: 0x06003C56 RID: 15446 RVA: 0x00219132 File Offset: 0x00217532
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.ex.firing)
		{
			this.ex.firing = false;
		}
	}

	// Token: 0x06003C57 RID: 15447 RVA: 0x00219150 File Offset: 0x00217550
	public void ParrySuccess()
	{
	}

	// Token: 0x06003C58 RID: 15448 RVA: 0x00219152 File Offset: 0x00217552
	public void LevelInit(PlayerId id)
	{
		this.currentWeapon = Weapon.arcade_weapon_peashot;
		this.weaponPrefabs.Init(this, this.weaponsRoot);
		this.superPrefabs.Init(base.player);
	}

	// Token: 0x06003C59 RID: 15449 RVA: 0x00219182 File Offset: 0x00217582
	public void EnableInput()
	{
		this.allowInput = true;
	}

	// Token: 0x06003C5A RID: 15450 RVA: 0x0021918B File Offset: 0x0021758B
	public void DisableInput()
	{
		this.allowInput = false;
		this.IsShooting = false;
	}

	// Token: 0x06003C5B RID: 15451 RVA: 0x0021919B File Offset: 0x0021759B
	private void _WeaponFireEx()
	{
		this.FireEx();
	}

	// Token: 0x06003C5C RID: 15452 RVA: 0x002191A3 File Offset: 0x002175A3
	private void _WeaponEndEx()
	{
		this.EndEx();
	}

	// Token: 0x06003C5D RID: 15453 RVA: 0x002191AB File Offset: 0x002175AB
	private void StartBasic()
	{
		this.UpdateAim();
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginBasic();
		if (this.OnBasicStart != null)
		{
			this.OnBasicStart();
		}
	}

	// Token: 0x06003C5E RID: 15454 RVA: 0x002191DF File Offset: 0x002175DF
	private void EndBasic()
	{
		if (this.currentWeapon == Weapon.None)
		{
			return;
		}
		this.weaponPrefabs.GetWeapon(this.currentWeapon).EndBasic();
		this.basic.firing = false;
	}

	// Token: 0x06003C5F RID: 15455 RVA: 0x00219214 File Offset: 0x00217614
	public void TriggerWeaponFire()
	{
		this.OnWeaponFire();
	}

	// Token: 0x06003C60 RID: 15456 RVA: 0x00219224 File Offset: 0x00217624
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

	// Token: 0x06003C61 RID: 15457 RVA: 0x00219292 File Offset: 0x00217692
	private void FireEx()
	{
		this.weaponPrefabs.GetWeapon(this.currentWeapon).BeginEx();
		if (this.OnExFire != null)
		{
			this.OnExFire();
		}
	}

	// Token: 0x06003C62 RID: 15458 RVA: 0x002192C0 File Offset: 0x002176C0
	private void EndEx()
	{
		this.ex.firing = false;
		if (this.OnExEnd != null)
		{
			this.OnExEnd();
		}
	}

	// Token: 0x06003C63 RID: 15459 RVA: 0x002192E4 File Offset: 0x002176E4
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
			transform3.AddPositionForward2D(-100f);
		}
		UnityEngine.Object.Destroy(transform.gameObject);
	}

	// Token: 0x06003C64 RID: 15460 RVA: 0x00219415 File Offset: 0x00217815
	private void StartSuper()
	{
	}

	// Token: 0x06003C65 RID: 15461 RVA: 0x00219417 File Offset: 0x00217817
	private void EndSuper()
	{
	}

	// Token: 0x06003C66 RID: 15462 RVA: 0x00219419 File Offset: 0x00217819
	public void EndSuperFromSuper()
	{
		this.EndSuper();
	}

	// Token: 0x06003C67 RID: 15463 RVA: 0x00219424 File Offset: 0x00217824
	private void HandleWeaponFiring()
	{
		if (base.player.motor.Dashing || base.player.motor.IsHit)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(4))
		{
			if (base.player.stats.SuperMeter >= base.player.stats.SuperMeterMax)
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
		if (this.ex.firing)
		{
			return;
		}
		if (this.basic.firing != base.player.input.actions.GetButton(3))
		{
			if (base.player.input.actions.GetButton(3))
			{
				this.StartBasic();
			}
			else
			{
				this.EndBasic();
			}
		}
		this.basic.firing = base.player.input.actions.GetButton(3);
	}

	// Token: 0x06003C68 RID: 15464 RVA: 0x00219554 File Offset: 0x00217954
	private ArcadePlayerWeaponManager.Pose GetCurrentPose()
	{
		if (this.ex.firing)
		{
			return ArcadePlayerWeaponManager.Pose.Ex;
		}
		if (!base.player.motor.Grounded)
		{
			return ArcadePlayerWeaponManager.Pose.Jump;
		}
		if (base.player.motor.Locked)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return ArcadePlayerWeaponManager.Pose.Up_D;
				}
				return ArcadePlayerWeaponManager.Pose.Up;
			}
			else if (base.player.motor.LookDirection.y < 0)
			{
				if (base.player.motor.LookDirection.x != 0)
				{
					return ArcadePlayerWeaponManager.Pose.Down_D;
				}
				return ArcadePlayerWeaponManager.Pose.Down;
			}
		}
		else if (base.player.motor.LookDirection.x != 0)
		{
			if (base.player.motor.LookDirection.y > 0)
			{
				return ArcadePlayerWeaponManager.Pose.Up_D_R;
			}
			return ArcadePlayerWeaponManager.Pose.Forward_R;
		}
		else if (base.player.motor.LookDirection.y > 0)
		{
			return ArcadePlayerWeaponManager.Pose.Up;
		}
		return ArcadePlayerWeaponManager.Pose.Forward;
	}

	// Token: 0x06003C69 RID: 15465 RVA: 0x002196AC File Offset: 0x00217AAC
	public ArcadePlayerWeaponManager.Pose GetDirectionPose()
	{
		if (base.player.motor.Dashing)
		{
			return ArcadePlayerWeaponManager.Pose.Forward;
		}
		if (base.player.motor.LookDirection.y > 0)
		{
			if (base.player.motor.LookDirection.x != 0)
			{
				return ArcadePlayerWeaponManager.Pose.Up_D;
			}
			return ArcadePlayerWeaponManager.Pose.Up;
		}
		else
		{
			if (base.player.motor.LookDirection.y >= 0)
			{
				return ArcadePlayerWeaponManager.Pose.Forward;
			}
			if (base.player.motor.LookDirection.x != 0)
			{
				return ArcadePlayerWeaponManager.Pose.Down_D;
			}
			return ArcadePlayerWeaponManager.Pose.Down;
		}
	}

	// Token: 0x06003C6A RID: 15466 RVA: 0x00219764 File Offset: 0x00217B64
	public void UpdateAim()
	{
		if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Rocket)
		{
			this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(base.transform.up.normalized)));
		}
		else if (base.player.controlScheme == ArcadePlayerController.ControlScheme.Jetpack)
		{
			this.aim.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(base.player.motor.TrueLookDirection)));
		}
		else
		{
			this.aim.SetEulerAngles(new float?(0f), new float?(0f), new float?(90f));
		}
	}

	// Token: 0x06003C6B RID: 15467 RVA: 0x00219843 File Offset: 0x00217C43
	public Vector2 GetBulletPosition()
	{
		return this.aim.transform.position;
	}

	// Token: 0x06003C6C RID: 15468 RVA: 0x0021985C File Offset: 0x00217C5C
	public float GetBulletRotation()
	{
		return this.aim.eulerAngles.z;
	}

	// Token: 0x06003C6D RID: 15469 RVA: 0x0021987C File Offset: 0x00217C7C
	public Vector3 GetBulletScale()
	{
		return new Vector3(1f, base.player.motor.TrueLookDirection.x, 1f);
	}

	// Token: 0x040043B1 RID: 17329
	[SerializeField]
	private ArcadePlayerWeaponManager.WeaponPrefabs weaponPrefabs;

	// Token: 0x040043B2 RID: 17330
	[SerializeField]
	private ArcadePlayerWeaponManager.SuperPrefabs superPrefabs;

	// Token: 0x040043B3 RID: 17331
	[Space(10f)]
	[SerializeField]
	private Effect exDustEffect;

	// Token: 0x040043B4 RID: 17332
	[SerializeField]
	private Effect exChargeEffect;

	// Token: 0x040043B5 RID: 17333
	[SerializeField]
	private Transform exRoot;

	// Token: 0x040043B9 RID: 17337
	private Weapon currentWeapon = Weapon.None;

	// Token: 0x040043BA RID: 17338
	private ArcadePlayerWeaponManager.Pose currentPose;

	// Token: 0x040043C2 RID: 17346
	private ArcadePlayerWeaponManager.WeaponState basic;

	// Token: 0x040043C3 RID: 17347
	private ArcadePlayerWeaponManager.ExState ex;

	// Token: 0x040043C4 RID: 17348
	private Transform weaponsRoot;

	// Token: 0x040043C5 RID: 17349
	private Transform aim;

	// Token: 0x040043C6 RID: 17350
	private bool allowInput = true;

	// Token: 0x020009F8 RID: 2552
	public enum Pose
	{
		// Token: 0x040043C8 RID: 17352
		Forward,
		// Token: 0x040043C9 RID: 17353
		Forward_R,
		// Token: 0x040043CA RID: 17354
		Up,
		// Token: 0x040043CB RID: 17355
		Up_D,
		// Token: 0x040043CC RID: 17356
		Up_D_R,
		// Token: 0x040043CD RID: 17357
		Down,
		// Token: 0x040043CE RID: 17358
		Down_D,
		// Token: 0x040043CF RID: 17359
		Duck,
		// Token: 0x040043D0 RID: 17360
		Jump,
		// Token: 0x040043D1 RID: 17361
		Ex
	}

	// Token: 0x020009F9 RID: 2553
	// (Invoke) Token: 0x06003C6F RID: 15471
	public delegate void OnWeaponChangeHandler(Weapon weapon);

	// Token: 0x020009FA RID: 2554
	public struct ProjectilePosition
	{
		// Token: 0x06003C72 RID: 15474 RVA: 0x002198B5 File Offset: 0x00217CB5
		public static Vector2 Get(ArcadePlayerWeaponManager.Pose pose, ArcadePlayerWeaponManager.Pose direction)
		{
			if (pose == ArcadePlayerWeaponManager.Pose.Jump)
			{
				return new Vector2(0f, 105f);
			}
			return new Vector2(4f, 115f);
		}
	}

	// Token: 0x020009FB RID: 2555
	public class WeaponState
	{
		// Token: 0x040043D2 RID: 17362
		public ArcadePlayerWeaponManager.WeaponState.State state;

		// Token: 0x040043D3 RID: 17363
		public bool firing;

		// Token: 0x040043D4 RID: 17364
		public bool holding;

		// Token: 0x020009FC RID: 2556
		public enum State
		{
			// Token: 0x040043D6 RID: 17366
			Ready,
			// Token: 0x040043D7 RID: 17367
			Firing,
			// Token: 0x040043D8 RID: 17368
			Fired,
			// Token: 0x040043D9 RID: 17369
			Ended
		}
	}

	// Token: 0x020009FD RID: 2557
	public class ExState
	{
		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06003C75 RID: 15477 RVA: 0x002198F4 File Offset: 0x00217CF4
		public bool Able
		{
			get
			{
				return this.airAble && !this.firing;
			}
		}

		// Token: 0x040043DA RID: 17370
		public bool airAble = true;

		// Token: 0x040043DB RID: 17371
		public bool firing;
	}

	// Token: 0x020009FE RID: 2558
	[Serializable]
	public class WeaponPrefabs
	{
		// Token: 0x06003C77 RID: 15479 RVA: 0x00219915 File Offset: 0x00217D15
		public void Init(ArcadePlayerWeaponManager weaponManager, Transform root)
		{
			this.weaponManager = weaponManager;
			this.root = root;
			this.weapons = new Dictionary<Weapon, AbstractArcadeWeapon>();
			this.InitWeapon(Weapon.arcade_weapon_peashot);
			this.InitWeapon(Weapon.arcade_weapon_rocket_peashot);
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x00219946 File Offset: 0x00217D46
		public AbstractArcadeWeapon GetWeapon(Weapon weapon)
		{
			return this.weapons[weapon];
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x00219954 File Offset: 0x00217D54
		private void InitWeapon(Weapon id)
		{
			AbstractArcadeWeapon original = this.peashot;
			AbstractArcadeWeapon abstractArcadeWeapon = UnityEngine.Object.Instantiate<AbstractArcadeWeapon>(original);
			abstractArcadeWeapon.transform.parent = this.root.transform;
			abstractArcadeWeapon.Initialize(this.weaponManager, id);
			abstractArcadeWeapon.name = abstractArcadeWeapon.name.Replace("(Clone)", string.Empty);
			this.weapons[id] = abstractArcadeWeapon;
		}

		// Token: 0x040043DC RID: 17372
		[SerializeField]
		private ArcadeWeaponPeashot peashot;

		// Token: 0x040043DD RID: 17373
		[SerializeField]
		private ArcadeWeaponRocketPeashot rocketPeashot;

		// Token: 0x040043DE RID: 17374
		private Transform root;

		// Token: 0x040043DF RID: 17375
		private ArcadePlayerWeaponManager weaponManager;

		// Token: 0x040043E0 RID: 17376
		private Dictionary<Weapon, AbstractArcadeWeapon> weapons;
	}

	// Token: 0x020009FF RID: 2559
	[Serializable]
	public class SuperPrefabs
	{
		// Token: 0x06003C7B RID: 15483 RVA: 0x002199C4 File Offset: 0x00217DC4
		public void Init(ArcadePlayerController player)
		{
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x002199C6 File Offset: 0x00217DC6
		public AbstractPlayerSuper GetPrefab(Super super)
		{
			return null;
		}
	}
}
