using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000593 RID: 1427
public class DevilLevelPitchforkSpinnerProjectile : AbstractProjectile
{
	// Token: 0x17000357 RID: 855
	// (get) Token: 0x06001B53 RID: 6995 RVA: 0x000FA57B File Offset: 0x000F897B
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001B54 RID: 6996 RVA: 0x000FA584 File Offset: 0x000F8984
	public DevilLevelPitchforkSpinnerProjectile Create(Vector2 pos, float maxSpeed, float acceleration, float homingDuration, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkSpinnerProjectile devilLevelPitchforkSpinnerProjectile = this.InstantiatePrefab<DevilLevelPitchforkSpinnerProjectile>();
		devilLevelPitchforkSpinnerProjectile.transform.position = pos;
		devilLevelPitchforkSpinnerProjectile.homingDuration = homingDuration;
		devilLevelPitchforkSpinnerProjectile.startingY = pos.y;
		devilLevelPitchforkSpinnerProjectile.parent = parent;
		devilLevelPitchforkSpinnerProjectile.waitTime = waitTime;
		devilLevelPitchforkSpinnerProjectile.homingMaxSpeed = maxSpeed;
		devilLevelPitchforkSpinnerProjectile.homingAcceleration = acceleration;
		devilLevelPitchforkSpinnerProjectile.StartCoroutine(devilLevelPitchforkSpinnerProjectile.main_cr());
		devilLevelPitchforkSpinnerProjectile.SetParryable(true);
		devilLevelPitchforkSpinnerProjectile.animator.SetBool("IsPink", true);
		devilLevelPitchforkSpinnerProjectile.OrbitStartSFX();
		return devilLevelPitchforkSpinnerProjectile;
	}

	// Token: 0x06001B55 RID: 6997 RVA: 0x000FA608 File Offset: 0x000F8A08
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B56 RID: 6998 RVA: 0x000FA627 File Offset: 0x000F8A27
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B57 RID: 6999 RVA: 0x000FA648 File Offset: 0x000F8A48
	protected override void FixedUpdate()
	{
		if (!this.waitTimeUp)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		base.transform.SetPosition(null, new float?(this.startingY + Mathf.Sin(this.t * 3.1415927f * 2f / 1.5f) * 10f), null);
		if (Mathf.Abs(base.transform.position.x) > 1500f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.Update();
	}

	// Token: 0x06001B58 RID: 7000 RVA: 0x000FA6F4 File Offset: 0x000F8AF4
	private IEnumerator main_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<GroundHomingMovement>().EnableHoming = false;
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		this.waitTimeUp = true;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("StartAtHalf", Rand.Bool());
		GroundHomingMovement homingMovement = base.GetComponent<GroundHomingMovement>();
		homingMovement.maxSpeed = this.homingMaxSpeed;
		homingMovement.acceleration = this.homingAcceleration;
		homingMovement.bounceEnabled = false;
		homingMovement.destroyOffScreen = false;
		homingMovement.TrackingPlayer = PlayerManager.GetNext();
		homingMovement.EnableHoming = false;
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<GroundHomingMovement>().EnableHoming = true;
		yield return CupheadTime.WaitForSeconds(this, this.homingDuration);
		base.GetComponent<GroundHomingMovement>().EnableHoming = false;
		yield break;
	}

	// Token: 0x06001B59 RID: 7001 RVA: 0x000FA70F File Offset: 0x000F8B0F
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
		this.OrbitStopSFX();
	}

	// Token: 0x06001B5A RID: 7002 RVA: 0x000FA728 File Offset: 0x000F8B28
	public override void OnParry(AbstractPlayerController player)
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x06001B5B RID: 7003 RVA: 0x000FA742 File Offset: 0x000F8B42
	private void OrbitStartSFX()
	{
		if (!this.SpinSFXPlaying)
		{
			AudioManager.PlayLoop("devil_orbit_projectile");
			this.emitAudioFromObject.Add("devil_orbit_projectile");
			this.SpinSFXPlaying = true;
		}
	}

	// Token: 0x06001B5C RID: 7004 RVA: 0x000FA770 File Offset: 0x000F8B70
	private void OrbitStopSFX()
	{
		AudioManager.Stop("devil_orbit_projectile");
		this.SpinSFXPlaying = false;
	}

	// Token: 0x0400248D RID: 9357
	private const float SIN_HEIGHT = 10f;

	// Token: 0x0400248E RID: 9358
	private const float SIN_PERIOD = 1.5f;

	// Token: 0x0400248F RID: 9359
	private const float DESTROY_X = 1500f;

	// Token: 0x04002490 RID: 9360
	private float waitTime;

	// Token: 0x04002491 RID: 9361
	private float homingDuration;

	// Token: 0x04002492 RID: 9362
	private float homingMaxSpeed;

	// Token: 0x04002493 RID: 9363
	private float homingAcceleration;

	// Token: 0x04002494 RID: 9364
	private float startingY;

	// Token: 0x04002495 RID: 9365
	private float t;

	// Token: 0x04002496 RID: 9366
	private bool waitTimeUp;

	// Token: 0x04002497 RID: 9367
	private bool SpinSFXPlaying;

	// Token: 0x04002498 RID: 9368
	private DevilLevelSittingDevil parent;
}
