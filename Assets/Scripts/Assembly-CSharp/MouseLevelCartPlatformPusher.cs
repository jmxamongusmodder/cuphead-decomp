using System;
using UnityEngine;

// Token: 0x020006E5 RID: 1765
public class MouseLevelCartPlatformPusher : AbstractCollidableObject
{
	// Token: 0x060025C0 RID: 9664 RVA: 0x001616C4 File Offset: 0x0015FAC4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		AbstractPlayerController component = hit.GetComponent<AbstractPlayerController>();
		Collider2D component2 = base.GetComponent<Collider2D>();
		Collider2D component3 = component.GetComponent<Collider2D>();
		if (component.bottom < component2.bounds.max.y)
		{
			if (component.center.x < component2.bounds.center.x)
			{
				float num = component3.bounds.max.x - component2.bounds.min.x;
				if (num > 0f)
				{
					component.transform.AddPosition(-num, 0f, 0f);
				}
			}
			else
			{
				float num2 = component2.bounds.max.x - component3.bounds.min.x;
				if (num2 > 0f)
				{
					component.transform.AddPosition(num2, 0f, 0f);
				}
			}
		}
	}
}
