using System;
using UnityEngine;

// Token: 0x020007D0 RID: 2000
public class SaltbakerLevelPestle : AbstractProjectile
{
	// Token: 0x06002D67 RID: 11623 RVA: 0x001AC9C6 File Offset: 0x001AADC6
	public SaltbakerLevelPestle Init(Vector3 spawnPos, float velocityX, float velocityY, float gravity)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = spawnPos;
		this.speed = new Vector3(velocityX, velocityY);
		this.gravity = gravity;
		return this;
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x001AC9F6 File Offset: 0x001AADF6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x001ACA14 File Offset: 0x001AAE14
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.Move();
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x001ACA24 File Offset: 0x001AAE24
	private void Move()
	{
		this.speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
		base.transform.Translate(this.speed * CupheadTime.FixedDelta);
	}

	// Token: 0x040035ED RID: 13805
	private Vector3 speed;

	// Token: 0x040035EE RID: 13806
	private float gravity;
}
