using System;
using UnityEngine;

// Token: 0x02000557 RID: 1367
public class ClownLevelBomb : AbstractCollidableObject
{
	// Token: 0x06001983 RID: 6531 RVA: 0x000E7BAC File Offset: 0x000E5FAC
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000E7BBF File Offset: 0x000E5FBF
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x000E7BD7 File Offset: 0x000E5FD7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x040022A5 RID: 8869
	private DamageDealer damageDealer;
}
