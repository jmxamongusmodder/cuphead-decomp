using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class ClownLevelCoasterPiece : AbstractCollidableObject
{
	// Token: 0x060019F8 RID: 6648 RVA: 0x000ED8B7 File Offset: 0x000EBCB7
	public void Init(Vector3 startPos)
	{
		base.transform.position = startPos;
	}

	// Token: 0x0400231A RID: 8986
	public List<ClownLevelRiders> riders;

	// Token: 0x0400231B RID: 8987
	public Transform newPieceRoot;

	// Token: 0x0400231C RID: 8988
	public Transform tailRoot;

	// Token: 0x0400231D RID: 8989
	public Transform ridersFrontRoot;

	// Token: 0x0400231E RID: 8990
	public Transform ridersBackRoot;
}
