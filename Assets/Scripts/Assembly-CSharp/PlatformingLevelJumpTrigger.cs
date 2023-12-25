using System;
using UnityEngine;

// Token: 0x02000869 RID: 2153
public class PlatformingLevelJumpTrigger : AbstractCollidableObject
{
	// Token: 0x06003207 RID: 12807 RVA: 0x001D3410 File Offset: 0x001D1810
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			PlatformingLevelGroundMovementEnemy component = hit.GetComponent<PlatformingLevelGroundMovementEnemy>();
			if (component != null && component.direction == this.direction)
			{
				component.Jump();
			}
		}
	}

	// Token: 0x06003208 RID: 12808 RVA: 0x001D3455 File Offset: 0x001D1855
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003209 RID: 12809 RVA: 0x001D3468 File Offset: 0x001D1868
	private void DrawGizmos(float a)
	{
		BoxCollider2D component = base.GetComponent<BoxCollider2D>();
		Gizmos.color = new Color(1f, 1f, 0f, a);
		Gizmos.DrawWireCube(component.bounds.center, component.bounds.size);
	}

	// Token: 0x04003A6D RID: 14957
	[SerializeField]
	private PlatformingLevelGroundMovementEnemy.Direction direction;
}
