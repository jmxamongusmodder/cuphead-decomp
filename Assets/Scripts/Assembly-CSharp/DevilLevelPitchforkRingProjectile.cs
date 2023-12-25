using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000591 RID: 1425
public class DevilLevelPitchforkRingProjectile : AbstractProjectile
{
	// Token: 0x17000355 RID: 853
	// (get) Token: 0x06001B45 RID: 6981 RVA: 0x000FA1A9 File Offset: 0x000F85A9
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06001B46 RID: 6982 RVA: 0x000FA1AC File Offset: 0x000F85AC
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001B47 RID: 6983 RVA: 0x000FA1B4 File Offset: 0x000F85B4
	public DevilLevelPitchforkRingProjectile Create(Vector2 pos, float speed, float groundDuration, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkRingProjectile devilLevelPitchforkRingProjectile = this.InstantiatePrefab<DevilLevelPitchforkRingProjectile>();
		devilLevelPitchforkRingProjectile.transform.position = pos;
		devilLevelPitchforkRingProjectile.speed = speed;
		devilLevelPitchforkRingProjectile.state = DevilLevelPitchforkRingProjectile.State.Idle;
		devilLevelPitchforkRingProjectile.groundDuration = groundDuration;
		devilLevelPitchforkRingProjectile.parent = parent;
		devilLevelPitchforkRingProjectile.waitTime = waitTime;
		devilLevelPitchforkRingProjectile.StartCoroutine(devilLevelPitchforkRingProjectile.wait_cr());
		return devilLevelPitchforkRingProjectile;
	}

	// Token: 0x06001B48 RID: 6984 RVA: 0x000FA20C File Offset: 0x000F860C
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B49 RID: 6985 RVA: 0x000FA22B File Offset: 0x000F862B
	protected override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001B4A RID: 6986 RVA: 0x000FA23F File Offset: 0x000F863F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B4B RID: 6987 RVA: 0x000FA260 File Offset: 0x000F8660
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (!base.dead && this.state == DevilLevelPitchforkRingProjectile.State.Attacking)
		{
			if (!this.soundPlayed)
			{
				this.AttackSFX();
				this.soundPlayed = true;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			float radius = base.GetComponent<CircleCollider2D>().radius;
		}
	}

	// Token: 0x06001B4C RID: 6988 RVA: 0x000FA2EC File Offset: 0x000F86EC
	private IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x06001B4D RID: 6989 RVA: 0x000FA308 File Offset: 0x000F8708
	public void Attack()
	{
		if (!base.dead)
		{
			this.state = DevilLevelPitchforkRingProjectile.State.Attacking;
			this.velocity = this.speed * (PlayerManager.GetNext().center - base.transform.position).normalized;
			base.StartCoroutine(this.main_cr());
		}
	}

	// Token: 0x06001B4E RID: 6990 RVA: 0x000FA36C File Offset: 0x000F876C
	private IEnumerator main_cr()
	{
		while (this.state == DevilLevelPitchforkRingProjectile.State.Attacking)
		{
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.groundDuration);
		this.Die();
		yield break;
	}

	// Token: 0x06001B4F RID: 6991 RVA: 0x000FA387 File Offset: 0x000F8787
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001B50 RID: 6992 RVA: 0x000FA39A File Offset: 0x000F879A
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetBool("IsPink", parryable);
	}

	// Token: 0x06001B51 RID: 6993 RVA: 0x000FA3B4 File Offset: 0x000F87B4
	private void AttackSFX()
	{
		AudioManager.Play("devil_ring_projectile");
		this.emitAudioFromObject.Add("devil_ring_projectile");
	}

	// Token: 0x04002481 RID: 9345
	public DevilLevelPitchforkRingProjectile.State state;

	// Token: 0x04002482 RID: 9346
	private Vector2 velocity;

	// Token: 0x04002483 RID: 9347
	private float speed;

	// Token: 0x04002484 RID: 9348
	private float groundDuration;

	// Token: 0x04002485 RID: 9349
	private DevilLevelSittingDevil parent;

	// Token: 0x04002486 RID: 9350
	private float waitTime;

	// Token: 0x04002487 RID: 9351
	private bool waitTimeUp;

	// Token: 0x04002488 RID: 9352
	private bool soundPlayed;

	// Token: 0x02000592 RID: 1426
	public enum State
	{
		// Token: 0x0400248A RID: 9354
		Idle,
		// Token: 0x0400248B RID: 9355
		Attacking,
		// Token: 0x0400248C RID: 9356
		OnGround
	}
}
