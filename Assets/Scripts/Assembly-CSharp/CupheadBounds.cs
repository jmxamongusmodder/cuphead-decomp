using System;
using UnityEngine;

// Token: 0x0200037B RID: 891
[Serializable]
public class CupheadBounds
{
	// Token: 0x06000A60 RID: 2656 RVA: 0x0007E908 File Offset: 0x0007CD08
	public CupheadBounds()
	{
		this.left = 0f;
		this.right = 0f;
		this.top = 0f;
		this.bottom = 0f;
	}

	// Token: 0x06000A61 RID: 2657 RVA: 0x0007E93C File Offset: 0x0007CD3C
	public CupheadBounds(float left, float right, float top, float bottom)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}

	// Token: 0x06000A62 RID: 2658 RVA: 0x0007E964 File Offset: 0x0007CD64
	public CupheadBounds(Rect r)
	{
		this.left = r.center.x - r.x;
		this.top = r.center.y - r.y;
		this.right = r.xMax - r.center.x;
		this.bottom = r.yMax - r.center.y;
	}

	// Token: 0x06000A63 RID: 2659 RVA: 0x0007E9EB File Offset: 0x0007CDEB
	public static implicit operator CupheadBounds(Rect r)
	{
		return new CupheadBounds(r);
	}

	// Token: 0x06000A64 RID: 2660 RVA: 0x0007E9F3 File Offset: 0x0007CDF3
	public CupheadBounds Copy()
	{
		return base.MemberwiseClone() as CupheadBounds;
	}

	// Token: 0x04001467 RID: 5223
	public float left;

	// Token: 0x04001468 RID: 5224
	public float right;

	// Token: 0x04001469 RID: 5225
	public float top;

	// Token: 0x0400146A RID: 5226
	public float bottom;
}
