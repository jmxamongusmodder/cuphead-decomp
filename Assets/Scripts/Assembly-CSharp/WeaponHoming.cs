using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7C RID: 2684
public class WeaponHoming : AbstractLevelWeapon
{
	// Token: 0x1700058B RID: 1419
	// (get) Token: 0x06004026 RID: 16422 RVA: 0x0022FB41 File Offset: 0x0022DF41
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700058C RID: 1420
	// (get) Token: 0x06004027 RID: 16423 RVA: 0x0022FB44 File Offset: 0x0022DF44
	protected override float rapidFireRate
	{
		get
		{
			return this.fireRate;
		}
	}

	// Token: 0x06004028 RID: 16424 RVA: 0x0022FB4C File Offset: 0x0022DF4C
	private void FixedUpdate()
	{
		if (this.player.motor.Locked)
		{
			this.fireRateLockRatio = Mathf.Clamp01(this.fireRateLockRatio + CupheadTime.FixedDelta / WeaponProperties.LevelWeaponHoming.Basic.lockedShotAccelerationTime);
		}
		else
		{
			this.fireRateLockRatio = 0f;
		}
		this.fireRate = WeaponProperties.LevelWeaponHoming.Basic.fireRate.GetFloatAt(1f - this.fireRateLockRatio);
	}

	// Token: 0x06004029 RID: 16425 RVA: 0x0022FBB8 File Offset: 0x0022DFB8
	protected override AbstractProjectile fireBasic()
	{
		WeaponHomingProjectile weaponHomingProjectile = base.fireBasic() as WeaponHomingProjectile;
		weaponHomingProjectile.rotation = weaponHomingProjectile.transform.rotation.eulerAngles.z + UnityEngine.Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.angleVariation, WeaponProperties.LevelWeaponHoming.Basic.angleVariation);
		weaponHomingProjectile.speed = WeaponProperties.LevelWeaponHoming.Basic.speed + UnityEngine.Random.Range(-WeaponProperties.LevelWeaponHoming.Basic.speedVariation, WeaponProperties.LevelWeaponHoming.Basic.speedVariation);
		weaponHomingProjectile.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed;
		weaponHomingProjectile.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime;
		weaponHomingProjectile.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed;
		weaponHomingProjectile.Damage = WeaponProperties.LevelWeaponHoming.Basic.damage;
		weaponHomingProjectile.PlayerId = this.player.id;
		weaponHomingProjectile.DamagesType.PlayerProjectileDefault();
		weaponHomingProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponHomingProjectile.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Basic.trailFrameDelay, 1, 10);
		if (UnityEngine.Random.Range(0, 4) == 0)
		{
			weaponHomingProjectile.transform.SetScale(new float?(0.8f), new float?(0.8f), null);
		}
		if (MathUtils.RandomBool())
		{
			weaponHomingProjectile.transform.SetScale(new float?(-weaponHomingProjectile.transform.localScale.x), null, null);
		}
		weaponHomingProjectile.FindTarget();
		return weaponHomingProjectile;
	}

	// Token: 0x0600402A RID: 16426 RVA: 0x0022FD04 File Offset: 0x0022E104
	protected override AbstractProjectile fireEx()
	{
		foreach (WeaponHomingProjectile weaponHomingProjectile in this.swirlingProjectiles)
		{
			if (weaponHomingProjectile != null)
			{
				weaponHomingProjectile.StopSwirling();
			}
		}
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		this.swirlingProjectiles.Clear();
		for (int i = 0; i < WeaponProperties.LevelWeaponHoming.Ex.bulletCount; i++)
		{
			WeaponHomingProjectile weaponHomingProjectile2 = base.fireEx() as WeaponHomingProjectile;
			weaponHomingProjectile2.speed = WeaponProperties.LevelWeaponHoming.Ex.speed;
			weaponHomingProjectile2.rotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeed;
			weaponHomingProjectile2.rotationSpeedEaseTime = WeaponProperties.LevelWeaponHoming.Basic.rotationSpeedEaseTime;
			weaponHomingProjectile2.timeBeforeEaseRotationSpeed = WeaponProperties.LevelWeaponHoming.Basic.timeBeforeEaseRotationSpeed;
			weaponHomingProjectile2.Damage = WeaponProperties.LevelWeaponHoming.Ex.damage;
			weaponHomingProjectile2.PlayerId = this.player.id;
			weaponHomingProjectile2.DamagesType.PlayerProjectileDefault();
			weaponHomingProjectile2.CollisionDeath.PlayerProjectileDefault();
			weaponHomingProjectile2.CollisionDeath.SetBounds(false);
			weaponHomingProjectile2.swirlDistance = WeaponProperties.LevelWeaponHoming.Ex.swirlDistance;
			weaponHomingProjectile2.swirlEaseTime = WeaponProperties.LevelWeaponHoming.Ex.swirlEaseTime;
			weaponHomingProjectile2.trailFollowFrames = Mathf.Clamp(WeaponProperties.LevelWeaponHoming.Ex.trailFrameDelay, 1, 10);
			weaponHomingProjectile2.StartSwirling(i, WeaponProperties.LevelWeaponHoming.Ex.bulletCount, WeaponProperties.LevelWeaponHoming.Ex.spread, this.player);
			weaponHomingProjectile2.isEx = true;
			this.swirlingProjectiles.Add(weaponHomingProjectile2);
			meterScoreTracker.Add(weaponHomingProjectile2);
		}
		return this.swirlingProjectiles[0];
	}

	// Token: 0x0600402B RID: 16427 RVA: 0x0022FE84 File Offset: 0x0022E284
	public override void BeginBasic()
	{
		base.BeginBasic();
		AudioManager.Play("player_weapon_homing_fire_start");
		this.emitAudioFromObject.Add("player_weapon_homing_fire_start");
		this.BasicSoundLoop("player_weapon_homing_loop", "player_weapon_homing_loop_p2");
	}

	// Token: 0x0600402C RID: 16428 RVA: 0x0022FEB6 File Offset: 0x0022E2B6
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_homing_loop", "player_weapon_homing_loop_p2");
	}

	// Token: 0x040046EF RID: 18159
	private float fireRate = 1f;

	// Token: 0x040046F0 RID: 18160
	private float fireRateLockRatio;

	// Token: 0x040046F1 RID: 18161
	public static Transform target;

	// Token: 0x040046F2 RID: 18162
	private List<WeaponHomingProjectile> swirlingProjectiles = new List<WeaponHomingProjectile>();
}
