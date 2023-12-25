using System;
using UnityEngine;

// Token: 0x020006F1 RID: 1777
public class MouseLevelGhostMouseBall : AbstractProjectile
{
	// Token: 0x06002618 RID: 9752 RVA: 0x0016435C File Offset: 0x0016275C
	public MouseLevelGhostMouseBall Create(Vector2 pos, float speed, float childSpeed)
	{
		MouseLevelGhostMouseBall mouseLevelGhostMouseBall = this.InstantiatePrefab<MouseLevelGhostMouseBall>();
		Vector2 a = new Vector2(PlayerManager.GetNext().transform.position.x, (float)Level.Current.Ground);
		Vector2 normalized = (a - pos).normalized;
		mouseLevelGhostMouseBall.transform.position = pos;
		mouseLevelGhostMouseBall.velocity = speed * normalized;
		mouseLevelGhostMouseBall.childSpeed = childSpeed;
		mouseLevelGhostMouseBall.state = MouseLevelGhostMouseBall.State.Moving;
		mouseLevelGhostMouseBall.transform.Rotate(0f, 0f, MathUtils.DirectionToAngle(normalized) - 90f);
		return mouseLevelGhostMouseBall;
	}

	// Token: 0x06002619 RID: 9753 RVA: 0x001643F8 File Offset: 0x001627F8
	protected override void Update()
	{
		base.Update();
		if (this.state == MouseLevelGhostMouseBall.State.Moving)
		{
			if (base.transform.position.y < (float)Level.Current.Ground)
			{
				this.Explode();
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
		}
	}

	// Token: 0x0600261A RID: 9754 RVA: 0x0016447D File Offset: 0x0016287D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600261B RID: 9755 RVA: 0x001644A8 File Offset: 0x001628A8
	private void Explode()
	{
		this.state = MouseLevelGhostMouseBall.State.Dead;
		this.childProjectile.Create(base.transform.position, 0f, Vector2.one, this.childSpeed);
		this.childProjectile.Create(base.transform.position, 0f, new Vector2(1f, -1f), -this.childSpeed);
		this.Die();
	}

	// Token: 0x0600261C RID: 9756 RVA: 0x00164525 File Offset: 0x00162925
	protected override void Die()
	{
		base.Die();
		base.transform.SetLocalEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x0600261D RID: 9757 RVA: 0x00164556 File Offset: 0x00162956
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.childProjectile = null;
	}

	// Token: 0x04002E98 RID: 11928
	[SerializeField]
	private BasicProjectile childProjectile;

	// Token: 0x04002E99 RID: 11929
	private MouseLevelGhostMouseBall.State state;

	// Token: 0x04002E9A RID: 11930
	private Vector2 velocity;

	// Token: 0x04002E9B RID: 11931
	private float childSpeed;

	// Token: 0x020006F2 RID: 1778
	public enum State
	{
		// Token: 0x04002E9D RID: 11933
		Init,
		// Token: 0x04002E9E RID: 11934
		Moving,
		// Token: 0x04002E9F RID: 11935
		Dead
	}
}
