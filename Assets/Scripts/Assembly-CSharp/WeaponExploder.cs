using System;
using System.Collections.Generic;

// Token: 0x02000A76 RID: 2678
public class WeaponExploder : AbstractLevelWeapon
{
	// Token: 0x17000587 RID: 1415
	// (get) Token: 0x06003FFE RID: 16382 RVA: 0x0022EB93 File Offset: 0x0022CF93
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000588 RID: 1416
	// (get) Token: 0x06003FFF RID: 16383 RVA: 0x0022EB96 File Offset: 0x0022CF96
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponExploder.Basic.fireRate;
		}
	}

	// Token: 0x06004000 RID: 16384 RVA: 0x0022EBA0 File Offset: 0x0022CFA0
	protected override AbstractProjectile fireBasic()
	{
		WeaponExploderProjectile weaponExploderProjectile = base.fireBasic() as WeaponExploderProjectile;
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Basic.speed;
		weaponExploderProjectile.PlayerId = this.player.id;
		weaponExploderProjectile.DamagesType.SetAll(false);
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponExploderProjectile.weapon = this;
		weaponExploderProjectile.minMaxSpeed = WeaponProperties.LevelWeaponExploder.Basic.easeSpeed;
		weaponExploderProjectile.easeTime = WeaponProperties.LevelWeaponExploder.Basic.easeTime;
		if (WeaponProperties.LevelWeaponExploder.Basic.easing)
		{
			weaponExploderProjectile.EaseSpeed();
		}
		return weaponExploderProjectile;
	}

	// Token: 0x06004001 RID: 16385 RVA: 0x0022EC1C File Offset: 0x0022D01C
	protected override AbstractProjectile fireEx()
	{
		WeaponExploderProjectile weaponExploderProjectile = base.fireEx() as WeaponExploderProjectile;
		weaponExploderProjectile.Speed = WeaponProperties.LevelWeaponExploder.Ex.speed;
		weaponExploderProjectile.Damage = WeaponProperties.LevelWeaponExploder.Ex.damage;
		weaponExploderProjectile.explodeRadius = WeaponProperties.LevelWeaponExploder.Ex.explodeRadius;
		weaponExploderProjectile.PlayerId = this.player.id;
		weaponExploderProjectile.DamagesType.SetAll(false);
		weaponExploderProjectile.CollisionDeath.PlayerProjectileDefault();
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponExploderProjectile);
		return weaponExploderProjectile;
	}

	// Token: 0x040046CD RID: 18125
	public List<WeaponArcProjectile> projectilesOnGround = new List<WeaponArcProjectile>();
}
