using System;
using UnityEngine;

// Token: 0x02000A72 RID: 2674
public class WeaponCrackshot : AbstractLevelWeapon
{
	// Token: 0x17000584 RID: 1412
	// (get) Token: 0x06003FDA RID: 16346 RVA: 0x0022D64D File Offset: 0x0022BA4D
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000585 RID: 1413
	// (get) Token: 0x06003FDB RID: 16347 RVA: 0x0022D650 File Offset: 0x0022BA50
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponCrackshot.Basic.fireRate;
		}
	}

	// Token: 0x06003FDC RID: 16348 RVA: 0x0022D658 File Offset: 0x0022BA58
	protected override AbstractProjectile fireBasic()
	{
		WeaponCrackshotProjectile weaponCrackshotProjectile = base.fireBasic() as WeaponCrackshotProjectile;
		weaponCrackshotProjectile.Speed = WeaponProperties.LevelWeaponCrackshot.Basic.initialSpeed;
		weaponCrackshotProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Basic.initialDamage;
		weaponCrackshotProjectile.PlayerId = this.player.id;
		weaponCrackshotProjectile.DamagesType.PlayerProjectileDefault();
		weaponCrackshotProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponCrackshotProjectile.maxAngleRange = ((!WeaponProperties.LevelWeaponCrackshot.Basic.enableMaxAngle) ? 180f : WeaponProperties.LevelWeaponCrackshot.Basic.maxAngle);
		weaponCrackshotProjectile.variant = this.variantString.PopInt();
		weaponCrackshotProjectile.useBComet = this.useBComet;
		this.useBComet = !this.useBComet;
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		weaponCrackshotProjectile.transform.AddPosition(0f, y, 0f);
		return weaponCrackshotProjectile;
	}

	// Token: 0x06003FDD RID: 16349 RVA: 0x0022D748 File Offset: 0x0022BB48
	protected override AbstractProjectile fireEx()
	{
		if (this.activeEX)
		{
			this.activeEX.GetReplaced();
		}
		WeaponCrackshotExProjectile weaponCrackshotExProjectile = base.fireEx() as WeaponCrackshotExProjectile;
		weaponCrackshotExProjectile.Damage = WeaponProperties.LevelWeaponCrackshot.Ex.collideDamage;
		weaponCrackshotExProjectile.DamageRate = 0f;
		weaponCrackshotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponCrackshotExProjectile);
		this.activeEX = weaponCrackshotExProjectile;
		return weaponCrackshotExProjectile;
	}

	// Token: 0x06003FDE RID: 16350 RVA: 0x0022D7B9 File Offset: 0x0022BBB9
	public override void BeginBasic()
	{
		AudioManager.Play("player_weapon_crackshot_shoot_start");
		this.emitAudioFromObject.Add("player_weapon_crackshot_shoot_start");
		this.variantString = new PatternString("0,1,0,2,1,2,0,1,2", true);
		base.BeginBasic();
	}

	// Token: 0x06003FDF RID: 16351 RVA: 0x0022D7EC File Offset: 0x0022BBEC
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
	}

	// Token: 0x040046B3 RID: 18099
	private const float Y_POS = 20f;

	// Token: 0x040046B4 RID: 18100
	private const float ROTATION_OFFSET = 3f;

	// Token: 0x040046B5 RID: 18101
	private float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x040046B6 RID: 18102
	private int currentY;

	// Token: 0x040046B7 RID: 18103
	[SerializeField]
	private PatternString variantString;

	// Token: 0x040046B8 RID: 18104
	private bool useBComet;

	// Token: 0x040046B9 RID: 18105
	private WeaponCrackshotExProjectile activeEX;
}
