using System;
using UnityEngine;

// Token: 0x02000AB8 RID: 2744
public class PlaneWeaponChaliceBomb : AbstractPlaneWeapon
{
	// Token: 0x170005C4 RID: 1476
	// (get) Token: 0x060041EF RID: 16879 RVA: 0x0023A19B File Offset: 0x0023859B
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFire;
		}
	}

	// Token: 0x170005C5 RID: 1477
	// (get) Token: 0x060041F0 RID: 16880 RVA: 0x0023A1A2 File Offset: 0x002385A2
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponChaliceBomb.Basic.rapidFireRate;
		}
	}

	// Token: 0x060041F1 RID: 16881 RVA: 0x0023A1AC File Offset: 0x002385AC
	protected override AbstractProjectile fireBasic()
	{
		PlaneWeaponChaliceBombProjectile planeWeaponChaliceBombProjectile = base.fireBasic() as PlaneWeaponChaliceBombProjectile;
		planeWeaponChaliceBombProjectile.transform.Rotate(new Vector3(0f, 0f, UnityEngine.Random.Range(-WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange, WeaponProperties.PlaneWeaponChaliceBomb.Basic.angleRange)));
		planeWeaponChaliceBombProjectile.velocity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponChaliceBombProjectile.transform.rotation.eulerAngles.z);
		planeWeaponChaliceBombProjectile.gravity = WeaponProperties.PlaneWeaponChaliceBomb.Basic.gravity;
		planeWeaponChaliceBombProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damage;
		planeWeaponChaliceBombProjectile.size = WeaponProperties.PlaneWeaponChaliceBomb.Basic.size;
		planeWeaponChaliceBombProjectile.damageExplosion = WeaponProperties.PlaneWeaponChaliceBomb.Basic.damageExplosion;
		planeWeaponChaliceBombProjectile.explosionSize = WeaponProperties.PlaneWeaponChaliceBomb.Basic.sizeExplosion;
		planeWeaponChaliceBombProjectile.PlayerId = this.player.id;
		planeWeaponChaliceBombProjectile.SetAnimation(this.isA);
		this.isA = !this.isA;
		return planeWeaponChaliceBombProjectile;
	}

	// Token: 0x060041F2 RID: 16882 RVA: 0x0023A284 File Offset: 0x00238684
	protected override AbstractProjectile fireEx()
	{
		PlaneWeaponChaliceBombExProjectile planeWeaponChaliceBombExProjectile = base.fireEx() as PlaneWeaponChaliceBombExProjectile;
		planeWeaponChaliceBombExProjectile.FreezeTime = WeaponProperties.PlaneWeaponChaliceBomb.Ex.freezeTime;
		planeWeaponChaliceBombExProjectile.Damage = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damage;
		planeWeaponChaliceBombExProjectile.DamageRate = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRate;
		planeWeaponChaliceBombExProjectile.DamageRateIncrease = WeaponProperties.PlaneWeaponChaliceBomb.Ex.damageRateIncrease;
		planeWeaponChaliceBombExProjectile.Gravity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.gravity;
		planeWeaponChaliceBombExProjectile.Velocity = WeaponProperties.PlaneWeaponChaliceBomb.Ex.startSpeed * Vector3.right;
		planeWeaponChaliceBombExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(planeWeaponChaliceBombExProjectile);
		return planeWeaponChaliceBombExProjectile;
	}

	// Token: 0x04004854 RID: 18516
	private bool isA;
}
