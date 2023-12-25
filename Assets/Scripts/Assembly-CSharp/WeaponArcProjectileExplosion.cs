using System;
using UnityEngine;

// Token: 0x02000A6A RID: 2666
public class WeaponArcProjectileExplosion : Effect
{
	// Token: 0x1700057A RID: 1402
	// (get) Token: 0x06003F9B RID: 16283 RVA: 0x0022B8F4 File Offset: 0x00229CF4
	public DamageDealer DamageDealer
	{
		get
		{
			return this.damageDealer;
		}
	}

	// Token: 0x06003F9C RID: 16284 RVA: 0x0022B8FC File Offset: 0x00229CFC
	public WeaponArcProjectileExplosion Create(Vector2 position, float damage, float damageMultiplier, PlayerId playerId)
	{
		WeaponArcProjectileExplosion weaponArcProjectileExplosion = base.Create(position) as WeaponArcProjectileExplosion;
		weaponArcProjectileExplosion.damageDealer.SetDamage(damage);
		weaponArcProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		weaponArcProjectileExplosion.damageDealer.SetDamageFlags(false, true, false);
		weaponArcProjectileExplosion.damageDealer.PlayerId = playerId;
		return weaponArcProjectileExplosion;
	}

	// Token: 0x06003F9D RID: 16285 RVA: 0x0022B956 File Offset: 0x00229D56
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0f);
	}

	// Token: 0x06003F9E RID: 16286 RVA: 0x0022B973 File Offset: 0x00229D73
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003F9F RID: 16287 RVA: 0x0022B98B File Offset: 0x00229D8B
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04004682 RID: 18050
	private DamageDealer damageDealer;
}
