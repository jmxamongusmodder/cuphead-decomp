using System;

// Token: 0x02000385 RID: 901
[Serializable]
public struct Rangef
{
	// Token: 0x06000AA8 RID: 2728 RVA: 0x0007FA5C File Offset: 0x0007DE5C
	public Rangef(float minimum, float maximum)
	{
		this.minimum = minimum;
		this.maximum = maximum;
	}

	// Token: 0x06000AA9 RID: 2729 RVA: 0x0007FA6C File Offset: 0x0007DE6C
	public bool ContainsInclusive(float checkValue)
	{
		return MathUtilities.BetweenInclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x06000AAA RID: 2730 RVA: 0x0007FA80 File Offset: 0x0007DE80
	public bool ContainsExclusive(float checkValue)
	{
		return MathUtilities.BetweenExclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x06000AAB RID: 2731 RVA: 0x0007FA94 File Offset: 0x0007DE94
	public bool ContainsInclusiveExclusive(float checkValue)
	{
		return MathUtilities.BetweenInclusiveExclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x06000AAC RID: 2732 RVA: 0x0007FAA8 File Offset: 0x0007DEA8
	public bool ContainsExclusiveInclusive(float checkValue)
	{
		return MathUtilities.BetweenExclusiveInclusive(checkValue, this.minimum, this.maximum);
	}

	// Token: 0x06000AAD RID: 2733 RVA: 0x0007FABC File Offset: 0x0007DEBC
	public override string ToString()
	{
		return string.Format("({0}, {1})", this.minimum, this.maximum);
	}

	// Token: 0x0400147E RID: 5246
	public float minimum;

	// Token: 0x0400147F RID: 5247
	public float maximum;
}
