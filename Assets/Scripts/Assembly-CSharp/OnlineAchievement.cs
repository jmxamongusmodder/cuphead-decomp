using System;

// Token: 0x020009BC RID: 2492
public interface OnlineAchievement
{
	// Token: 0x170004C1 RID: 1217
	// (get) Token: 0x06003A6E RID: 14958
	string Id { get; }

	// Token: 0x170004C2 RID: 1218
	// (get) Token: 0x06003A6F RID: 14959
	string Name { get; }

	// Token: 0x170004C3 RID: 1219
	// (get) Token: 0x06003A70 RID: 14960
	string Description { get; }

	// Token: 0x170004C4 RID: 1220
	// (get) Token: 0x06003A71 RID: 14961
	bool IsUnlocked { get; }
}
