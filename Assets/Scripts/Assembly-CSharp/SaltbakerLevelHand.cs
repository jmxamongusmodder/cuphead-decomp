using System;
using UnityEngine;

// Token: 0x020007C9 RID: 1993
public class SaltbakerLevelHand : AbstractCollidableObject
{
	// Token: 0x06002D32 RID: 11570 RVA: 0x001A9E36 File Offset: 0x001A8236
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002D33 RID: 11571 RVA: 0x001A9E43 File Offset: 0x001A8243
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002D34 RID: 11572 RVA: 0x001A9E5B File Offset: 0x001A825B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002D35 RID: 11573 RVA: 0x001A9E7C File Offset: 0x001A827C
	public void Shoot(float speed)
	{
		float rotation = (!this.leftHand) ? 180f : 0f;
		this.projectile.Create(this.root.position, rotation, speed);
	}

	// Token: 0x040035A9 RID: 13737
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040035AA RID: 13738
	[SerializeField]
	private Transform root;

	// Token: 0x040035AB RID: 13739
	[SerializeField]
	private bool leftHand;

	// Token: 0x040035AC RID: 13740
	private DamageDealer damageDealer;
}
