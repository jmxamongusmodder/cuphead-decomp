using System;
using UnityEngine;

// Token: 0x020005AA RID: 1450
public class DicePalaceCardLevelSpikes : AbstractCollidableObject
{
	// Token: 0x06001BED RID: 7149 RVA: 0x000FFE95 File Offset: 0x000FE295
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001BEE RID: 7150 RVA: 0x000FFEA8 File Offset: 0x000FE2A8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001BEF RID: 7151 RVA: 0x000FFEC6 File Offset: 0x000FE2C6
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x04002504 RID: 9476
	private DamageDealer damageDealer;
}
