using System;

// Token: 0x02000AC6 RID: 2758
public class PlayerDebug
{
	// Token: 0x06004239 RID: 16953 RVA: 0x0023C2F6 File Offset: 0x0023A6F6
	public static void Enable()
	{
		PlayerDebug.Enabled = true;
	}

	// Token: 0x0600423A RID: 16954 RVA: 0x0023C2FE File Offset: 0x0023A6FE
	public static void Disable()
	{
		PlayerDebug.Enabled = false;
	}

	// Token: 0x0600423B RID: 16955 RVA: 0x0023C306 File Offset: 0x0023A706
	public static void Toggle()
	{
		PlayerDebug.Enabled = !PlayerDebug.Enabled;
	}

	// Token: 0x040048AF RID: 18607
	public static bool Enabled = true;
}
