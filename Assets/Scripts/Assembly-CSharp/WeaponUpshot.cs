using System;
using UnityEngine;

// Token: 0x02000A89 RID: 2697
public class WeaponUpshot : AbstractLevelWeapon
{
	// Token: 0x17000596 RID: 1430
	// (get) Token: 0x06004076 RID: 16502 RVA: 0x00231991 File Offset: 0x0022FD91
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000597 RID: 1431
	// (get) Token: 0x06004077 RID: 16503 RVA: 0x00231994 File Offset: 0x0022FD94
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponUpshot.Basic.fireRate;
		}
	}

	// Token: 0x06004078 RID: 16504 RVA: 0x0023199C File Offset: 0x0022FD9C
	protected override AbstractProjectile fireBasic()
	{
		this.animationCycleCount++;
		for (int i = 0; i < 3; i++)
		{
			WeaponUpshotProjectile weaponUpshotProjectile = (i != 0) ? (base.fireBasicNoEffect() as WeaponUpshotProjectile) : (base.fireBasic() as WeaponUpshotProjectile);
			if (i == 1)
			{
				weaponUpshotProjectile.GetComponent<SpriteRenderer>().sortingOrder = 1;
			}
			weaponUpshotProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Basic.damage;
			weaponUpshotProjectile.PlayerId = this.player.id;
			weaponUpshotProjectile.DamagesType.PlayerProjectileDefault();
			weaponUpshotProjectile.CollisionDeath.PlayerProjectileDefault();
			weaponUpshotProjectile.CollisionDeath.Other = false;
			weaponUpshotProjectile.xSpeed = WeaponProperties.LevelWeaponUpshot.Basic.xSpeed[i];
			weaponUpshotProjectile.ySpeedMinMax = WeaponProperties.LevelWeaponUpshot.Basic.ySpeed[i];
			weaponUpshotProjectile.timeToArc = WeaponProperties.LevelWeaponUpshot.Basic.timeToMaxSpeed[i];
			weaponUpshotProjectile.animator.Play(((this.animationCycleCount + i) % 3).ToString(), 0, UnityEngine.Random.Range(0f, 1f));
		}
		return null;
	}

	// Token: 0x06004079 RID: 16505 RVA: 0x00231A98 File Offset: 0x0022FE98
	protected override AbstractProjectile fireEx()
	{
		WeaponUpshotExProjectile weaponUpshotExProjectile = base.fireEx() as WeaponUpshotExProjectile;
		weaponUpshotExProjectile.Damage = WeaponProperties.LevelWeaponUpshot.Ex.damage;
		weaponUpshotExProjectile.DamageRate = WeaponProperties.LevelWeaponUpshot.Ex.damageRate;
		weaponUpshotExProjectile.PlayerId = this.player.id;
		weaponUpshotExProjectile.rotateDir = Mathf.Sign(this.player.gameObject.transform.localScale.x);
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponUpshotExProjectile);
		return weaponUpshotExProjectile;
	}

	// Token: 0x0600407A RID: 16506 RVA: 0x00231B0F File Offset: 0x0022FF0F
	public override void BeginBasic()
	{
		base.BeginBasic();
		AudioManager.Play("player_weapon_upshot_start");
		this.emitAudioFromObject.Add("player_weapon_upshot_start");
		this.BasicSoundLoop("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
	}

	// Token: 0x0600407B RID: 16507 RVA: 0x00231B41 File Offset: 0x0022FF41
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_upshot_loop_p1", "player_weapon_upshot_loop_p2");
	}

	// Token: 0x04004737 RID: 18231
	private const int NUM_OF_BULLETS = 3;

	// Token: 0x04004738 RID: 18232
	private int[] xOffset = new int[]
	{
		-1,
		1,
		0,
		1,
		-1,
		0
	};

	// Token: 0x04004739 RID: 18233
	private int xIndex;

	// Token: 0x0400473A RID: 18234
	private int animationCycleCount;
}
