using System;
using UnityEngine;

// Token: 0x0200089D RID: 2205
public class CircusPlatformingLevelBallRunnerBall : AbstractCollidableObject
{
	// Token: 0x0600334F RID: 13135 RVA: 0x001DDD3F File Offset: 0x001DC13F
	private void Start()
	{
		base.animator.SetBool("isBlue", Rand.Bool());
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06003350 RID: 13136 RVA: 0x001DDD64 File Offset: 0x001DC164
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (hit.GetComponentInParent<PlatformingLevelEditorPlatform>() && phase == CollisionPhase.Enter && this.isMoving)
		{
			this.direction *= -1f;
		}
	}

	// Token: 0x06003351 RID: 13137 RVA: 0x001DDDB0 File Offset: 0x001DC1B0
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003352 RID: 13138 RVA: 0x001DDDC8 File Offset: 0x001DC1C8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003353 RID: 13139 RVA: 0x001DDDE6 File Offset: 0x001DC1E6
	private void FixedUpdate()
	{
		if (this.isMoving)
		{
			this.Move();
		}
	}

	// Token: 0x06003354 RID: 13140 RVA: 0x001DDDF9 File Offset: 0x001DC1F9
	protected virtual void Move()
	{
		base.transform.position += this.direction * this.Speed * CupheadTime.FixedDelta;
	}

	// Token: 0x04003B9B RID: 15259
	public bool isMoving;

	// Token: 0x04003B9C RID: 15260
	[SerializeField]
	private float Speed = 500f;

	// Token: 0x04003B9D RID: 15261
	[SerializeField]
	private CircusPlatformingLevelBallRunner runner;

	// Token: 0x04003B9E RID: 15262
	private DamageDealer damageDealer;

	// Token: 0x04003B9F RID: 15263
	public Vector3 direction = Vector3.right;
}
