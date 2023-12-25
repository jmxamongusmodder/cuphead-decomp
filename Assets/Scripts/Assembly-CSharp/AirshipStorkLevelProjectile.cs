using System;
using UnityEngine;

// Token: 0x020004DA RID: 1242
public class AirshipStorkLevelProjectile : BasicProjectile
{
	// Token: 0x06001539 RID: 5433 RVA: 0x000BE408 File Offset: 0x000BC808
	public virtual BasicProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed, int direction)
	{
		AirshipStorkLevelProjectile airshipStorkLevelProjectile = base.Create(position, rotation, speed) as AirshipStorkLevelProjectile;
		airshipStorkLevelProjectile.rotationSpeed = rotationSpeed;
		airshipStorkLevelProjectile.rotationBase = new GameObject("SpiralProjectileBase").transform;
		airshipStorkLevelProjectile.rotationBase.position = position;
		airshipStorkLevelProjectile.transform.parent = airshipStorkLevelProjectile.rotationBase;
		airshipStorkLevelProjectile.direction = direction;
		return airshipStorkLevelProjectile;
	}

	// Token: 0x0600153A RID: 5434 RVA: 0x000BE46C File Offset: 0x000BC86C
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
		if (base.transform.position.y < -360f || base.transform.position.y > 360f)
		{
			this.Die();
		}
	}

	// Token: 0x0600153B RID: 5435 RVA: 0x000BE55C File Offset: 0x000BC95C
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04001E97 RID: 7831
	private int direction;

	// Token: 0x04001E98 RID: 7832
	private float rotationSpeed;

	// Token: 0x04001E99 RID: 7833
	private Transform rotationBase;
}
