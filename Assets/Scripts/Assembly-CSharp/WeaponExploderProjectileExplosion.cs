using System;
using UnityEngine;

// Token: 0x02000A78 RID: 2680
public class WeaponExploderProjectileExplosion : Effect
{
	// Token: 0x0600400B RID: 16395 RVA: 0x0022EF9C File Offset: 0x0022D39C
	public void Create(Vector2 position, float radius, float damage, float damageMultiplier, WeaponExploder weapon, MeterScoreTracker tracker)
	{
		float num = radius / 15f;
		WeaponExploderProjectileExplosion weaponExploderProjectileExplosion = base.Create(position, new Vector3(num, num, 1f)) as WeaponExploderProjectileExplosion;
		weaponExploderProjectileExplosion.damageDealer.SetDamage(damage);
		weaponExploderProjectileExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		weaponExploderProjectileExplosion.damageDealer.SetDamageFlags(false, true, false);
		weaponExploderProjectileExplosion.weapon = weapon;
		weaponExploderProjectileExplosion.damageDealer.OnDealDamage += weaponExploderProjectileExplosion.OnDealDamage;
		if (tracker != null)
		{
			tracker.Add(weaponExploderProjectileExplosion.damageDealer);
		}
	}

	// Token: 0x0600400C RID: 16396 RVA: 0x0022F030 File Offset: 0x0022D430
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = new DamageDealer(1f, 0f);
	}

	// Token: 0x0600400D RID: 16397 RVA: 0x0022F04D File Offset: 0x0022D44D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600400E RID: 16398 RVA: 0x0022F065 File Offset: 0x0022D465
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600400F RID: 16399 RVA: 0x0022F08D File Offset: 0x0022D48D
	private void OnDealDamage(float damage, DamageReceiver damageReceiver, DamageDealer damageDealer)
	{
		if (this.weapon != null)
		{
			this.weapon.OnDealDamage(damage, damageReceiver, damageDealer);
		}
	}

	// Token: 0x040046D6 RID: 18134
	private DamageDealer damageDealer;

	// Token: 0x040046D7 RID: 18135
	private const float BASE_RADIUS = 15f;

	// Token: 0x040046D8 RID: 18136
	private WeaponExploder weapon;
}
