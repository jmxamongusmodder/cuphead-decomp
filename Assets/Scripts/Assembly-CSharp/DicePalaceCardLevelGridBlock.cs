using System;
using UnityEngine;

// Token: 0x020005A9 RID: 1449
public class DicePalaceCardLevelGridBlock : AbstractCollidableObject
{
	// Token: 0x06001BEB RID: 7147 RVA: 0x000FFE83 File Offset: 0x000FE283
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x040024FF RID: 9471
	public float Xcoordinate;

	// Token: 0x04002500 RID: 9472
	public float Ycoordinate;

	// Token: 0x04002501 RID: 9473
	public bool hasBlock;

	// Token: 0x04002502 RID: 9474
	public float size;

	// Token: 0x04002503 RID: 9475
	public DicePalaceCardLevelBlock blockHeld;
}
