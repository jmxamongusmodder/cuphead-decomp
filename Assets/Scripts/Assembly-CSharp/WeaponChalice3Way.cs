using System;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public class WeaponChalice3Way : AbstractPlaneWeapon
{
	// Token: 0x170005C6 RID: 1478
	// (get) Token: 0x0600420E RID: 16910 RVA: 0x0023B01F File Offset: 0x0023941F
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170005C7 RID: 1479
	// (get) Token: 0x0600420F RID: 16911 RVA: 0x0023B022 File Offset: 0x00239422
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponChaliceWay.Basic.rapidFireRate;
		}
	}

	// Token: 0x06004210 RID: 16912 RVA: 0x0023B029 File Offset: 0x00239429
	protected override Effect GetEffect(AbstractPlaneWeapon.Mode mode)
	{
		if (mode == AbstractPlaneWeapon.Mode.Basic || mode != AbstractPlaneWeapon.Mode.Ex)
		{
			return (this.bulletNumber != 0) ? null : this.basicEffectPrefab;
		}
		return this.exEffectPrefab;
	}

	// Token: 0x06004211 RID: 16913 RVA: 0x0023B05C File Offset: 0x0023945C
	protected override AbstractProjectile fireBasic()
	{
		float damage = WeaponProperties.PlaneWeaponChaliceWay.Basic.damage;
		BasicProjectile basicProjectile = null;
		float z = 0f;
		float angle = WeaponProperties.PlaneWeaponChaliceWay.Basic.angle;
		this.bulletNumber = 0;
		while ((float)this.bulletNumber < 3f)
		{
			if (this.bulletNumber > 0)
			{
				z = (((float)this.bulletNumber < 2f) ? (-angle) : angle);
			}
			basicProjectile = (base.fireBasic() as BasicProjectile);
			basicProjectile.Speed = WeaponProperties.PlaneWeaponChaliceWay.Basic.speed;
			basicProjectile.DestroyDistance = WeaponProperties.PlaneWeaponChaliceWay.Basic.distance - 20f * (float)(this.bulletNumber + 1);
			basicProjectile.Damage = damage;
			basicProjectile.PlayerId = this.player.id;
			basicProjectile.transform.AddEulerAngles(0f, 0f, z);
			basicProjectile.transform.position += Vector3.right * ((this.bulletNumber != 0) ? -40f : 40f);
			Animator component = basicProjectile.GetComponent<Animator>();
			component.Play(((this.bulletNumber + this.animatorOffset) % 3).ToString(), 0, UnityEngine.Random.Range(0f, 1f));
			AudioManager.Play("player_plane_weapon_chalice");
			this.emitAudioFromObject.Add("player_plane_weapon_chalice");
			this.bulletNumber++;
		}
		this.animatorOffset++;
		return basicProjectile;
	}

	// Token: 0x06004212 RID: 16914 RVA: 0x0023B1D0 File Offset: 0x002395D0
	protected override AbstractProjectile fireEx()
	{
		PlaneWeaponChalice3WayExProjectile[] array = new PlaneWeaponChalice3WayExProjectile[2];
		for (int i = 0; i < 2; i++)
		{
			array[i] = (base.fireEx() as PlaneWeaponChalice3WayExProjectile);
			array[i].Damage = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageBeforeLaunch;
			array[i].PlayerId = this.player.id;
			array[i].arcSpeed = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcSpeed;
			array[i].arcX = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcX;
			array[i].arcY = WeaponProperties.PlaneWeaponChaliceWay.Ex.arcY;
			array[i].damageAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageAfterLaunch;
			array[i].pauseTime = WeaponProperties.PlaneWeaponChaliceWay.Ex.pauseTime;
			array[i].FreezeTime = WeaponProperties.PlaneWeaponChaliceWay.Ex.freezeTime;
			array[i].speedAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.speedAfterLaunch;
			array[i].accelAfterLaunch = WeaponProperties.PlaneWeaponChaliceWay.Ex.accelAfterLaunch;
			array[i].minXDistance = WeaponProperties.PlaneWeaponChaliceWay.Ex.minXDistance;
			array[i].xDistanceNoTarget = (float)WeaponProperties.PlaneWeaponChaliceWay.Ex.xDistanceNoTarget;
			array[i].transform.parent = base.transform;
			array[i].SetArcPosition();
			array[i].vDirection = (float)((i != 0) ? 1 : -1);
			array[i].DamageRate = WeaponProperties.PlaneWeaponChaliceWay.Ex.damageRateBeforeLaunch;
			array[i].CollisionDeath.OnlyBounds();
			array[i].ID = i;
			MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
			meterScoreTracker.Add(array[i]);
		}
		array[0].partner = array[1];
		array[1].partner = array[0];
		return null;
	}

	// Token: 0x0400488E RID: 18574
	private int animatorOffset;

	// Token: 0x0400488F RID: 18575
	private int bulletNumber;
}
