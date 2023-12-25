using System;
using UnityEngine;

// Token: 0x02000380 RID: 896
public struct Trilean2
{
	// Token: 0x06000A84 RID: 2692 RVA: 0x0007EE80 File Offset: 0x0007D280
	public Trilean2(Vector2 v)
	{
		this.x = v.x;
		this.y = v.y;
	}

	// Token: 0x06000A85 RID: 2693 RVA: 0x0007EEA6 File Offset: 0x0007D2A6
	public Trilean2(bool x, bool y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000A86 RID: 2694 RVA: 0x0007EEC0 File Offset: 0x0007D2C0
	public Trilean2(int x, int y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000A87 RID: 2695 RVA: 0x0007EEDA File Offset: 0x0007D2DA
	public Trilean2(float x, float y)
	{
		this.x = x;
		this.y = y;
	}

	// Token: 0x06000A88 RID: 2696 RVA: 0x0007EEF4 File Offset: 0x0007D2F4
	public static implicit operator Trilean2(Vector2 v)
	{
		return new Trilean2(v);
	}

	// Token: 0x06000A89 RID: 2697 RVA: 0x0007EEFC File Offset: 0x0007D2FC
	public static implicit operator Vector2(Trilean2 t)
	{
		return new Vector2(t.x, t.y);
	}

	// Token: 0x06000A8A RID: 2698 RVA: 0x0007EF1B File Offset: 0x0007D31B
	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	// Token: 0x06000A8B RID: 2699 RVA: 0x0007EF2E File Offset: 0x0007D32E
	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	// Token: 0x06000A8C RID: 2700 RVA: 0x0007EF40 File Offset: 0x0007D340
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			"Trilean2(x:",
			this.x.Value,
			", y:",
			this.y.Value,
			")"
		});
	}

	// Token: 0x06000A8D RID: 2701 RVA: 0x0007EF96 File Offset: 0x0007D396
	public static bool operator ==(Trilean2 a, Trilean2 b)
	{
		return a.x.Value == b.x.Value && a.y.Value == b.y.Value;
	}

	// Token: 0x06000A8E RID: 2702 RVA: 0x0007EFD2 File Offset: 0x0007D3D2
	public static bool operator !=(Trilean2 a, Trilean2 b)
	{
		return a.x.Value != b.x.Value || a.y.Value != b.y.Value;
	}

	// Token: 0x04001474 RID: 5236
	public Trilean x;

	// Token: 0x04001475 RID: 5237
	public Trilean y;
}
