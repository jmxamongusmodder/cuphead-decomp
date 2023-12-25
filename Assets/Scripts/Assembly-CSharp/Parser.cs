using System;
using System.Globalization;

// Token: 0x02000B29 RID: 2857
public static class Parser
{
	// Token: 0x06004538 RID: 17720 RVA: 0x00247A6A File Offset: 0x00245E6A
	public static string ToStringInvariant(this int value)
	{
		return value.ToString(Parser.InvariantInfo);
	}

	// Token: 0x06004539 RID: 17721 RVA: 0x00247A78 File Offset: 0x00245E78
	public static string ToStringInvariant(this float value)
	{
		return value.ToString(Parser.InvariantInfo);
	}

	// Token: 0x0600453A RID: 17722 RVA: 0x00247A86 File Offset: 0x00245E86
	public static int IntParse(string s)
	{
		return int.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x0600453B RID: 17723 RVA: 0x00247A93 File Offset: 0x00245E93
	public static bool IntTryParse(string s, out int result)
	{
		return int.TryParse(s, NumberStyles.Integer, Parser.InvariantInfo, out result);
	}

	// Token: 0x0600453C RID: 17724 RVA: 0x00247AA2 File Offset: 0x00245EA2
	public static float FloatParse(string s)
	{
		return float.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x0600453D RID: 17725 RVA: 0x00247AAF File Offset: 0x00245EAF
	public static bool FloatTryParse(string s, out float result)
	{
		return float.TryParse(s, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, Parser.InvariantInfo, out result);
	}

	// Token: 0x0600453E RID: 17726 RVA: 0x00247AC2 File Offset: 0x00245EC2
	public static byte ByteParse(string s)
	{
		return byte.Parse(s, Parser.InvariantInfo);
	}

	// Token: 0x0600453F RID: 17727 RVA: 0x00247ACF File Offset: 0x00245ECF
	public static byte ByteParse(string s, NumberStyles style)
	{
		return byte.Parse(s, style, Parser.InvariantInfo);
	}

	// Token: 0x06004540 RID: 17728 RVA: 0x00247ADD File Offset: 0x00245EDD
	public static bool ByteTryParse(string s, out byte result)
	{
		return byte.TryParse(s, NumberStyles.Integer, Parser.InvariantInfo, out result);
	}

	// Token: 0x04004AEA RID: 19178
	private static NumberFormatInfo InvariantInfo = CultureInfo.InvariantCulture.NumberFormat;
}
