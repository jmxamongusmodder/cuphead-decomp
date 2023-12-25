using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000594 RID: 1428
public class DevilLevelPitchforkWheelProjectile : AbstractProjectile
{
	// Token: 0x17000358 RID: 856
	// (get) Token: 0x06001B5E RID: 7006 RVA: 0x000FA958 File Offset: 0x000F8D58
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000359 RID: 857
	// (get) Token: 0x06001B5F RID: 7007 RVA: 0x000FA95B File Offset: 0x000F8D5B
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001B60 RID: 7008 RVA: 0x000FA964 File Offset: 0x000F8D64
	public DevilLevelPitchforkWheelProjectile Create(Vector2 pos, float attackDelay, float speed, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkWheelProjectile devilLevelPitchforkWheelProjectile = this.InstantiatePrefab<DevilLevelPitchforkWheelProjectile>();
		devilLevelPitchforkWheelProjectile.transform.position = pos;
		devilLevelPitchforkWheelProjectile.attackDelay = attackDelay;
		devilLevelPitchforkWheelProjectile.speed = speed;
		devilLevelPitchforkWheelProjectile.state = DevilLevelPitchforkWheelProjectile.State.Idle;
		devilLevelPitchforkWheelProjectile.StartCoroutine(devilLevelPitchforkWheelProjectile.main_cr());
		devilLevelPitchforkWheelProjectile.parent = parent;
		return devilLevelPitchforkWheelProjectile;
	}

	// Token: 0x06001B61 RID: 7009 RVA: 0x000FA9B4 File Offset: 0x000F8DB4
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B62 RID: 7010 RVA: 0x000FA9D3 File Offset: 0x000F8DD3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B63 RID: 7011 RVA: 0x000FA9F4 File Offset: 0x000F8DF4
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!base.dead && this.state != DevilLevelPitchforkWheelProjectile.State.Idle)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		}
	}

	// Token: 0x06001B64 RID: 7012 RVA: 0x000FAA50 File Offset: 0x000F8E50
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.attackDelay);
		this.state = DevilLevelPitchforkWheelProjectile.State.Attacking;
		this.velocity = this.speed * (PlayerManager.GetNext().center - base.transform.position).normalized;
		while (this.state == DevilLevelPitchforkWheelProjectile.State.Attacking)
		{
			float colliderRadius = base.GetComponent<CircleCollider2D>().radius;
			if (base.transform.position.x < (float)Level.Current.Left + colliderRadius || base.transform.position.x > (float)Level.Current.Right - colliderRadius || base.transform.position.y < (float)Level.Current.Ground + colliderRadius || base.transform.position.y > (float)Level.Current.Ceiling - colliderRadius)
			{
				this.velocity *= -1f;
				this.state = DevilLevelPitchforkWheelProjectile.State.Returning;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001B65 RID: 7013 RVA: 0x000FAA6B File Offset: 0x000F8E6B
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002499 RID: 9369
	public DevilLevelPitchforkWheelProjectile.State state;

	// Token: 0x0400249A RID: 9370
	private float attackDelay;

	// Token: 0x0400249B RID: 9371
	private Vector2 velocity;

	// Token: 0x0400249C RID: 9372
	private float speed;

	// Token: 0x0400249D RID: 9373
	private DevilLevelSittingDevil parent;

	// Token: 0x02000595 RID: 1429
	public enum State
	{
		// Token: 0x0400249F RID: 9375
		Idle,
		// Token: 0x040024A0 RID: 9376
		Attacking,
		// Token: 0x040024A1 RID: 9377
		Returning
	}
}
