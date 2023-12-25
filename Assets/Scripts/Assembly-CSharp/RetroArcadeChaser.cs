using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000743 RID: 1859
public class RetroArcadeChaser : RetroArcadeEnemy
{
	// Token: 0x170003D9 RID: 985
	// (get) Token: 0x0600287D RID: 10365 RVA: 0x00179635 File Offset: 0x00177A35
	// (set) Token: 0x0600287E RID: 10366 RVA: 0x0017963D File Offset: 0x00177A3D
	public bool IsDone { get; private set; }

	// Token: 0x170003DA RID: 986
	// (get) Token: 0x0600287F RID: 10367 RVA: 0x00179646 File Offset: 0x00177A46
	// (set) Token: 0x06002880 RID: 10368 RVA: 0x0017964E File Offset: 0x00177A4E
	public bool HomingEnabled { get; set; }

	// Token: 0x06002881 RID: 10369 RVA: 0x00179658 File Offset: 0x00177A58
	public virtual RetroArcadeChaser Init(Vector3 pos, float launchRotation, float launchSpeed, float homingMoveSpeed, float rotationSpeed, float timeBeforeDeath, float hp, AbstractPlayerController player, LevelProperties.RetroArcade.Chasers properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.homingDirection = MathUtils.AngleToDirection(launchRotation);
		this.launchVelocity = MathUtils.AngleToDirection(launchRotation) * launchSpeed;
		base.transform.position = pos;
		this.player = player;
		this.rotationSpeed = rotationSpeed;
		this.homingMoveSpeed = homingMoveSpeed;
		this.timeBeforeDeath = timeBeforeDeath;
		this.HomingEnabled = true;
		this.hp = hp;
		this.StartChaser();
		return this;
	}

	// Token: 0x06002882 RID: 10370 RVA: 0x001796DD File Offset: 0x00177ADD
	private void StartChaser()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002883 RID: 10371 RVA: 0x001796EC File Offset: 0x00177AEC
	private IEnumerator move_cr()
	{
		float t = 0f;
		while (t < this.timeBeforeDeath + this.timeBeforeHoming)
		{
			while (!this.HomingEnabled)
			{
				yield return null;
			}
			t += CupheadTime.FixedDelta;
			if (this.player != null && !this.player.IsDead)
			{
				Vector3 center = this.player.center;
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
			else if (t < this.timeBeforeHoming)
			{
				float t2 = EaseUtils.EaseOutSine(0f, 1f, t - this.timeBeforeHoming);
				this.velocity = Vector2.Lerp(this.launchVelocity, homingVelocity, t2);
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		float offset = 100f;
		while (base.transform.position.x > -640f - offset && base.transform.position.x < 640f + offset && base.transform.position.x > -360f - offset && base.transform.position.x < 360f + offset)
		{
			base.transform.position += this.velocity.normalized * this.homingMoveSpeed * CupheadTime.FixedDelta;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + 180f));
			yield return new WaitForFixedUpdate();
		}
		this.IsDone = true;
		this.Recycle<RetroArcadeChaser>();
		yield break;
	}

	// Token: 0x04003151 RID: 12625
	private AbstractPlayerController player;

	// Token: 0x04003152 RID: 12626
	private Vector2 launchVelocity;

	// Token: 0x04003153 RID: 12627
	private float homingMoveSpeed;

	// Token: 0x04003154 RID: 12628
	private float rotationSpeed;

	// Token: 0x04003155 RID: 12629
	private float timeBeforeDeath;

	// Token: 0x04003156 RID: 12630
	private float timeBeforeHoming;

	// Token: 0x04003157 RID: 12631
	private Vector2 homingDirection;

	// Token: 0x04003159 RID: 12633
	protected Vector2 velocity;
}
