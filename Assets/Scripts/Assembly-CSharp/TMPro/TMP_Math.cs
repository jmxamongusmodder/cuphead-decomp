using System;

namespace TMPro
{
	// Token: 0x02000C99 RID: 3225
	public static class TMP_Math
	{
		// Token: 0x06005179 RID: 20857 RVA: 0x002995AF File Offset: 0x002979AF
		public static bool Approximately(float a, float b)
		{
			return b - 0.0001f < a && a < b + 0.0001f;
		}
	}
}
