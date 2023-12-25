using System;
using UnityEngine;

// Token: 0x0200097E RID: 2430
public class MapPlayerLadderObject
{
	// Token: 0x060038B6 RID: 14518 RVA: 0x002049B2 File Offset: 0x00202DB2
	public MapPlayerLadderObject(Vector2 top, Vector2 bottom)
	{
		this.top = top;
		this.bottom = bottom;
	}

	// Token: 0x0400406B RID: 16491
	public readonly Vector2 top;

	// Token: 0x0400406C RID: 16492
	public readonly Vector2 bottom;
}
