using System;

// Token: 0x020006B5 RID: 1717
public class FrogsLevelParryableFlame : ParrySwitch
{
	// Token: 0x06002471 RID: 9329 RVA: 0x001559DF File Offset: 0x00153DDF
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}
}
