using System;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class FlowerLevelFlowerDamageRegion : CollisionChild
{
	// Token: 0x06001F14 RID: 7956 RVA: 0x0011DF3B File Offset: 0x0011C33B
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x06001F15 RID: 7957 RVA: 0x0011DF4E File Offset: 0x0011C34E
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001F16 RID: 7958 RVA: 0x0011DF66 File Offset: 0x0011C366
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x040027B5 RID: 10165
	private DamageDealer damageDealer;
}
