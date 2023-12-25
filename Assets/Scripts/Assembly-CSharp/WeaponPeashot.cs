using System;

// Token: 0x02000A7F RID: 2687
public class WeaponPeashot : AbstractLevelWeapon
{
	// Token: 0x1700058E RID: 1422
	// (get) Token: 0x0600403F RID: 16447 RVA: 0x00230999 File Offset: 0x0022ED99
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.LevelWeaponPeashot.Basic.rapidFire;
		}
	}

	// Token: 0x1700058F RID: 1423
	// (get) Token: 0x06004040 RID: 16448 RVA: 0x002309A0 File Offset: 0x0022EDA0
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponPeashot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06004041 RID: 16449 RVA: 0x002309A8 File Offset: 0x0022EDA8
	protected override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.LevelWeaponPeashot.Basic.speed;
		basicProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		basicProjectile.DamagesType.PlayerProjectileDefault();
		basicProjectile.CollisionDeath.PlayerProjectileDefault();
		float y = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, y, 0f);
		return basicProjectile;
	}

	// Token: 0x06004042 RID: 16450 RVA: 0x00230A4C File Offset: 0x0022EE4C
	protected override AbstractProjectile fireEx()
	{
		WeaponPeashotExProjectile weaponPeashotExProjectile = base.fireEx() as WeaponPeashotExProjectile;
		weaponPeashotExProjectile.moveSpeed = WeaponProperties.LevelWeaponPeashot.Ex.speed;
		weaponPeashotExProjectile.Damage = WeaponProperties.LevelWeaponPeashot.Ex.damage;
		weaponPeashotExProjectile.hitFreezeTime = WeaponProperties.LevelWeaponPeashot.Ex.freezeTime;
		weaponPeashotExProjectile.DamageRate = weaponPeashotExProjectile.hitFreezeTime + WeaponProperties.LevelWeaponPeashot.Ex.damageDistance / weaponPeashotExProjectile.moveSpeed;
		weaponPeashotExProjectile.maxDamage = WeaponProperties.LevelWeaponPeashot.Ex.maxDamage;
		weaponPeashotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponPeashotExProjectile);
		return weaponPeashotExProjectile;
	}

	// Token: 0x06004043 RID: 16451 RVA: 0x00230ACA File Offset: 0x0022EECA
	public override void BeginBasic()
	{
		this.OneShotCooldown("player_default_fire_start");
		this.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2");
		base.BeginBasic();
	}

	// Token: 0x06004044 RID: 16452 RVA: 0x00230AED File Offset: 0x0022EEED
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
		this.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2");
	}

	// Token: 0x04004711 RID: 18193
	private const float Y_POS = 20f;

	// Token: 0x04004712 RID: 18194
	private const float ROTATION_OFFSET = 3f;

	// Token: 0x04004713 RID: 18195
	private float[] yPositions = new float[]
	{
		0f,
		20f,
		40f,
		20f
	};

	// Token: 0x04004714 RID: 18196
	private int currentY;
}
