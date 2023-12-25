using System;
using UnityEngine;

// Token: 0x020006A3 RID: 1699
public class FlyingMermaidLevelTurtleSpiralProjectile : BasicProjectile
{
	// Token: 0x06002406 RID: 9222 RVA: 0x00152640 File Offset: 0x00150A40
	public virtual FlyingMermaidLevelTurtleSpiralProjectile Create(Vector2 position, float rotation, float speed, float rotationSpeed)
	{
		FlyingMermaidLevelTurtleSpiralProjectile flyingMermaidLevelTurtleSpiralProjectile = base.Create(position, rotation, speed) as FlyingMermaidLevelTurtleSpiralProjectile;
		flyingMermaidLevelTurtleSpiralProjectile.rotationSpeed = rotationSpeed;
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase = new GameObject("SpiralProjectileBase");
		flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform.position = position;
		flyingMermaidLevelTurtleSpiralProjectile.transform.parent = flyingMermaidLevelTurtleSpiralProjectile.rotationBase.transform;
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 0, UnityEngine.Random.Range(0f, 1f));
		flyingMermaidLevelTurtleSpiralProjectile.animator.Play("A", 1, UnityEngine.Random.Range(0f, 1f));
		return flyingMermaidLevelTurtleSpiralProjectile;
	}

	// Token: 0x06002407 RID: 9223 RVA: 0x001526E4 File Offset: 0x00150AE4
	protected override void Move()
	{
		if (this.Speed == 0f)
		{
		}
		base.transform.localPosition += this.rotationBase.transform.InverseTransformDirection(base.transform.right) * this.Speed * CupheadTime.FixedDelta;
		this.rotationBase.transform.AddEulerAngles(0f, 0f, this.rotationSpeed * 360f * CupheadTime.FixedDelta);
	}

	// Token: 0x06002408 RID: 9224 RVA: 0x00152773 File Offset: 0x00150B73
	protected override void Die()
	{
		base.Die();
	}

	// Token: 0x06002409 RID: 9225 RVA: 0x0015277B File Offset: 0x00150B7B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.rotationBase.gameObject);
	}

	// Token: 0x04002CD4 RID: 11476
	private float rotationSpeed;

	// Token: 0x04002CD5 RID: 11477
	private GameObject rotationBase;
}
