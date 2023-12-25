using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000550 RID: 1360
public class ChessRookLevelHazard : AbstractProjectile
{
	// Token: 0x06001941 RID: 6465 RVA: 0x000E5260 File Offset: 0x000E3660
	public ChessRookLevelHazard Create(Vector3 position, float speed)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.speed = speed;
		this.Move();
		return this;
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x000E5288 File Offset: 0x000E3688
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x000E52A6 File Offset: 0x000E36A6
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x000E52B8 File Offset: 0x000E36B8
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > -740f)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<ChessRookLevelHazard>();
		yield break;
	}

	// Token: 0x0400225F RID: 8799
	private float speed;
}
