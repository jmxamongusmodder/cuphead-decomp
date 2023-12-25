using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AF6 RID: 2806
public class HomingProjectile : AbstractProjectile
{
	// Token: 0x17000615 RID: 1557
	// (get) Token: 0x06004404 RID: 17412 RVA: 0x000B6008 File Offset: 0x000B4408
	// (set) Token: 0x06004405 RID: 17413 RVA: 0x000B6010 File Offset: 0x000B4410
	public bool HomingEnabled { get; set; }

	// Token: 0x06004406 RID: 17414 RVA: 0x000B601C File Offset: 0x000B441C
	public HomingProjectile Create(Vector2 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float homingEaseTime, AbstractPlayerController player)
	{
		return this.Create(pos, launchRotation, launchSpeed, homingMoveSpeed, rotationSpeed, timeBeforeDeath, 0f, homingEaseTime, player);
	}

	// Token: 0x06004407 RID: 17415 RVA: 0x000B6044 File Offset: 0x000B4444
	public HomingProjectile Create(Vector2 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float timeBeforeHoming, float homingEaseTime, AbstractPlayerController player)
	{
		HomingProjectile homingProjectile = base.Create() as HomingProjectile;
		homingProjectile.homingDirection = MathUtils.AngleToDirection(launchRotation);
		homingProjectile.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed;
		homingProjectile.transform.position = pos;
		homingProjectile.player = player;
		homingProjectile.rotationSpeed = rotationSpeed;
		homingProjectile.homingMoveSpeed = homingMoveSpeed;
		homingProjectile.timeBeforeDeath = timeBeforeDeath;
		homingProjectile.timeBeforeHoming = timeBeforeHoming;
		homingProjectile.easeTime = homingEaseTime;
		homingProjectile.HomingEnabled = true;
		return homingProjectile;
	}

	// Token: 0x06004408 RID: 17416 RVA: 0x000B60C4 File Offset: 0x000B44C4
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06004409 RID: 17417 RVA: 0x000B60D9 File Offset: 0x000B44D9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600440A RID: 17418 RVA: 0x000B60F8 File Offset: 0x000B44F8
	private IEnumerator move_cr()
	{
		float t = 0f;
		while (t < this.timeBeforeDeath + this.easeTime + this.timeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
				if (this.trackGround)
				{
					center.y = (float)Level.Current.Ground;
				}
				Vector2 direction = (center - base.transform.position).normalized;
				Quaternion b = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(direction));
				Quaternion a = Quaternion.Euler(0f, 0f, MathUtils.DirectionToAngle(this.homingDirection));
				this.homingDirection = MathUtils.AngleToDirection(Quaternion.Slerp(a, b, Mathf.Min(1f, CupheadTime.FixedDelta * this.rotationSpeed)).eulerAngles.z);
			}
			Vector2 homingVelocity = this.homingDirection * this.homingMoveSpeed;
			this.velocity = homingVelocity;
			if (t < this.timeBeforeHoming)
			{
				this.velocity = this.launchVelocity;
			}
			else if (t < this.timeBeforeHoming + this.easeTime)
			{
				float t2 = EaseUtils.EaseOutSine(0f, 1f, (t - this.timeBeforeHoming) / this.easeTime);
				this.velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, t2);
			}
			if (this.faceMoveDirection)
			{
				base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + this.spriteRotation));
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.Die();
		yield break;
	}

	// Token: 0x0400499E RID: 18846
	private AbstractPlayerController player;

	// Token: 0x0400499F RID: 18847
	private Vector2 launchVelocity;

	// Token: 0x040049A0 RID: 18848
	private float homingMoveSpeed;

	// Token: 0x040049A1 RID: 18849
	private float rotationSpeed;

	// Token: 0x040049A2 RID: 18850
	private float timeBeforeDeath;

	// Token: 0x040049A3 RID: 18851
	private float timeBeforeHoming;

	// Token: 0x040049A4 RID: 18852
	private float easeTime;

	// Token: 0x040049A5 RID: 18853
	private Vector2 homingDirection;

	// Token: 0x040049A7 RID: 18855
	[SerializeField]
	private bool trackGround;

	// Token: 0x040049A8 RID: 18856
	[SerializeField]
	private bool faceMoveDirection;

	// Token: 0x040049A9 RID: 18857
	[SerializeField]
	private float spriteRotation;

	// Token: 0x040049AA RID: 18858
	protected Vector2 velocity;
}
