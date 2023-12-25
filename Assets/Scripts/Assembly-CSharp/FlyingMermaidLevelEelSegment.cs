using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000686 RID: 1670
public class FlyingMermaidLevelEelSegment : AbstractPausableComponent
{
	// Token: 0x06002337 RID: 9015 RVA: 0x0014ADE4 File Offset: 0x001491E4
	public FlyingMermaidLevelEelSegment Create(Vector2 position, string sortingLayer, int sortingOrder)
	{
		FlyingMermaidLevelEelSegment flyingMermaidLevelEelSegment = UnityEngine.Object.Instantiate<FlyingMermaidLevelEelSegment>(this);
		flyingMermaidLevelEelSegment.transform.position = position;
		flyingMermaidLevelEelSegment.velocity = this.launchSpeed * MathUtils.AngleToDirection(this.angleRange.RandomFloat());
		flyingMermaidLevelEelSegment.transform.Rotate(0f, 0f, UnityEngine.Random.Range(0f, 360f));
		if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
		{
			flyingMermaidLevelEelSegment.transform.SetScale(new float?(-1f), null, null);
		}
		SpriteRenderer component = flyingMermaidLevelEelSegment.GetComponent<SpriteRenderer>();
		component.sortingLayerName = sortingLayer;
		component.sortingOrder = sortingOrder;
		flyingMermaidLevelEelSegment.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
		flyingMermaidLevelEelSegment.StartCoroutine(flyingMermaidLevelEelSegment.move_cr());
		return flyingMermaidLevelEelSegment;
	}

	// Token: 0x06002338 RID: 9016 RVA: 0x0014AED4 File Offset: 0x001492D4
	private IEnumerator move_cr()
	{
		while (base.transform.position.y > this.despawnY || this.velocity.y > 0f)
		{
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002BE2 RID: 11234
	[SerializeField]
	private float gravity;

	// Token: 0x04002BE3 RID: 11235
	[SerializeField]
	private MinMax angleRange;

	// Token: 0x04002BE4 RID: 11236
	[SerializeField]
	private float launchSpeed;

	// Token: 0x04002BE5 RID: 11237
	[SerializeField]
	private float despawnY;

	// Token: 0x04002BE6 RID: 11238
	private Vector2 velocity;
}
