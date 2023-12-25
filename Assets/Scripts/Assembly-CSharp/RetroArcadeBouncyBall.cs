using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200073A RID: 1850
public class RetroArcadeBouncyBall : RetroArcadeEnemy
{
	// Token: 0x0600284B RID: 10315 RVA: 0x00177E94 File Offset: 0x00176294
	public RetroArcadeBouncyBall Create(Vector3 pos, RetroArcadeBouncyManager manager, LevelProperties.RetroArcade.Bouncy properties, float startAngle)
	{
		RetroArcadeBouncyBall retroArcadeBouncyBall = this.InstantiatePrefab<RetroArcadeBouncyBall>();
		retroArcadeBouncyBall.transform.position = pos;
		retroArcadeBouncyBall.startAngle = startAngle;
		retroArcadeBouncyBall.properties = properties;
		retroArcadeBouncyBall.GetComponent<Collider2D>().enabled = false;
		return retroArcadeBouncyBall;
	}

	// Token: 0x0600284C RID: 10316 RVA: 0x00177ED0 File Offset: 0x001762D0
	public void StartMoving(Vector3 middlePos)
	{
		this.hp = 1f;
		base.transform.parent = null;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600284D RID: 10317 RVA: 0x00177F04 File Offset: 0x00176304
	private IEnumerator move_cr()
	{
		this.velocity = MathUtils.AngleToDirection(this.startAngle);
		for (;;)
		{
			base.transform.position += this.velocity * this.properties.groupMoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600284E RID: 10318 RVA: 0x00177F20 File Offset: 0x00176320
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x0600284F RID: 10319 RVA: 0x00177F60 File Offset: 0x00176360
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x06002850 RID: 10320 RVA: 0x00177FA0 File Offset: 0x001763A0
	protected void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
	}

	// Token: 0x06002851 RID: 10321 RVA: 0x00178008 File Offset: 0x00176408
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		Vector3 newVelocity = this.velocity;
		if (base.transform.position.x > 0f)
		{
			newVelocity.x = Mathf.Min(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
		else
		{
			newVelocity.x = Mathf.Max(newVelocity.x, -newVelocity.x);
			this.ChangeDir(newVelocity);
		}
	}

	// Token: 0x06002852 RID: 10322 RVA: 0x0017808A File Offset: 0x0017648A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (!base.IsDead)
		{
			base.OnCollisionPlayer(hit, phase);
		}
	}

	// Token: 0x06002853 RID: 10323 RVA: 0x0017809F File Offset: 0x0017649F
	public override void Dead()
	{
		base.Dead();
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<DamageReceiver>().enabled = false;
		UnityEngine.Object.Destroy(base.GetComponent<Rigidbody2D>());
	}

	// Token: 0x04003114 RID: 12564
	private LevelProperties.RetroArcade.Bouncy properties;

	// Token: 0x04003115 RID: 12565
	private RetroArcadeBouncyManager manager;

	// Token: 0x04003116 RID: 12566
	private Vector3 velocity;

	// Token: 0x04003117 RID: 12567
	private float currentAngle;

	// Token: 0x04003118 RID: 12568
	private float startAngle;
}
