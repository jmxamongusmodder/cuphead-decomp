using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A67 RID: 2663
public class WeaponArc : AbstractLevelWeapon
{
	// Token: 0x17000577 RID: 1399
	// (get) Token: 0x06003F88 RID: 16264 RVA: 0x0022B204 File Offset: 0x00229604
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.LevelWeaponArc.Basic.rapidFire;
		}
	}

	// Token: 0x17000578 RID: 1400
	// (get) Token: 0x06003F89 RID: 16265 RVA: 0x0022B20B File Offset: 0x0022960B
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponArc.Basic.fireRate;
		}
	}

	// Token: 0x06003F8A RID: 16266 RVA: 0x0022B214 File Offset: 0x00229614
	protected override AbstractProjectile fireBasic()
	{
		AudioManager.Play("player_weapon_arc");
		this.emitAudioFromObject.Add("player_weapon_arc");
		WeaponArcProjectile weaponArcProjectile = base.fireBasic() as WeaponArcProjectile;
		weaponArcProjectile.PlayerId = this.player.id;
		float num = weaponArcProjectile.transform.rotation.eulerAngles.z;
		if (num == 0f)
		{
			num += WeaponProperties.LevelWeaponArc.Basic.straightShotAngle;
			this.isDiagonal = false;
		}
		else if (num == 180f)
		{
			num -= WeaponProperties.LevelWeaponArc.Basic.straightShotAngle;
			this.isDiagonal = false;
		}
		else if (Mathf.Approximately(num, 45f) || Mathf.Approximately(num, 135f))
		{
			num += WeaponProperties.LevelWeaponArc.Basic.diagShotAngle;
			this.isDiagonal = true;
		}
		else
		{
			this.isDiagonal = false;
		}
		weaponArcProjectile.transform.SetEulerAngles(null, null, new float?(num));
		if (this.isDiagonal)
		{
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.diagLaunchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.diagGravity;
		}
		else
		{
			weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
			weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity;
		}
		weaponArcProjectile.weapon = this;
		return weaponArcProjectile;
	}

	// Token: 0x06003F8B RID: 16267 RVA: 0x0022B3A4 File Offset: 0x002297A4
	protected override AbstractProjectile fireEx()
	{
		AudioManager.Play("player_weapon_peashot_ex");
		WeaponArcProjectile weaponArcProjectile = base.fireEx() as WeaponArcProjectile;
		weaponArcProjectile.velocity = WeaponProperties.LevelWeaponArc.Basic.launchSpeed * MathUtils.AngleToDirection(weaponArcProjectile.transform.rotation.eulerAngles.z);
		weaponArcProjectile.gravity = WeaponProperties.LevelWeaponArc.Basic.gravity;
		weaponArcProjectile.weapon = this;
		weaponArcProjectile.Damage = WeaponProperties.LevelWeaponArc.Ex.damage;
		weaponArcProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponArcProjectile);
		return weaponArcProjectile;
	}

	// Token: 0x04004676 RID: 18038
	public List<WeaponArcProjectile> projectilesOnGround = new List<WeaponArcProjectile>();

	// Token: 0x04004677 RID: 18039
	private bool isDiagonal;
}
