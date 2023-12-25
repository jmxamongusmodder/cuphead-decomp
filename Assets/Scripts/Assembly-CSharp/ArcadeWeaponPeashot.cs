using System;

// Token: 0x02000A07 RID: 2567
public class ArcadeWeaponPeashot : AbstractArcadeWeapon
{
	// Token: 0x17000520 RID: 1312
	// (get) Token: 0x06003CA2 RID: 15522 RVA: 0x00219F7A File Offset: 0x0021837A
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x17000521 RID: 1313
	// (get) Token: 0x06003CA3 RID: 15523 RVA: 0x00219F81 File Offset: 0x00218381
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.ArcadeWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06003CA4 RID: 15524 RVA: 0x00219F88 File Offset: 0x00218388
	protected override AbstractProjectile fireBasic()
	{
		if (this.p != null && !this.p.dead)
		{
			return null;
		}
		this.p = (base.fireBasic() as ArcadeWeaponBullet);
		this.p.Speed = WeaponProperties.ArcadeWeaponPeashot.Basic.speed;
		this.p.Damage = WeaponProperties.ArcadeWeaponPeashot.Basic.damage;
		this.p.PlayerId = this.player.id;
		this.p.DamagesType.PlayerProjectileDefault();
		this.p.CollisionDeath.PlayerProjectileDefault();
		return this.p;
	}

	// Token: 0x06003CA5 RID: 15525 RVA: 0x0021A026 File Offset: 0x00218426
	protected override AbstractProjectile fireEx()
	{
		return null;
	}

	// Token: 0x040043F4 RID: 17396
	private const float Y_POS = 20f;

	// Token: 0x040043F5 RID: 17397
	private const float ROTATION_OFFSET = 3f;

	// Token: 0x040043F6 RID: 17398
	private ArcadeWeaponBullet p;
}
