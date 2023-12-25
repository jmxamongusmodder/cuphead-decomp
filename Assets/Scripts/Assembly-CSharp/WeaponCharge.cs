using System;
using UnityEngine;

// Token: 0x02000A6F RID: 2671
public class WeaponCharge : AbstractLevelWeapon
{
	// Token: 0x17000581 RID: 1409
	// (get) Token: 0x06003FC8 RID: 16328 RVA: 0x0022D059 File Offset: 0x0022B459
	protected override bool rapidFire
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000582 RID: 1410
	// (get) Token: 0x06003FC9 RID: 16329 RVA: 0x0022D05C File Offset: 0x0022B45C
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponCharge.Basic.fireRate;
		}
	}

	// Token: 0x17000583 RID: 1411
	// (get) Token: 0x06003FCA RID: 16330 RVA: 0x0022D063 File Offset: 0x0022B463
	protected override bool isChargeWeapon
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06003FCB RID: 16331 RVA: 0x0022D068 File Offset: 0x0022B468
	protected override void StartCharging()
	{
		base.StartCharging();
		this.BasicSoundOneShot("player_weapon_charge_start", "player_weapon_charge_start_p2");
		if (this.chargeEffect != null)
		{
			UnityEngine.Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
		}
		this.chargeEffect = this.chargeEffectPrefab.Create(base.transform.position);
		this.chargeEffect.transform.parent = this.player.transform;
		this.timeCharged = 0f;
	}

	// Token: 0x06003FCC RID: 16332 RVA: 0x0022D0FA File Offset: 0x0022B4FA
	protected override void StopCharging()
	{
		base.StopCharging();
		if (this.chargeEffect != null)
		{
			UnityEngine.Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
			this.timeCharged = 0f;
		}
	}

	// Token: 0x06003FCD RID: 16333 RVA: 0x0022D138 File Offset: 0x0022B538
	private void FixedUpdate()
	{
		if (this.chargeEffect == null)
		{
			this.fullyCharged = false;
			this.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
		}
		else
		{
			this.chargeEffect.transform.position = this.player.weaponManager.GetBulletPosition();
			this.timeCharged += CupheadTime.FixedDelta;
			if (this.timeCharged > WeaponProperties.LevelWeaponCharge.Basic.timeStateThree)
			{
				this.fullyCharged = true;
				if (this.AllowChargeSound)
				{
					AudioManager.Play("player_weapon_charge_ready");
					this.AllowChargeSound = false;
				}
				this.chargeEffect.animator.SetTrigger("IsFull");
				this.damage = WeaponProperties.LevelWeaponCharge.Basic.damageStateThree;
			}
			else
			{
				this.fullyCharged = false;
				this.damage = WeaponProperties.LevelWeaponCharge.Basic.baseDamage;
			}
		}
	}

	// Token: 0x06003FCE RID: 16334 RVA: 0x0022D210 File Offset: 0x0022B610
	protected override AbstractProjectile fireBasic()
	{
		WeaponChargeProjectile weaponChargeProjectile;
		if (this.fullyCharged)
		{
			Effect basicEffectPrefab = this.basicEffectPrefab;
			this.basicEffectPrefab = null;
			weaponChargeProjectile = (base.fireBasic() as WeaponChargeProjectile);
			this.basicEffectPrefab = basicEffectPrefab;
		}
		else
		{
			weaponChargeProjectile = (base.fireBasic() as WeaponChargeProjectile);
		}
		weaponChargeProjectile.Speed = ((!this.fullyCharged) ? WeaponProperties.LevelWeaponCharge.Basic.speed : WeaponProperties.LevelWeaponCharge.Basic.speedStateTwo);
		weaponChargeProjectile.Damage = this.damage;
		weaponChargeProjectile.PlayerId = this.player.id;
		weaponChargeProjectile.DamagesType.PlayerProjectileDefault();
		weaponChargeProjectile.CollisionDeath.PlayerProjectileDefault();
		if (this.fullyCharged && this.player.motor.Ducking)
		{
			weaponChargeProjectile.CollisionDeath.Ground = false;
			weaponChargeProjectile.CollisionDeath.Walls = false;
			weaponChargeProjectile.CollisionDeath.Other = false;
		}
		weaponChargeProjectile.fullyCharged = this.fullyCharged;
		weaponChargeProjectile.animator.SetBool("FullCharge", this.fullyCharged);
		if (this.chargeEffect != null)
		{
			UnityEngine.Object.Destroy(this.chargeEffect.gameObject);
			this.chargeEffect = null;
			this.timeCharged = 0f;
		}
		if (this.fullyCharged)
		{
			Effect effect = this.fullChargeFx.Create(weaponChargeProjectile.transform.position);
			effect.transform.eulerAngles = new Vector3(0f, 0f, this.weaponManager.GetBulletRotation());
			this.BasicSoundOneShot("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
			this.AllowChargeSound = true;
		}
		else
		{
			this.BasicSoundOneShot("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		return weaponChargeProjectile;
	}

	// Token: 0x06003FCF RID: 16335 RVA: 0x0022D3BC File Offset: 0x0022B7BC
	protected override AbstractProjectile fireEx()
	{
		WeaponChargeExBurst weaponChargeExBurst = base.fireEx() as WeaponChargeExBurst;
		Vector2 vector = 125f * MathUtils.AngleToDirection(weaponChargeExBurst.transform.eulerAngles.z);
		weaponChargeExBurst.transform.AddPosition(vector.x, vector.y, 0f);
		weaponChargeExBurst.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		weaponChargeExBurst.transform.SetScale(new float?((float)((!Rand.Bool()) ? 1 : -1)), null, null);
		weaponChargeExBurst.PlayerId = this.player.id;
		weaponChargeExBurst.Damage = WeaponProperties.LevelWeaponCharge.Ex.damage;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponChargeExBurst);
		return weaponChargeExBurst;
	}

	// Token: 0x06003FD0 RID: 16336 RVA: 0x0022D4A0 File Offset: 0x0022B8A0
	public override void BeginBasic()
	{
		if (this.fullyCharged)
		{
			this.BeginBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
		}
		else
		{
			this.BeginBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		base.BeginBasic();
	}

	// Token: 0x06003FD1 RID: 16337 RVA: 0x0022D4D8 File Offset: 0x0022B8D8
	public override void EndBasic()
	{
		if (this.fullyCharged)
		{
			this.EndBasicCheckAttenuation("player_weapon_charge_full_fireball", "player_weapon_charge_full_fireball_p2");
		}
		else
		{
			this.EndBasicCheckAttenuation("player_weapon_charge_fire_small", "player_weapon_charge_fire_small_p2");
		}
		base.EndBasic();
	}

	// Token: 0x06003FD2 RID: 16338 RVA: 0x0022D510 File Offset: 0x0022B910
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.chargeEffectPrefab = null;
		this.fullChargeFx = null;
	}

	// Token: 0x040046A9 RID: 18089
	[SerializeField]
	private WeaponChargeChargingEffect chargeEffectPrefab;

	// Token: 0x040046AA RID: 18090
	[SerializeField]
	private Effect fullChargeFx;

	// Token: 0x040046AB RID: 18091
	private WeaponChargeChargingEffect chargeEffect;

	// Token: 0x040046AC RID: 18092
	private bool fullyCharged;

	// Token: 0x040046AD RID: 18093
	private float timeCharged;

	// Token: 0x040046AE RID: 18094
	private int damageState;

	// Token: 0x040046AF RID: 18095
	private float damage;

	// Token: 0x040046B0 RID: 18096
	private bool AllowChargeSound = true;
}
