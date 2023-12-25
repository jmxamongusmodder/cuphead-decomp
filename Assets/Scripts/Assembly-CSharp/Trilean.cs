using System;
using UnityEngine;

// Token: 0x0200037F RID: 895
public struct Trilean
{
	// Token: 0x06000A78 RID: 2680 RVA: 0x0007EDB1 File Offset: 0x0007D1B1
	public Trilean(bool b)
	{
		if (b)
		{
			this.value = 1;
		}
		else
		{
			this.value = -1;
		}
	}

	// Token: 0x06000A79 RID: 2681 RVA: 0x0007EDCC File Offset: 0x0007D1CC
	public Trilean(int i)
	{
		this.value = i;
	}

	// Token: 0x06000A7A RID: 2682 RVA: 0x0007EDD5 File Offset: 0x0007D1D5
	public Trilean(float f)
	{
		if (f == 0f)
		{
			this.value = 0;
		}
		else
		{
			this.value = (int)Mathf.Sign(f);
		}
	}

	// Token: 0x170001FC RID: 508
	// (get) Token: 0x06000A7B RID: 2683 RVA: 0x0007EDFB File Offset: 0x0007D1FB
	// (set) Token: 0x06000A7C RID: 2684 RVA: 0x0007EE03 File Offset: 0x0007D203
	public int Value
	{
		get
		{
			return this.value;
		}
		set
		{
			if (value > 0)
			{
				this.value = 1;
			}
			else if (value < 0)
			{
				this.value = -1;
			}
			else
			{
				this.value = 0;
			}
		}
	}

	// Token: 0x06000A7D RID: 2685 RVA: 0x0007EE32 File Offset: 0x0007D232
	public static implicit operator Trilean(bool b)
	{
		return new Trilean(b);
	}

	// Token: 0x06000A7E RID: 2686 RVA: 0x0007EE3A File Offset: 0x0007D23A
	public static implicit operator bool(Trilean t)
	{
		return t.Value >= 0;
	}

	// Token: 0x06000A7F RID: 2687 RVA: 0x0007EE50 File Offset: 0x0007D250
	public static implicit operator Trilean(int i)
	{
		return new Trilean(i);
	}

	// Token: 0x06000A80 RID: 2688 RVA: 0x0007EE58 File Offset: 0x0007D258
	public static implicit operator int(Trilean t)
	{
		return t.Value;
	}

	// Token: 0x06000A81 RID: 2689 RVA: 0x0007EE61 File Offset: 0x0007D261
	public static implicit operator Trilean(float f)
	{
		return new Trilean(f);
	}

	// Token: 0x06000A82 RID: 2690 RVA: 0x0007EE69 File Offset: 0x0007D269
	public static implicit operator float(Trilean t)
	{
		return (float)t.Value;
	}

	// Token: 0x06000A83 RID: 2691 RVA: 0x0007EE73 File Offset: 0x0007D273
	public override string ToString()
	{
		return this.Value.ToStringInvariant();
	}

	// Token: 0x04001473 RID: 5235
	private int value;
}
