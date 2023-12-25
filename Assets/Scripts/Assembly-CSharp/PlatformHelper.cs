using System;

// Token: 0x02000379 RID: 889
public static class PlatformHelper
{
	// Token: 0x170001F4 RID: 500
	// (get) Token: 0x06000A53 RID: 2643 RVA: 0x0007E702 File Offset: 0x0007CB02
	public static bool IsConsole
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001F5 RID: 501
	// (get) Token: 0x06000A54 RID: 2644 RVA: 0x0007E705 File Offset: 0x0007CB05
	public static bool PreloadSettingsData
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001F6 RID: 502
	// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0007E708 File Offset: 0x0007CB08
	public static bool ShowAchievements
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001F7 RID: 503
	// (get) Token: 0x06000A56 RID: 2646 RVA: 0x0007E70B File Offset: 0x0007CB0B
	public static bool ShowDLCMenuItem
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001F8 RID: 504
	// (get) Token: 0x06000A57 RID: 2647 RVA: 0x0007E70E File Offset: 0x0007CB0E
	public static bool GarbageCollectOnPause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001F9 RID: 505
	// (get) Token: 0x06000A58 RID: 2648 RVA: 0x0007E711 File Offset: 0x0007CB11
	public static bool ForceAdditionalHeapMemory
	{
		get
		{
			return false;
		}
	}

	// Token: 0x170001FA RID: 506
	// (get) Token: 0x06000A59 RID: 2649 RVA: 0x0007E714 File Offset: 0x0007CB14
	public static bool ManuallyRefreshDLCAvailability
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001FB RID: 507
	// (get) Token: 0x06000A5A RID: 2650 RVA: 0x0007E717 File Offset: 0x0007CB17
	public static bool CanSwitchUserFromPause
	{
		get
		{
			return false;
		}
	}
}
