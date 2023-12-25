using System;

// Token: 0x02000A08 RID: 2568
public class ArcadeWeaponRocketPeashot : AbstractArcadeWeapon
{
	// Token: 0x17000522 RID: 1314
	// (get) Token: 0x06003CA7 RID: 15527 RVA: 0x0021A031 File Offset: 0x00218431
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x17000523 RID: 1315
	// (get) Token: 0x06003CA8 RID: 15528 RVA: 0x0021A038 File Offset: 0x00218438
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.ArcadeWeaponRocketPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003CA9 RID: 15529 RVA: 0x0021A040 File Offset: 0x00218440
	protected override AbstractProjectile fireBasic()
	{
		if (this.p != null && !this.p.dead)
		{
			return null;
		}
		this.p = (base.fireBasic() as ArcadeWeaponBullet);
		this.p.Speed = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.speed;
		this.p.Damage = WeaponProperties.ArcadeWeaponRocketPeashot.Basic.damage;
		this.p.PlayerId = this.player.id;
		this.p.DamagesType.PlayerProjectileDefault();
		this.p.CollisionDeath.PlayerProjectileDefault();
		return this.p;
	}

	// Token: 0x040043F7 RID: 17399
	private ArcadeWeaponBullet p;
}
