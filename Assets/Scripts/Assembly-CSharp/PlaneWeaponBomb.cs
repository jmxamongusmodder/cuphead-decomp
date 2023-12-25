using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AB4 RID: 2740
public class PlaneWeaponBomb : AbstractPlaneWeapon
{
	// Token: 0x170005C1 RID: 1473
	// (get) Token: 0x060041D0 RID: 16848 RVA: 0x002394CA File Offset: 0x002378CA
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponBomb.Basic.rapidFire;
		}
	}

	// Token: 0x170005C2 RID: 1474
	// (get) Token: 0x060041D1 RID: 16849 RVA: 0x002394D1 File Offset: 0x002378D1
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponBomb.Basic.rapidFireRate;
		}
	}

	// Token: 0x060041D2 RID: 16850 RVA: 0x002394D8 File Offset: 0x002378D8
	protected override AbstractProjectile fireBasic()
	{
		PlaneWeaponBombProjectile planeWeaponBombProjectile = base.fireBasic() as PlaneWeaponBombProjectile;
		planeWeaponBombProjectile.shootsUp = false;
		planeWeaponBombProjectile.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z);
		planeWeaponBombProjectile.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity;
		planeWeaponBombProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage;
		planeWeaponBombProjectile.PlayerId = this.player.id;
		planeWeaponBombProjectile.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size;
		planeWeaponBombProjectile.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion;
		planeWeaponBombProjectile.SetAnimation(this.player.id);
		if (WeaponProperties.PlaneWeaponBomb.Basic.Up)
		{
			PlaneWeaponBombProjectile planeWeaponBombProjectile2 = base.fireBasic() as PlaneWeaponBombProjectile;
			planeWeaponBombProjectile2.shootsUp = true;
			planeWeaponBombProjectile2.velocity = WeaponProperties.PlaneWeaponBomb.Basic.speed * MathUtils.AngleToDirection(planeWeaponBombProjectile.transform.rotation.eulerAngles.z);
			planeWeaponBombProjectile2.gravity = WeaponProperties.PlaneWeaponBomb.Basic.gravity;
			planeWeaponBombProjectile2.Damage = WeaponProperties.PlaneWeaponBomb.Basic.damage;
			planeWeaponBombProjectile2.PlayerId = this.player.id;
			planeWeaponBombProjectile2.bulletSize = WeaponProperties.PlaneWeaponBomb.Basic.size;
			planeWeaponBombProjectile2.explosionSize = WeaponProperties.PlaneWeaponBomb.Basic.sizeExplosion;
			planeWeaponBombProjectile2.SetAnimation(this.player.id);
		}
		return planeWeaponBombProjectile;
	}

	// Token: 0x060041D3 RID: 16851 RVA: 0x00239614 File Offset: 0x00237A14
	protected override AbstractProjectile fireEx()
	{
		base.StartCoroutine(this.ex_cr());
		return null;
	}

	// Token: 0x060041D4 RID: 16852 RVA: 0x00239624 File Offset: 0x00237A24
	private IEnumerator ex_cr()
	{
		for (int wave = 0; wave < WeaponProperties.PlaneWeaponBomb.Ex.counts.Length; wave++)
		{
			int count = WeaponProperties.PlaneWeaponBomb.Ex.counts[wave];
			float angle = WeaponProperties.PlaneWeaponBomb.Ex.angles[wave];
			MeterScoreTracker tracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
			for (int i = 0; i < count; i++)
			{
				float num = Mathf.Lerp(0f, angle, (float)i / (float)count) - 90f;
				PlaneWeaponBombExProjectile planeWeaponBombExProjectile = base.fireEx() as PlaneWeaponBombExProjectile;
				planeWeaponBombExProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(num));
				planeWeaponBombExProjectile.rotation = num;
				planeWeaponBombExProjectile.speed = WeaponProperties.PlaneWeaponBomb.Ex.speed;
				planeWeaponBombExProjectile.Damage = WeaponProperties.PlaneWeaponBomb.Ex.damage;
				planeWeaponBombExProjectile.PlayerId = this.player.id;
				planeWeaponBombExProjectile.rotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeed;
				planeWeaponBombExProjectile.rotationSpeedEaseTime = WeaponProperties.PlaneWeaponBomb.Ex.rotationSpeedEaseTime;
				planeWeaponBombExProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.PlaneWeaponBomb.Ex.timeBeforeEaseRotationSpeed;
				tracker.Add(planeWeaponBombExProjectile);
				planeWeaponBombExProjectile.Init();
				planeWeaponBombExProjectile.FindTarget();
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}
}
