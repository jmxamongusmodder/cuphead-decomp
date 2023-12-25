using System;

// Token: 0x02000AE1 RID: 2785
public interface PlmInterface
{
	// Token: 0x140000B8 RID: 184
	// (add) Token: 0x0600432A RID: 17194
	// (remove) Token: 0x0600432B RID: 17195
	event OnSuspendHandler OnSuspend;

	// Token: 0x140000B9 RID: 185
	// (add) Token: 0x0600432C RID: 17196
	// (remove) Token: 0x0600432D RID: 17197
	event OnResumeHandler OnResume;

	// Token: 0x140000BA RID: 186
	// (add) Token: 0x0600432E RID: 17198
	// (remove) Token: 0x0600432F RID: 17199
	event OnConstrainedHandler OnConstrained;

	// Token: 0x140000BB RID: 187
	// (add) Token: 0x06004330 RID: 17200
	// (remove) Token: 0x06004331 RID: 17201
	event OnUnconstrainedHandler OnUnconstrained;

	// Token: 0x06004332 RID: 17202
	void Init();

	// Token: 0x06004333 RID: 17203
	bool IsConstrained();
}
