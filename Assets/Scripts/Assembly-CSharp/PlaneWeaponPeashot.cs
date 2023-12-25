using System;
using UnityEngine;

// Token: 0x02000AC0 RID: 2752
public class PlaneWeaponPeashot : AbstractPlaneWeapon
{
	// Token: 0x170005CA RID: 1482
	// (get) Token: 0x0600421B RID: 16923 RVA: 0x0023B613 File Offset: 0x00239A13
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x170005CB RID: 1483
	// (get) Token: 0x0600421C RID: 16924 RVA: 0x0023B61A File Offset: 0x00239A1A
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x0600421D RID: 16925 RVA: 0x0023B624 File Offset: 0x00239A24
	protected override AbstractProjectile fireBasic()
	{
		if ((this.player.id == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (this.player.id == PlayerId.PlayerTwo && PlayerManager.player1IsMugman))
		{
			if (!AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_cuphead"))
			{
				AudioManager.PlayLoop("player_plane_weapon_fire_loop_cuphead");
			}
		}
		else if (!AudioManager.CheckIfPlaying("player_plane_weapon_fire_loop_mugman"))
		{
			AudioManager.PlayLoop("player_plane_weapon_fire_loop_mugman");
		}
		this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_cuphead");
		this.emitAudioFromObject.Add("player_plane_weapon_fire_loop_mugman");
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.PlaneWeaponPeashot.Basic.speed;
		basicProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float num = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, num, 0f);
		Animator component = basicProjectile.GetComponent<Animator>();
		component.SetInteger("Variant", UnityEngine.Random.Range(0, component.GetInteger("MaxVariants")));
		component.SetBool("isCH", (basicProjectile.PlayerId == PlayerId.PlayerOne && !PlayerManager.player1IsMugman) || (basicProjectile.PlayerId == PlayerId.PlayerTwo && PlayerManager.player1IsMugman));
		if (this.player.Shrunk)
		{
			basicProjectile.Damage *= this.shrunkDamageMultiplier;
			basicProjectile.transform.AddPosition(0f, num * -0.5f, 0f);
			basicProjectile.DestroyDistance = (float)UnityEngine.Random.Range(200, 350);
			basicProjectile.DestroyDistanceAnimated = true;
			basicProjectile.DamageSource = DamageDealer.DamageSource.SmallPlane;
		}
		return basicProjectile;
	}

	// Token: 0x0600421E RID: 16926 RVA: 0x0023B800 File Offset: 0x00239C00
	protected override AbstractProjectile fireEx()
	{
		PlaneWeaponPeashotExProjectile planeWeaponPeashotExProjectile = base.fireEx() as PlaneWeaponPeashotExProjectile;
		planeWeaponPeashotExProjectile.MaxSpeed = WeaponProperties.PlaneWeaponPeashot.Ex.maxSpeed;
		planeWeaponPeashotExProjectile.Acceleration = WeaponProperties.PlaneWeaponPeashot.Ex.acceleration;
		planeWeaponPeashotExProjectile.FreezeTime = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime;
		planeWeaponPeashotExProjectile.Damage = WeaponProperties.PlaneWeaponPeashot.Ex.damage;
		planeWeaponPeashotExProjectile.DamageRate = WeaponProperties.PlaneWeaponPeashot.Ex.freezeTime + WeaponProperties.PlaneWeaponPeashot.Ex.damageDistance / planeWeaponPeashotExProjectile.MaxSpeed;
		planeWeaponPeashotExProjectile.PlayerId = this.player.id;
		planeWeaponPeashotExProjectile.speed = Mathf.Clamp(this.player.motor.Velocity.x, 0f, planeWeaponPeashotExProjectile.MaxSpeed);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(planeWeaponPeashotExProjectile);
		planeWeaponPeashotExProjectile.Init();
		return planeWeaponPeashotExProjectile;
	}

	// Token: 0x04004893 RID: 18579
	private const float Y_POS = 20f;

	// Token: 0x04004894 RID: 18580
	private float[] yPositions = new float[]
	{
		20f,
		-20f
	};

	// Token: 0x04004895 RID: 18581
	private int currentY;
}
