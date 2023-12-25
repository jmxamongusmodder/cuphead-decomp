using System;
using UnityEngine;

// Token: 0x02000A6D RID: 2669
public class WeaponBouncer : AbstractLevelWeapon
{
	// Token: 0x1700057E RID: 1406
	// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x0022C78D File Offset: 0x0022AB8D
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700057F RID: 1407
	// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x0022C790 File Offset: 0x0022AB90
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponBouncer.Basic.fireRate;
		}
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x0022C798 File Offset: 0x0022AB98
	protected override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_weapon_bouncer", "player_weapon_bouncer_p2");
		WeaponBouncerProjectile weaponBouncerProjectile = base.fireBasic() as WeaponBouncerProjectile;
		float adjustedAngle = this.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z);
		weaponBouncerProjectile.transform.SetEulerAngles(null, null, new float?(0f));
		weaponBouncerProjectile.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle);
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity;
		weaponBouncerProjectile.bounceRatio = WeaponProperties.LevelWeaponBouncer.Basic.bounceRatio;
		weaponBouncerProjectile.bounceSpeedDampening = WeaponProperties.LevelWeaponBouncer.Basic.bounceSpeedDampening;
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Basic.damage;
		weaponBouncerProjectile.PlayerId = this.player.id;
		weaponBouncerProjectile.weapon = this;
		return weaponBouncerProjectile;
	}

	// Token: 0x06003FB8 RID: 16312 RVA: 0x0022C890 File Offset: 0x0022AC90
	private float getAdjustedAngle(float angle)
	{
		int num = Mathf.RoundToInt(angle);
		if (num == 0)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle;
		}
		else if (num == 45)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle;
		}
		else if (num == 135)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalUpExtraAngle;
		}
		else if (num == 180)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.straightExtraAngle;
		}
		else if (num == 225)
		{
			angle -= WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle;
		}
		else if (num == 315)
		{
			angle += WeaponProperties.LevelWeaponBouncer.Basic.diagonalDownExtraAngle;
		}
		return angle;
	}

	// Token: 0x06003FB9 RID: 16313 RVA: 0x0022C930 File Offset: 0x0022AD30
	protected override AbstractProjectile fireEx()
	{
		WeaponBouncerProjectile weaponBouncerProjectile = base.fireEx() as WeaponBouncerProjectile;
		float adjustedAngle = this.getAdjustedAngle(weaponBouncerProjectile.transform.rotation.eulerAngles.z);
		weaponBouncerProjectile.transform.SetEulerAngles(null, null, new float?(0f));
		weaponBouncerProjectile.transform.SetScale(new float?(1f), new float?(1f), new float?(1f));
		weaponBouncerProjectile.velocity = WeaponProperties.LevelWeaponBouncer.Basic.launchSpeed * MathUtils.AngleToDirection(adjustedAngle);
		weaponBouncerProjectile.gravity = WeaponProperties.LevelWeaponBouncer.Basic.gravity;
		weaponBouncerProjectile.weapon = this;
		weaponBouncerProjectile.Damage = WeaponProperties.LevelWeaponBouncer.Ex.damage;
		weaponBouncerProjectile.PlayerId = this.player.id;
		return weaponBouncerProjectile;
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x0022CA02 File Offset: 0x0022AE02
	public override void BeginBasic()
	{
		this.BeginBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2");
		base.BeginBasic();
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x0022CA1A File Offset: 0x0022AE1A
	public override void EndBasic()
	{
		this.EndBasicCheckAttenuation("player_weapon_bouncer", "player_weapon_bouncer_p2");
		base.EndBasic();
	}
}
