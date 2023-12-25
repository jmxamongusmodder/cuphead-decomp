using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200052A RID: 1322
public class ChessBOldALevelWall : AbstractProjectile
{
	// Token: 0x1700032E RID: 814
	// (get) Token: 0x060017DC RID: 6108 RVA: 0x000D7905 File Offset: 0x000D5D05
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060017DD RID: 6109 RVA: 0x000D790C File Offset: 0x000D5D0C
	public void StartRotate(float angle, ChessBOldALevelBishop parent, float loopSize, float speed, bool isClockwise, float scale)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.angle = angle;
		this.parent = parent;
		this.speed = speed;
		this.loopSize = loopSize;
		this.isClockwise = isClockwise;
		base.transform.SetScale(new float?(scale), null, null);
		base.StartCoroutine(this.move_wall_cr());
	}

	// Token: 0x060017DE RID: 6110 RVA: 0x000D797B File Offset: 0x000D5D7B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060017DF RID: 6111 RVA: 0x000D799C File Offset: 0x000D5D9C
	private IEnumerator move_wall_cr()
	{
		Vector3 handleRotation = Vector3.zero;
		if (this.angle == 0f || this.angle == 180f)
		{
			base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.loopSize;
		}
		else
		{
			base.transform.position = this.parent.transform.position + MathUtils.AngleToDirection(this.angle) * this.loopSize;
		}
		this.angle *= 0.017453292f;
		for (;;)
		{
			if (this.isClockwise)
			{
				this.angle += this.speed * CupheadTime.FixedDelta;
			}
			else
			{
				this.angle -= this.speed * CupheadTime.FixedDelta;
			}
			handleRotation = new Vector3(Mathf.Sin(this.angle) * this.loopSize, Mathf.Cos(this.angle) * this.loopSize, 0f);
			base.transform.position = this.parent.transform.position;
			base.transform.position += handleRotation;
			Vector3 diff = this.parent.transform.position - base.transform.position;
			diff.Normalize();
			base.transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(diff.y, diff.x) * 57.29578f + 90f);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x060017E0 RID: 6112 RVA: 0x000D79B7 File Offset: 0x000D5DB7
	public void Dead()
	{
		this.Recycle<ChessBOldALevelWall>();
	}

	// Token: 0x04002109 RID: 8457
	private ChessBOldALevelBishop parent;

	// Token: 0x0400210A RID: 8458
	private bool isClockwise;

	// Token: 0x0400210B RID: 8459
	private float angle;

	// Token: 0x0400210C RID: 8460
	private float loopSize;

	// Token: 0x0400210D RID: 8461
	private float speed;
}
