using System;
using UnityEngine;

// Token: 0x02000A88 RID: 2696
public class WeaponSpread : AbstractLevelWeapon
{
	// Token: 0x17000594 RID: 1428
	// (get) Token: 0x0600406F RID: 16495 RVA: 0x00231776 File Offset: 0x0022FB76
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000595 RID: 1429
	// (get) Token: 0x06004070 RID: 16496 RVA: 0x00231779 File Offset: 0x0022FB79
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponSpreadshot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06004071 RID: 16497 RVA: 0x00231780 File Offset: 0x0022FB80
	protected override AbstractProjectile fireBasic()
	{
		float[] array = new float[]
		{
			0.5f,
			0.75f
		};
		float damage = WeaponProperties.LevelWeaponSpreadshot.Basic.damage;
		for (int i = 0; i < 2; i++)
		{
			BasicProjectile basicProjectile = base.fireBasicNoEffect() as BasicProjectile;
			basicProjectile.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array[i];
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20f * (float)(i + 1);
			basicProjectile.Damage = damage;
			basicProjectile.PlayerId = this.player.id;
			basicProjectile.transform.AddEulerAngles(0f, 0f, 15f * (float)(i + 1));
			Animator component = basicProjectile.GetComponent<Animator>();
			component.SetBool("Large", i == 1);
			BasicProjectile basicProjectile2 = base.fireBasicNoEffect() as BasicProjectile;
			basicProjectile2.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed * array[i];
			basicProjectile2.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance - 20f * (float)(i + 1);
			basicProjectile2.Damage = damage;
			basicProjectile2.PlayerId = this.player.id;
			basicProjectile2.transform.AddEulerAngles(0f, 0f, -15f * (float)(i + 1));
			Animator component2 = basicProjectile2.GetComponent<Animator>();
			component2.SetBool("Large", i == 1);
		}
		BasicProjectile basicProjectile3 = base.fireBasic() as BasicProjectile;
		basicProjectile3.Speed = WeaponProperties.LevelWeaponSpreadshot.Basic.speed;
		basicProjectile3.Damage = damage;
		basicProjectile3.PlayerId = this.player.id;
		basicProjectile3.DestroyDistance = WeaponProperties.LevelWeaponSpreadshot.Basic.distance;
		return basicProjectile3;
	}

	// Token: 0x06004072 RID: 16498 RVA: 0x00231904 File Offset: 0x0022FD04
	protected override AbstractProjectile fireEx()
	{
		AudioManager.Play("player_weapon_exploder_fire");
		PlayerLevelSpreadEx playerLevelSpreadEx = base.fireEx() as PlayerLevelSpreadEx;
		playerLevelSpreadEx.Init(WeaponProperties.LevelWeaponSpreadshot.Ex.speed, WeaponProperties.LevelWeaponSpreadshot.Ex.damage, WeaponProperties.LevelWeaponSpreadshot.Ex.childCount, WeaponProperties.LevelWeaponSpreadshot.Ex.radius);
		return playerLevelSpreadEx;
	}

	// Token: 0x06004073 RID: 16499 RVA: 0x00231942 File Offset: 0x0022FD42
	public override void BeginBasic()
	{
		base.BeginBasic();
		this.BasicSoundLoop("player_weapon_spread_loop", "player_weapon_spread_loop_p2");
	}

	// Token: 0x06004074 RID: 16500 RVA: 0x0023195A File Offset: 0x0022FD5A
	public override void EndBasic()
	{
		base.EndBasic();
		this.StopLoopSound("player_weapon_spread_loop", "player_weapon_spread_loop_p2");
	}
}
