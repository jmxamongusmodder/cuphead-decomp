using System;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class ChessQueenLevelEgg : AbstractProjectile
{
	// Token: 0x1700033D RID: 829
	// (get) Token: 0x060018FE RID: 6398 RVA: 0x000E29BE File Offset: 0x000E0DBE
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x000E29C8 File Offset: 0x000E0DC8
	public ChessQueenLevelEgg Create(Vector3 position, Vector3 velocity, float gravity, float delay)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.velocity = velocity;
		this.gravity = gravity;
		this.delay = delay;
		this.coll.enabled = false;
		this.rend.flipX = Rand.Bool();
		this.anim.Play(UnityEngine.Random.Range(0, 12).ToString());
		return this;
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x000E2A46 File Offset: 0x000E0E46
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x000E2A64 File Offset: 0x000E0E64
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isDead)
		{
			return;
		}
		if (base.lifetime > this.delay)
		{
			this.rend.sortingLayerName = "Projectiles";
			this.rend.sortingOrder = 0;
			this.rend.color = Color.white;
			this.coll.enabled = true;
		}
		else
		{
			this.rend.color = Color.Lerp(new Color(0.7f, 0.7f, 0.7f, 1f), Color.white, base.lifetime / this.delay);
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		if (base.transform.position.y < (float)Level.Current.Ground + 15f)
		{
			this.HitGround();
		}
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x000E2B8C File Offset: 0x000E0F8C
	protected void HitGround()
	{
		this.StopAllCoroutines();
		base.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground + 15f);
		this.isDead = true;
		this.coll.enabled = false;
		this.explosionRend.flipX = Rand.Bool();
		this.anim.Play((!Rand.Bool()) ? "ExplodeB" : "ExplodeA", 1, 0f);
		this.anim.Update(0f);
	}

	// Token: 0x04002215 RID: 8725
	private const float GROUND_OFFSET = 15f;

	// Token: 0x04002216 RID: 8726
	private Vector2 velocity;

	// Token: 0x04002217 RID: 8727
	private float gravity;

	// Token: 0x04002218 RID: 8728
	private bool isDead;

	// Token: 0x04002219 RID: 8729
	private float delay;

	// Token: 0x0400221A RID: 8730
	[SerializeField]
	private Animator anim;

	// Token: 0x0400221B RID: 8731
	[SerializeField]
	private Collider2D coll;

	// Token: 0x0400221C RID: 8732
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x0400221D RID: 8733
	[SerializeField]
	private SpriteRenderer explosionRend;
}
