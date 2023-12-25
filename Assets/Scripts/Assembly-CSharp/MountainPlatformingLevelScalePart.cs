using System;
using UnityEngine;

// Token: 0x020008F1 RID: 2289
public class MountainPlatformingLevelScalePart : AbstractCollidableObject
{
	// Token: 0x060035AA RID: 13738 RVA: 0x001F4998 File Offset: 0x001F2D98
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<AbstractPlayerController>() != null)
		{
			if (phase == CollisionPhase.Exit)
			{
				this.steppedOn = false;
			}
			else
			{
				this.steppedOn = true;
			}
		}
	}

	// Token: 0x04003DC3 RID: 15811
	public bool steppedOn;
}
