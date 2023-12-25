using System;
using UnityEngine;

// Token: 0x02000A6B RID: 2667
public class WeaponBoomerang : AbstractLevelWeapon
{
	// Token: 0x1700057B RID: 1403
	// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x0022B9BB File Offset: 0x00229DBB
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700057C RID: 1404
	// (get) Token: 0x06003FA2 RID: 16290 RVA: 0x0022B9BE File Offset: 0x00229DBE
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponBoomerang.Basic.fireRate;
		}
	}

	// Token: 0x06003FA3 RID: 16291 RVA: 0x0022B9C8 File Offset: 0x00229DC8
	protected override void Awake()
	{
		base.Awake();
		string[] array = WeaponProperties.LevelWeaponBoomerang.Basic.xDistanceString.Split(new char[]
		{
			','
		});
		string[] array2 = WeaponProperties.LevelWeaponBoomerang.Basic.yDistanceString.Split(new char[]
		{
			','
		});
		this.distances = new Vector2[Mathf.Min(array.Length, array2.Length)];
		for (int i = 0; i < this.distances.Length; i++)
		{
			Parser.FloatTryParse(array[i], out this.distances[i].x);
			Parser.FloatTryParse(array2[i], out this.distances[i].y);
		}
		this.distanceIndex = UnityEngine.Random.Range(0, this.distances.Length);
	}

	// Token: 0x06003FA4 RID: 16292 RVA: 0x0022BA7E File Offset: 0x00229E7E
	public override void BeginBasic()
	{
		this.BeginBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2");
		base.BeginBasic();
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x0022BA98 File Offset: 0x00229E98
	protected override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_weapon_boomerang", "player_weapon_boomerang_p2");
		WeaponBoomerangProjectile weaponBoomerangProjectile = base.fireBasic() as WeaponBoomerangProjectile;
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Basic.speed;
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Basic.damage;
		weaponBoomerangProjectile.PlayerId = this.player.id;
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault();
		weaponBoomerangProjectile.CollisionDeath.PlayerProjectileDefault();
		weaponBoomerangProjectile.CollisionDeath.Other = false;
		weaponBoomerangProjectile.player = this.player;
		this.distanceIndex = (this.distanceIndex + 1) % this.distances.Length;
		weaponBoomerangProjectile.forwardDistance = this.distances[this.distanceIndex].x;
		weaponBoomerangProjectile.lateralDistance = this.distances[this.distanceIndex].y;
		return weaponBoomerangProjectile;
	}

	// Token: 0x06003FA6 RID: 16294 RVA: 0x0022BB67 File Offset: 0x00229F67
	public override void EndBasic()
	{
		base.EndBasic();
		this.EndBasicCheckAttenuation("player_weapon_boomerang", "player_weapon_boomerang_p2");
	}

	// Token: 0x06003FA7 RID: 16295 RVA: 0x0022BB80 File Offset: 0x00229F80
	protected override AbstractProjectile fireEx()
	{
		WeaponBoomerangProjectile weaponBoomerangProjectile = base.fireEx() as WeaponBoomerangProjectile;
		weaponBoomerangProjectile.Speed = WeaponProperties.LevelWeaponBoomerang.Ex.speed;
		weaponBoomerangProjectile.Damage = WeaponProperties.LevelWeaponBoomerang.Ex.damage;
		weaponBoomerangProjectile.maxDamage = WeaponProperties.LevelWeaponBoomerang.Ex.maxDamage * PlayerManager.DamageMultiplier;
		weaponBoomerangProjectile.PlayerId = this.player.id;
		weaponBoomerangProjectile.hitFreezeTime = WeaponProperties.LevelWeaponBoomerang.Ex.hitFreezeTime;
		weaponBoomerangProjectile.DamageRate = WeaponProperties.LevelWeaponBoomerang.Ex.damageRate + weaponBoomerangProjectile.hitFreezeTime;
		weaponBoomerangProjectile.DamagesType.PlayerProjectileDefault();
		weaponBoomerangProjectile.forwardDistance = WeaponProperties.LevelWeaponBoomerang.Ex.xDistance;
		weaponBoomerangProjectile.lateralDistance = WeaponProperties.LevelWeaponBoomerang.Ex.yDistance;
		weaponBoomerangProjectile.player = this.player;
		weaponBoomerangProjectile.CollisionDeath.Other = false;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponBoomerangProjectile);
		return weaponBoomerangProjectile;
	}

	// Token: 0x04004683 RID: 18051
	private int distanceIndex;

	// Token: 0x04004684 RID: 18052
	private Vector2[] distances;
}
