using System;

// Token: 0x02000A83 RID: 2691
public class WeaponSplitter : AbstractLevelWeapon
{
	// Token: 0x17000592 RID: 1426
	// (get) Token: 0x06004053 RID: 16467 RVA: 0x002310C9 File Offset: 0x0022F4C9
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000593 RID: 1427
	// (get) Token: 0x06004054 RID: 16468 RVA: 0x002310CC File Offset: 0x0022F4CC
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponSplitter.Basic.fireRate;
		}
	}

	// Token: 0x06004055 RID: 16469 RVA: 0x002310D4 File Offset: 0x0022F4D4
	protected override AbstractProjectile fireBasic()
	{
		WeaponSplitterProjectile weaponSplitterProjectile = base.fireBasic() as WeaponSplitterProjectile;
		weaponSplitterProjectile.Speed = WeaponProperties.LevelWeaponSplitter.Basic.speed;
		weaponSplitterProjectile.Damage = WeaponProperties.LevelWeaponSplitter.Basic.bulletDamage;
		weaponSplitterProjectile.isMain = true;
		weaponSplitterProjectile.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceA;
		weaponSplitterProjectile.PlayerId = this.player.id;
		weaponSplitterProjectile.DamagesType.PlayerProjectileDefault();
		weaponSplitterProjectile.CollisionDeath.PlayerProjectileDefault();
		return weaponSplitterProjectile;
	}

	// Token: 0x06004056 RID: 16470 RVA: 0x00231140 File Offset: 0x0022F540
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

	// Token: 0x06004057 RID: 16471 RVA: 0x002311BE File Offset: 0x0022F5BE
	public override void BeginBasic()
	{
		this.OneShotCooldown("player_default_fire_start");
		this.BasicSoundLoop("player_default_fire_loop", "player_default_fire_loop_p2");
		base.BeginBasic();
	}

	// Token: 0x06004058 RID: 16472 RVA: 0x002311E1 File Offset: 0x0022F5E1
	public override void EndBasic()
	{
		this.ActivateCooldown();
		base.EndBasic();
		this.StopLoopSound("player_default_fire_loop", "player_default_fire_loop_p2");
	}

	// Token: 0x0400472C RID: 18220
	private const float ROTATION_OFFSET = 3f;
}
