using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200058C RID: 1420
public class DevilLevelPitchforkBouncingProjectile : AbstractProjectile
{
	// Token: 0x17000350 RID: 848
	// (get) Token: 0x06001B22 RID: 6946 RVA: 0x000F9334 File Offset: 0x000F7734
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x17000351 RID: 849
	// (get) Token: 0x06001B23 RID: 6947 RVA: 0x000F933B File Offset: 0x000F773B
	// (set) Token: 0x06001B24 RID: 6948 RVA: 0x000F9343 File Offset: 0x000F7743
	public int BouncesRemaining { get; private set; }

	// Token: 0x17000352 RID: 850
	// (get) Token: 0x06001B25 RID: 6949 RVA: 0x000F934C File Offset: 0x000F774C
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001B26 RID: 6950 RVA: 0x000F9350 File Offset: 0x000F7750
	public DevilLevelPitchforkBouncingProjectile Create(Vector2 pos, float attackDelay, float speed, float angle, int numBounces, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkBouncingProjectile devilLevelPitchforkBouncingProjectile = this.InstantiatePrefab<DevilLevelPitchforkBouncingProjectile>();
		devilLevelPitchforkBouncingProjectile.transform.position = pos;
		devilLevelPitchforkBouncingProjectile.attackDelay = attackDelay;
		devilLevelPitchforkBouncingProjectile.velocity = speed * MathUtils.AngleToDirection(angle);
		devilLevelPitchforkBouncingProjectile.BouncesRemaining = numBounces;
		devilLevelPitchforkBouncingProjectile.parent = parent;
		devilLevelPitchforkBouncingProjectile.state = DevilLevelPitchforkBouncingProjectile.State.Idle;
		devilLevelPitchforkBouncingProjectile.waitTime = waitTime;
		devilLevelPitchforkBouncingProjectile.StartCoroutine(devilLevelPitchforkBouncingProjectile.main_cr());
		devilLevelPitchforkBouncingProjectile.animator.SetFloat("Variation", (float)UnityEngine.Random.Range(0, 3) / 2f);
		return devilLevelPitchforkBouncingProjectile;
	}

