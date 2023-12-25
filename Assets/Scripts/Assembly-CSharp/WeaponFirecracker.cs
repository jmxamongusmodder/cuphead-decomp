using System;
using UnityEngine;

// Token: 0x02000A79 RID: 2681
public class WeaponFirecracker : AbstractLevelWeapon
{
	// Token: 0x17000589 RID: 1417
	// (get) Token: 0x06004011 RID: 16401 RVA: 0x0022F0B6 File Offset: 0x0022D4B6
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700058A RID: 1418
	// (get) Token: 0x06004012 RID: 16402 RVA: 0x0022F0B9 File Offset: 0x0022D4B9
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponFirecracker.Basic.fireRate;
		}
	}

	// Token: 0x06004013 RID: 16403 RVA: 0x0022F0C0 File Offset: 0x0022D4C0
	private void Start()
	{
		this.CreateDummyObject();
		this.explosionAngles = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionAngleString.Split(new char[]
		{
			','
		});
	}

	// Token: 0x06004014 RID: 16404 RVA: 0x0022F0E4 File Offset: 0x0022D4E4
	protected override AbstractProjectile fireBasic()
	{
		WeaponFirecrackerProjectile weaponFirecrackerProjectile = base.fireBasic() as WeaponFirecrackerProjectile;
		if (this.isTypeB)
		{
			weaponFirecrackerProjectile.explosionRadiusSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionsRadiusSize;
			float explosionAngle = 0f;
			Parser.FloatTryParse(this.explosionAngles[this.explosionAngleIndex], out explosionAngle);
			weaponFirecrackerProjectile.explosionAngle = explosionAngle;
			this.explosionAngleIndex = (this.explosionAngleIndex + 1) % this.explosionAngles.Length;
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletSpeed;
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDamage;
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Basic.bulletLife;
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionSize;
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Basic.explosionDuration;
		}
		else
		{
			weaponFirecrackerProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Basic.bulletSpeed;
			weaponFirecrackerProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDamage;
			weaponFirecrackerProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Basic.bulletLife;
			weaponFirecrackerProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Basic.explosionSize;
			weaponFirecrackerProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Basic.explosionDuration;
		}
		weaponFirecrackerProjectile.collider.enabled = false;
		weaponFirecrackerProjectile.PlayerId = this.player.id;
		weaponFirecrackerProjectile.DamagesType.PlayerProjectileDefault();
		weaponFirecrackerProjectile.CollisionDeath.PlayerProjectileDefault();
		this.dummyObject.transform.eulerAngles = this.player.transform.eulerAngles;
		this.dummyObject.transform.localScale = this.player.transform.localScale;
		weaponFirecrackerProjectile.transform.parent = this.dummyObject.transform;
		weaponFirecrackerProjectile.SetupFirecracker(this.dummyObject.transform, this.player, this.isTypeB);
		return weaponFirecrackerProjectile;
	}

	// Token: 0x06004015 RID: 16405 RVA: 0x0022F268 File Offset: 0x0022D668
	protected override AbstractProjectile fireEx()
	{
		WeaponFirecrackerEXProjectile weaponFirecrackerEXProjectile = base.fireEx() as WeaponFirecrackerEXProjectile;
		if (this.isTypeB)
		{
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecrackerB.Ex.exSpeed;
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecrackerB.Ex.exLife;
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionRadius;
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecrackerB.Ex.damageRate;
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionDamage;
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecrackerB.Ex.explosionTime;
		}
		else
		{
			weaponFirecrackerEXProjectile.Speed = WeaponProperties.LevelWeaponFirecracker.Ex.exSpeed;
			weaponFirecrackerEXProjectile.bulletLife = WeaponProperties.LevelWeaponFirecracker.Ex.exLife;
			weaponFirecrackerEXProjectile.explosionSize = WeaponProperties.LevelWeaponFirecracker.Ex.explosionRadius;
			weaponFirecrackerEXProjectile.DamageRate = WeaponProperties.LevelWeaponFirecracker.Ex.damageRate;
			weaponFirecrackerEXProjectile.Damage = WeaponProperties.LevelWeaponFirecracker.Ex.explosionDamage;
			weaponFirecrackerEXProjectile.explosionDuration = WeaponProperties.LevelWeaponFirecracker.Ex.explosionTime;
		}
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponFirecrackerEXProjectile);
		return weaponFirecrackerEXProjectile;
	}

	// Token: 0x06004016 RID: 16406 RVA: 0x0022F324 File Offset: 0x0022D724
	private void Update()
	{
		this.dummyObject.transform.position = this.player.transform.position;
	}

	// Token: 0x06004017 RID: 16407 RVA: 0x0022F346 File Offset: 0x0022D746
	private void CreateDummyObject()
	{
		this.dummyObject = new GameObject();
		this.dummyObject.name = "FirecrackerDummyObj";
	}

	// Token: 0x040046D9 RID: 18137
	public bool isTypeB;

	// Token: 0x040046DA RID: 18138
	private GameObject dummyObject;

	// Token: 0x040046DB RID: 18139
	private string[] explosionAngles;

	// Token: 0x040046DC RID: 18140
	private int explosionAngleIndex;
}
