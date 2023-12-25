using System;

// Token: 0x02000A63 RID: 2659
public class WeaponAccuracy : AbstractLevelWeapon
{
	// Token: 0x17000575 RID: 1397
	// (get) Token: 0x06003F72 RID: 16242 RVA: 0x0022AE89 File Offset: 0x00229289
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000576 RID: 1398
	// (get) Token: 0x06003F73 RID: 16243 RVA: 0x0022AE8C File Offset: 0x0022928C
	protected override float rapidFireRate
	{
		get
		{
			return this.fireRate;
		}
	}

	// Token: 0x06003F74 RID: 16244 RVA: 0x0022AE94 File Offset: 0x00229294
	private void Start()
	{
		this.level = WeaponAccuracy.Levels.One;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage;
	}

	// Token: 0x06003F75 RID: 16245 RVA: 0x0022AECC File Offset: 0x002292CC
	protected override AbstractProjectile fireBasic()
	{
		WeaponAccuracyProjectile weaponAccuracyProjectile = base.fireBasic() as WeaponAccuracyProjectile;
		weaponAccuracyProjectile.Speed = this.speed;
		weaponAccuracyProjectile.PlayerId = this.player.id;
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponAccuracyProjectile.EnemyDeath = new WeaponAccuracyProjectile.OnEnemyDeath(this.EnemyHit);
		weaponAccuracyProjectile.Damage = this.damage;
		weaponAccuracyProjectile.SetSize(this.size);
		return weaponAccuracyProjectile;
	}

	// Token: 0x06003F76 RID: 16246 RVA: 0x0022AF38 File Offset: 0x00229338
	protected override AbstractProjectile fireEx()
	{
		WeaponAccuracyProjectile weaponAccuracyProjectile = base.fireEx() as WeaponAccuracyProjectile;
		weaponAccuracyProjectile.Speed = WeaponProperties.LevelWeaponAccuracy.Ex.exSpeed;
		weaponAccuracyProjectile.Damage = WeaponProperties.LevelWeaponAccuracy.Ex.exDamage;
		weaponAccuracyProjectile.SetSize(WeaponProperties.LevelWeaponAccuracy.Ex.exShotSize);
		weaponAccuracyProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponAccuracyProjectile.PlayerId = this.player.id;
		weaponAccuracyProjectile.EnemyDeath = new WeaponAccuracyProjectile.OnEnemyDeath(this.EXEnemyHit);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponAccuracyProjectile);
		return weaponAccuracyProjectile;
	}

	// Token: 0x06003F77 RID: 16247 RVA: 0x0022AFAF File Offset: 0x002293AF
	private void EXEnemyHit(bool exEnemyHit)
	{
		if (exEnemyHit)
		{
			this.shotCounter += WeaponProperties.LevelWeaponAccuracy.Ex.exShotEquivalent;
			this.CheckLevels();
		}
		else
		{
			this.shotCounter = 0;
			this.LevelOne();
		}
	}

	// Token: 0x06003F78 RID: 16248 RVA: 0x0022AFE1 File Offset: 0x002293E1
	private void EnemyHit(bool enemyHit)
	{
		if (enemyHit)
		{
			this.shotCounter++;
			this.CheckLevels();
		}
		else
		{
			this.shotCounter = 0;
			this.LevelOne();
		}
	}

	// Token: 0x06003F79 RID: 16249 RVA: 0x0022B010 File Offset: 0x00229410
	private void CheckLevels()
	{
		switch (this.level)
		{
		case WeaponAccuracy.Levels.One:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoCounter)
			{
				this.LevelTwo();
			}
			break;
		case WeaponAccuracy.Levels.Two:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeCounter)
			{
				this.LevelThree();
			}
			break;
		case WeaponAccuracy.Levels.Three:
			if (this.shotCounter >= WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourCounter)
			{
				this.LevelFour();
			}
			break;
		case WeaponAccuracy.Levels.Four:
			break;
		default:
			this.LevelOne();
			break;
		}
	}

	// Token: 0x06003F7A RID: 16250 RVA: 0x0022B0A0 File Offset: 0x002294A0
	private void LevelOne()
	{
		this.level = WeaponAccuracy.Levels.One;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlOneDamage;
	}

	// Token: 0x06003F7B RID: 16251 RVA: 0x0022B0D5 File Offset: 0x002294D5
	private void LevelTwo()
	{
		this.level = WeaponAccuracy.Levels.Two;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlTwoDamage;
	}

	// Token: 0x06003F7C RID: 16252 RVA: 0x0022B10A File Offset: 0x0022950A
	private void LevelThree()
	{
		this.level = WeaponAccuracy.Levels.Three;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlThreeDamage;
	}

	// Token: 0x06003F7D RID: 16253 RVA: 0x0022B13F File Offset: 0x0022953F
	private void LevelFour()
	{
		this.level = WeaponAccuracy.Levels.Four;
		this.speed = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSpeed;
		this.fireRate = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourFireRate;
		this.size = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourSize;
		this.damage = WeaponProperties.LevelWeaponAccuracy.Basic.LvlFourDamage;
	}

	// Token: 0x04004669 RID: 18025
	private int shotCounter;

	// Token: 0x0400466A RID: 18026
	private WeaponAccuracy.Levels level;

	// Token: 0x0400466B RID: 18027
	private float speed;

	// Token: 0x0400466C RID: 18028
	private float fireRate;

	// Token: 0x0400466D RID: 18029
	private float size;

	// Token: 0x0400466E RID: 18030
	private float damage;

	// Token: 0x02000A64 RID: 2660
	private enum Levels
	{
		// Token: 0x04004670 RID: 18032
		One,
		// Token: 0x04004671 RID: 18033
		Two,
		// Token: 0x04004672 RID: 18034
		Three,
		// Token: 0x04004673 RID: 18035
		Four
	}
}
