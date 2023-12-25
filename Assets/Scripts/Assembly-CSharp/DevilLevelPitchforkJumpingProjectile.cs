using System;
using UnityEngine;

// Token: 0x0200058E RID: 1422
public class DevilLevelPitchforkJumpingProjectile : AbstractProjectile
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06001B31 RID: 6961 RVA: 0x000F9A4B File Offset: 0x000F7E4B
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001B32 RID: 6962 RVA: 0x000F9A54 File Offset: 0x000F7E54
	public DevilLevelPitchforkJumpingProjectile Create(Vector2 pos, MinMax launchAngle, MinMax launchSpeed, float gravity, int numJumps, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkJumpingProjectile devilLevelPitchforkJumpingProjectile = this.InstantiatePrefab<DevilLevelPitchforkJumpingProjectile>();
		devilLevelPitchforkJumpingProjectile.transform.position = pos;
		devilLevelPitchforkJumpingProjectile.launchSpeed = launchSpeed;
		devilLevelPitchforkJumpingProjectile.launchAngle = launchAngle;
		devilLevelPitchforkJumpingProjectile.gravity = gravity;
		devilLevelPitchforkJumpingProjectile.parent = parent;
		devilLevelPitchforkJumpingProjectile.jumpsRemaining = numJumps;
		return devilLevelPitchforkJumpingProjectile;
	}

	// Token: 0x06001B33 RID: 6963 RVA: 0x000F9AA0 File Offset: 0x000F7EA0
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B34 RID: 6964 RVA: 0x000F9ABF File Offset: 0x000F7EBF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B35 RID: 6965 RVA: 0x000F9AE0 File Offset: 0x000F7EE0
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!base.dead && this.state == DevilLevelPitchforkJumpingProjectile.State.Jumping)
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			float radius = base.GetComponent<CircleCollider2D>().radius;
			if (base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				base.transform.SetPosition(null, new float?((float)Level.Current.Ground + radius), null);
				if (this.jumpsRemaining > 0)
				{
					this.state = DevilLevelPitchforkJumpingProjectile.State.OnGround;
				}
				else
				{
					this.Die();
				}
			}
		}
	}

	// Token: 0x06001B36 RID: 6966 RVA: 0x000F9BD8 File Offset: 0x000F7FD8
	public void Jump()
	{
		float num = float.MaxValue;
		Vector2 vector = Vector2.zero;
		Vector3 center = PlayerManager.GetNext().center;
		Vector2 vector2 = center - base.transform.position;
		vector2.x = Mathf.Abs(vector2.x);
		float radius = base.GetComponent<CircleCollider2D>().radius;
		AudioManager.Play("devil_projectile_move");
		this.emitAudioFromObject.Add("devil_projectile_move");
		float num2;
		if (center.x < base.transform.position.x)
		{
			num2 = base.transform.position.x - ((float)Level.Current.Left + radius);
		}
		else
		{
			num2 = (float)Level.Current.Right - radius - base.transform.position.x;
		}
		for (float num3 = 0f; num3 < 1f; num3 += 0.01f)
		{
			float floatAt = this.launchAngle.GetFloatAt(num3);
			float floatAt2 = this.launchSpeed.GetFloatAt(num3);
			Vector2 vector3 = MathUtils.AngleToDirection(floatAt) * floatAt2;
			float num4 = vector2.x / vector3.x;
			float num5 = vector3.y * num4 - 0.5f * this.gravity * num4 * num4;
			float num6 = Mathf.Abs(vector2.y - num5);
			float num7 = vector3.y - this.gravity * num4;
			if (num7 <= 0f)
			{
				float num8 = num2 / vector3.x;
				float num9 = vector3.y * num8 - 0.5f * this.gravity * num8 * num8;
				if (num9 <= (float)Level.Current.Ground + radius)
				{
					if (num6 < num)
					{
						num = num6;
						vector = vector3;
					}
				}
			}
		}
		if (center.x < base.transform.position.x)
		{
			vector.x *= -1f;
		}
		this.velocity = vector;
		this.state = DevilLevelPitchforkJumpingProjectile.State.Jumping;
		this.jumpsRemaining--;
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x000F9E1A File Offset: 0x000F821A
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400246F RID: 9327
	public DevilLevelPitchforkJumpingProjectile.State state;

	// Token: 0x04002470 RID: 9328
	private Vector2 velocity;

	// Token: 0x04002471 RID: 9329
	private MinMax launchSpeed;

	// Token: 0x04002472 RID: 9330
	private MinMax launchAngle;

	// Token: 0x04002473 RID: 9331
	private float gravity;

	// Token: 0x04002474 RID: 9332
	private int jumpsRemaining;

	// Token: 0x04002475 RID: 9333
	private DevilLevelSittingDevil parent;

	// Token: 0x0200058F RID: 1423
	public enum State
	{
		// Token: 0x04002477 RID: 9335
		Idle,
		// Token: 0x04002478 RID: 9336
		Jumping,
		// Token: 0x04002479 RID: 9337
		OnGround
	}
}
