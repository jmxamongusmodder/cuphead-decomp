using System;
using UnityEngine;

// Token: 0x02000A8E RID: 2702
public class WeaponWideShotProjectile : BasicProjectile
{
	// Token: 0x0600409E RID: 16542 RVA: 0x00232AB6 File Offset: 0x00230EB6
	protected override void Start()
	{
		base.Start();
		this.damageDealer.isDLCWeapon = true;
		base.GetComponent<SpriteRenderer>().flipY = Rand.Bool();
	}

	// Token: 0x0600409F RID: 16543 RVA: 0x00232ADA File Offset: 0x00230EDA
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		this.hitSpark.Create(base.transform.position + base.transform.right * 100f);
	}

	// Token: 0x060040A0 RID: 16544 RVA: 0x00232B18 File Offset: 0x00230F18
	protected override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		this.hitSpark.Create(base.transform.position + base.transform.right * 100f);
		base.OnCollisionDie(hit, phase);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04004758 RID: 18264
	private const float HITSPARK_OFFSET = 100f;

	// Token: 0x04004759 RID: 18265
	[SerializeField]
	private Effect hitSpark;
}
