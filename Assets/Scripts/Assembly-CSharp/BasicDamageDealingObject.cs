using System;
using UnityEngine;

// Token: 0x02000430 RID: 1072
[RequireComponent(typeof(Collider2D))]
public class BasicDamageDealingObject : AbstractCollidableObject
{
	// Token: 0x06000FA5 RID: 4005 RVA: 0x0009C651 File Offset: 0x0009AA51
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageDealer.SetRate(this.damageRate);
	}

	// Token: 0x06000FA6 RID: 4006 RVA: 0x0009C675 File Offset: 0x0009AA75
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06000FA7 RID: 4007 RVA: 0x0009C693 File Offset: 0x0009AA93
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x040018D8 RID: 6360
	[SerializeField]
	private float damageRate = 0.2f;

	// Token: 0x040018D9 RID: 6361
	private DamageDealer damageDealer;
}
