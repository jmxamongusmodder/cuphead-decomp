using System;

// Token: 0x02000467 RID: 1127
public class PlayerJoinPrompt : FlashingPrompt
{
	// Token: 0x170002B2 RID: 690
	// (get) Token: 0x06001136 RID: 4406 RVA: 0x000A4238 File Offset: 0x000A2638
	protected override bool ShouldShow
	{
		get
		{
			return PlayerManager.ShouldShowJoinPrompt;
		}
	}
}
