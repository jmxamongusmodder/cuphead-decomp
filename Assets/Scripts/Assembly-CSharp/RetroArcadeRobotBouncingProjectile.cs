using System;
using UnityEngine;

// Token: 0x02000753 RID: 1875
public class RetroArcadeRobotBouncingProjectile : AbstractProjectile
{
	// Token: 0x170003E0 RID: 992
	// (get) Token: 0x060028DD RID: 10461 RVA: 0x0017C96A File Offset: 0x0017AD6A
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x170003E1 RID: 993
	// (get) Token: 0x060028DE RID: 10462 RVA: 0x0017C971 File Offset: 0x0017AD71
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060028DF RID: 10463 RVA: 0x0017C974 File Offset: 0x0017AD74
	public RetroArcadeRobotBouncingProjectile Create(Vector2 pos, float speed, float angle, bool bounce)
	{
		RetroArcadeRobotBouncingProjectile retroArcadeRobotBouncingProjectile = this.InstantiatePrefab<RetroArcadeRobotBouncingProjectile>();
		retroArcadeRobotBouncingProjectile.transform.position = pos;
		retroArcadeRobotBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle);
		retroArcadeRobotBouncingProjectile.bounce = bounce;
		return retroArcadeRobotBouncingProjectile;
	}

	// Token: 0x060028E0 RID: 10464 RVA: 0x0017C9B4 File Offset: 0x0017ADB4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060028E1 RID: 10465 RVA: 0x0017C9CC File Offset: 0x0017ADCC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.dead)
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		float radius = base.GetComponent<CircleCollider2D>().radius;
		if (this.bounce)
		{
			if ((this.velocity.x < 0f && base.transform.position.x < (float)Level.Current.Left + radius) || (this.velocity.x > 0f && base.transform.position.x > (float)Level.Current.Right - radius))
			{
				this.velocity.x = this.velocity.x * -1f;
			}
			if (this.velocity.y < 0f && base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				this.velocity.y = this.velocity.y * -1f;
			}
		}
	}

	// Token: 0x040031BA RID: 12730
	private bool bounce;

	// Token: 0x040031BB RID: 12731
	private float attackDelay;

	// Token: 0x040031BC RID: 12732
	private Vector2 velocity;

	// Token: 0x040031BD RID: 12733
	private DevilLevelSittingDevil parent;
}
