using System;

// Token: 0x02000A0A RID: 2570
public class PlayerDeathParrySwitch : ParrySwitch
{
	// Token: 0x06003CC2 RID: 15554 RVA: 0x0021A0E6 File Offset: 0x002184E6
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.OnParry(1f, true);
	}
}
