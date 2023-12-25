using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000590 RID: 1424
public class DevilLevelPitchforkOrbitingProjectile : AbstractProjectile
{
	// Token: 0x17000354 RID: 852
	// (get) Token: 0x06001B39 RID: 6969 RVA: 0x000F9E35 File Offset: 0x000F8235
	protected override float DestroyLifetime
	{
		get
		{
			return -1f;
		}
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x000F9E3C File Offset: 0x000F823C
	public DevilLevelPitchforkOrbitingProjectile Create(AbstractProjectile target, float angle, float rotationSpeed, float radius, DevilLevelSittingDevil parent, float waitTime)
	{
		DevilLevelPitchforkOrbitingProjectile devilLevelPitchforkOrbitingProjectile = this.InstantiatePrefab<DevilLevelPitchforkOrbitingProjectile>();
		devilLevelPitchforkOrbitingProjectile.target = target;
		devilLevelPitchforkOrbitingProjectile.angle = angle;
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed;
		devilLevelPitchforkOrbitingProjectile.radius = radius;
		devilLevelPitchforkOrbitingProjectile.parent = parent;
		devilLevelPitchforkOrbitingProjectile.waitTime = waitTime;
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = false;
		return devilLevelPitchforkOrbitingProjectile;
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x000F9E88 File Offset: 0x000F8288
	public DevilLevelPitchforkOrbitingProjectile Create(AbstractProjectile target, float angle, float rotationSpeed, float radius, DevilLevelSittingDevil parent)
	{
		DevilLevelPitchforkOrbitingProjectile devilLevelPitchforkOrbitingProjectile = this.InstantiatePrefab<DevilLevelPitchforkOrbitingProjectile>();
		devilLevelPitchforkOrbitingProjectile.target = target;
		devilLevelPitchforkOrbitingProjectile.angle = angle;
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed;
		devilLevelPitchforkOrbitingProjectile.radius = radius;
		devilLevelPitchforkOrbitingProjectile.parent = parent;
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = true;
		return devilLevelPitchforkOrbitingProjectile;
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x000F9EC9 File Offset: 0x000F82C9
	protected override void Update()
	{
		base.Update();
		if (this.parent == null)
		{
			this.Die();
		}
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x000F9EE8 File Offset: 0x000F82E8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000F9F08 File Offset: 0x000F8308
	private IEnumerator wait_time_cr()
	{
		yield return new WaitForSeconds(this.waitTime);
		this.waitTimeUp = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.animator.SetTrigger("Continue");
		base.animator.SetBool("StartAtHalf", Rand.Bool());
		yield break;
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000F9F24 File Offset: 0x000F8324
	protected override void Start()
	{
		base.Start();
		Vector2 vector = this.target.transform.position + MathUtils.AngleToDirection(this.angle) * this.radius;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
		if (!this.waitTimeUp)
		{
			base.GetComponent<Collider2D>().enabled = false;
			base.StartCoroutine(this.wait_time_cr());
		}
	}

	// Token: 0x06001B40 RID: 6976 RVA: 0x000F9FB8 File Offset: 0x000F83B8
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.waitTimeUp)
		{
			return;
		}
		if (base.dead)
		{
			return;
		}
		if (this.target == null || this.target.dead)
		{
			this.Die();
			return;
		}
		this.angle += this.rotationSpeed * CupheadTime.FixedDelta;
		Vector2 vector = this.target.transform.position + MathUtils.AngleToDirection(this.angle) * this.radius;
		base.transform.SetPosition(new float?(vector.x), new float?(vector.y), null);
	}

	// Token: 0x06001B41 RID: 6977 RVA: 0x000FA081 File Offset: 0x000F8481
	protected override void Die()
	{
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
		this.OrbitStopSFX();
	}

	// Token: 0x06001B42 RID: 6978 RVA: 0x000FA09A File Offset: 0x000F849A
	private void OrbitStartSFX()
	{
		AudioManager.PlayLoop("devil_orbit_projectile");
		this.emitAudioFromObject.Add("devil_orbit_projectile");
	}

	// Token: 0x06001B43 RID: 6979 RVA: 0x000FA0B6 File Offset: 0x000F84B6
	private void OrbitStopSFX()
	{
		AudioManager.Stop("devil_orbit_projectile");
	}

	// Token: 0x0400247A RID: 9338
	private AbstractProjectile target;

	// Token: 0x0400247B RID: 9339
	private float rotationSpeed;

	// Token: 0x0400247C RID: 9340
	private float radius;

	// Token: 0x0400247D RID: 9341
	private float angle;

	// Token: 0x0400247E RID: 9342
	private float waitTime;

	// Token: 0x0400247F RID: 9343
	private DevilLevelSittingDevil parent;

	// Token: 0x04002480 RID: 9344
	private bool waitTimeUp;
}
