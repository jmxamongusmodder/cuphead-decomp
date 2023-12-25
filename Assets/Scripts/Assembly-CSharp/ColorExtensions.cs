using System;
using UnityEngine;

// Token: 0x02000368 RID: 872
public static class ColorExtensions
{
	// Token: 0x060009BE RID: 2494 RVA: 0x0007CBF0 File Offset: 0x0007AFF0
	public static string ToHex(this Color color)
	{
		return ColorUtils.ColorToHex(color, false);
	}

	// Token: 0x060009BF RID: 2495 RVA: 0x0007CBFE File Offset: 0x0007AFFE
	public static string ToHex(this Color color, bool alpha)
	{
		return ColorUtils.ColorToHex(color, alpha);
	}

	// Token: 0x060009C0 RID: 2496 RVA: 0x0007CC0C File Offset: 0x0007B00C
	public static string ToNiceString(this Color color)
	{
		return string.Concat(new object[]
		{
			"R:",
			color.r,
			" G:",
			color.g,
			" B:",
			color.b,
			" A:",
			color.a
		});
	}
}
