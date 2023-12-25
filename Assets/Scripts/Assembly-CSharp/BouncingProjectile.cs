using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000AEB RID: 2795
public class BouncingProjectile : AbstractProjectile
{
	// Token: 0x060043B4 RID: 17332 RVA: 0x0023FFF2 File Offset: 0x0023E3F2
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060043B5 RID: 17333 RVA: 0x00240008 File Offset: 0x0023E408
	private IEnumerator move_cr()
	{
		for (;;)
		{
			if (this.isMoving)
			{
				base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060043B6 RID: 17334 RVA: 0x00240024 File Offset: 0x0023E424
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Min(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x060043B7 RID: 17335 RVA: 0x0024005C File Offset: 0x0023E45C
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		Vector3 newVelocity = this.velocity;
		newVelocity.y = Mathf.Max(newVelocity.y, -newVelocity.y);
		this.ChangeDir(newVelocity);
	}

	// Token: 0x060043B8 RID: 17336 RVA: 0x00240094 File Offset: 0x0023E494
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
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

	// Token: 0x060043B9 RID: 17337 RVA: 0x00240110 File Offset: 0x0023E510
	protected virtual void ChangeDir(Vector3 newVelocity)
	{
		this.velocity = newVelocity;
		this.currentAngle = Mathf.Atan2(this.velocity.y, this.velocity.x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.currentAngle));
	}

	// Token: 0x04004968 RID: 18792
	public bool isMoving;

	// Token: 0x04004969 RID: 18793
	protected float speed;

	// Token: 0x0400496A RID: 18794
	protected float currentAngle;

	// Token: 0x0400496B RID: 18795
	protected Vector3 velocity;
}