	// Token: 0x06001B27 RID: 6951 RVA: 0x000F93DC File Offset: 0x000F77DC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (base.CanParry)
		{
			LevelPlayerParryController component = hit.GetComponent<LevelPlayerParryController>();
			if (component != null && component.State == LevelPlayerParryController.ParryState.Parrying)
			{
				return;
			}
		}
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B28 RID: 6952 RVA: 0x000F9430 File Offset: 0x000F7830
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B29 RID: 6953 RVA: 0x000F9450 File Offset: 0x000F7850
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (!base.dead && this.state != DevilLevelPitchforkBouncingProjectile.State.Idle)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			if (this.velocity == Vector2.zero)
			{
				this.bounceTime += CupheadTime.FixedDelta;
				if (this.bounceTime > 0.083333336f)
				{
					this.velocity = this.velocityOld;
				}
			}
			float radius = base.GetComponent<CircleCollider2D>().radius;
			if (this.BouncesRemaining > 0)
			{
				if ((this.velocity.x < 0f && base.transform.position.x < (float)Level.Current.Left + radius) || (this.velocity.x > 0f && base.transform.position.x > (float)Level.Current.Right - radius))
				{
					if (this.bounceTime == 0f)
					{
						base.animator.Play("BounceWall");
						this.BounceSFX();
						this.velocityOld = this.velocity;
						this.velocity = Vector2.zero;
					}
					else if (this.bounceTime > 0.083333336f)
					{
						this.BouncesRemaining--;
						this.velocity.x = this.velocity.x * -1f;
						this.bounceTime = 0f;
					}
				}
				if (this.velocity.y > 0f && base.transform.position.y > (float)Level.Current.Ceiling + radius)
				{
					if (this.bounceTime == 0f)
					{
						base.animator.Play("BounceGround");
						this.BounceSFX();
						this.velocityOld = this.velocity;
						this.velocity = Vector2.zero;
					}
					else if (this.bounceTime > 0.083333336f)
					{
						this.BouncesRemaining--;
						this.velocity.y = this.velocity.y * -1f;
						this.bounceTime = 0f;
					}
				}
			}
			if (this.velocity.y < 0f && base.transform.position.y < (float)Level.Current.Ground + radius)
			{
				if (this.bounceTime == 0f)
				{
					base.animator.Play("BounceGround");
					this.BounceSFX();
					this.velocityOld = this.velocity;
					this.velocity = Vector2.zero;
					if (base.CanParry)
					{
						this.bounceEffectPink.Create(base.transform.position);
					}
					else
					{
						this.bounceEffect.Create(base.transform.position);
					}
				}
				else if (this.bounceTime > 0.083333336f)
				{
					this.BouncesRemaining--;
					this.velocity.y = this.velocity.y * -1f;
					this.bounceTime = 0f;
				}
			}
		}
	}

	// Token: 0x06001B2A RID: 6954 RVA: 0x000F97C0 File Offset: 0x000F7BC0
	private IEnumerator main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		base.animator.SetTrigger("Continue");
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, this.attackDelay);
		this.state = DevilLevelPitchforkBouncingProjectile.State.Attacking;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			Effect selectedSparkle = (!base.CanParry) ? this.blueSparkle : this.pinkSparkle;
			Effect inst = selectedSparkle.Create(base.transform.position);
			SpriteRenderer r = inst.GetComponent<SpriteRenderer>();
			r.sortingLayerName = "Projectiles";
			r.sortingOrder = -1;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001B2B RID: 6955 RVA: 0x000F97DB File Offset: 0x000F7BDB
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B2C RID: 6956 RVA: 0x000F97EE File Offset: 0x000F7BEE
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("IsPink", parryable);
	}

	// Token: 0x06001B2D RID: 6957 RVA: 0x000F9808 File Offset: 0x000F7C08
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		this.BouncesRemaining = 0;
	}

	// Token: 0x06001B2E RID: 6958 RVA: 0x000F9818 File Offset: 0x000F7C18
	private void BounceSFX()
	{
		AudioManager.Play("devil_projectile_bounce");
		this.emitAudioFromObject.Add("devil_projectile_bounce");
	}

	// Token: 0x06001B2F RID: 6959 RVA: 0x000F9834 File Offset: 0x000F7C34
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.blueSparkle = null;
		this.pinkSparkle = null;
		this.bounceEffect = null;
		this.bounceEffectPink = null;
	}

	// Token: 0x0400245C RID: 9308
	private const string ProjectilesLayerName = "Projectiles";

	// Token: 0x0400245D RID: 9309
	private const int VariationMax = 3;

	// Token: 0x0400245E RID: 9310
	private const float BounceTimeThreshold = 0.083333336f;

	// Token: 0x0400245F RID: 9311
	public DevilLevelPitchforkBouncingProjectile.State state;

	// Token: 0x04002461 RID: 9313
	private float attackDelay;

	// Token: 0x04002462 RID: 9314
	private Vector2 velocity;

	// Token: 0x04002463 RID: 9315
	private Vector2 velocityOld;

	// Token: 0x04002464 RID: 9316
	private DevilLevelSittingDevil parent;

	// Token: 0x04002465 RID: 9317
	private float waitTime;

	// Token: 0x04002466 RID: 9318
	private bool waitTimeUp;

	// Token: 0x04002467 RID: 9319
	private float bounceTime;

	// Token: 0x04002468 RID: 9320
	[SerializeField]
	private Effect blueSparkle;

	// Token: 0x04002469 RID: 9321
	[SerializeField]
	private Effect pinkSparkle;

	// Token: 0x0400246A RID: 9322
	[SerializeField]
	private Effect bounceEffect;

	// Token: 0x0400246B RID: 9323
	[SerializeField]
	private Effect bounceEffectPink;

	// Token: 0x0200058D RID: 1421
	public enum State
	{
		// Token: 0x0400246D RID: 9325
		Idle,
		// Token: 0x0400246E RID: 9326
		Attacking
	}
}
