using System;
using UnityEngine;

// Token: 0x020005CB RID: 1483
public class DicePalaceFlyingMemoryLevelSpiralProjectile : BasicProjectile
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x0010A8A0 File Offset: 0x00108CA0
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed, int direction)
	{
		DicePalaceFlyingMemoryLevelSpiralProjectile dicePalaceFlyingMemoryLevelSpiralProjectile = base.Create(position, rotation, speed) as DicePalaceFlyingMemoryLevelSpiralProjectile;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationSpeed = rotationSpeed;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase = new GameObject("SpiralProjectileBase").transform;
		dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase.position = position;
		dicePalaceFlyingMemoryLevelSpiralProjectile.transform.parent = dicePalaceFlyingMemoryLevelSpiralProjectile.rotationBase;
		dicePalaceFlyingMemoryLevelSpiralProjectile.direction = direction;
		return dicePalaceFlyingMemoryLevelSpiralProjectile;
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x0010A904 File Offset: 0x00108D04
	protected override void Move()
	{
		float num = 360f;
		if (this.direction == 1)
		{
			num = -360f;
		}
		else if (this.direction == 2)
		{
			num = 360f;
		}
		if (this.Speed == 0f)
		{
		}
		base.transform.localPosition += this.rotationBase.InverseTransformDirection(base.transform.right) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.AddEulerAngles(0f, 0f, this.rotationSpeed * num * CupheadTime.FixedDelta);
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x0010A9B4 File Offset: 0x00108DB4
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04002600 RID: 9728
	private int direction;

	// Token: 0x04002601 RID: 9729
	private float rotationSpeed;

	// Token: 0x04002602 RID: 9730
	private Transform rotationBase;
}
