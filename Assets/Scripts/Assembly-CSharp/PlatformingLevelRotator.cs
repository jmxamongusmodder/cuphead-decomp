using System;
using UnityEngine;

// Token: 0x0200090B RID: 2315
public class PlatformingLevelRotator : AbstractCollidableObject
{
	// Token: 0x06003650 RID: 13904 RVA: 0x001F7D3A File Offset: 0x001F613A
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x001F7D4D File Offset: 0x001F614D
	private void Update()
	{
		base.transform.AddEulerAngles(0f, 0f, -this.speed * CupheadTime.Delta);
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x001F7D8C File Offset: 0x001F618C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04003E42 RID: 15938
	public float speed = 180f;

	// Token: 0x04003E43 RID: 15939
	private DamageDealer damageDealer;
}
