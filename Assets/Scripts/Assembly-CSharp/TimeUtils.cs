using System;

// Token: 0x02000399 RID: 921
public static class TimeUtils
{
	// Token: 0x06000B38 RID: 2872 RVA: 0x00082594 File Offset: 0x00080994
	public static int GetCurrentSecond()
	{
		DateTime d = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
		return (int)(DateTime.UtcNow - d).TotalSeconds;
	}
}
