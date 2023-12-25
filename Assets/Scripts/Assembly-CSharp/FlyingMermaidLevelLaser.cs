using System;
using UnityEngine;

// Token: 0x0200068C RID: 1676
public class FlyingMermaidLevelLaser : AbstractCollidableObject
{
	// Token: 0x06002354 RID: 9044 RVA: 0x0014BDDC File Offset: 0x0014A1DC
	public void SetStoneTime(float stoneTime)
	{
		this.stoneTime = stoneTime;
	}

	// Token: 0x06002355 RID: 9045 RVA: 0x0014BDE5 File Offset: 0x0014A1E5
	public void StartLaser()
	{
		if (base.GetComponent<Collider2D>())
		{
			this.checkCollider = true;
		}
	}

	// Token: 0x06002356 RID: 9046 RVA: 0x0014BDFE File Offset: 0x0014A1FE
	public void StopLaser()
	{
		if (base.GetComponent<Collider2D>())
		{
			this.checkCollider = false;
		}
	}

	// Token: 0x06002357 RID: 9047 RVA: 0x0014BE17 File Offset: 0x0014A217
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.checkCollider)
		{
			hit.GetComponent<PlanePlayerController>().GetStoned(this.stoneTime);
		}
	}

	// Token: 0x04002BF4 RID: 11252
	private float stoneTime = 5f;

	// Token: 0x04002BF5 RID: 11253
	private bool checkCollider;
}
