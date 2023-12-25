using System;
using UnityEngine;

// Token: 0x020004BD RID: 1213
public class AirplaneLevelLaser : ParrySwitch
{
	// Token: 0x06001415 RID: 5141 RVA: 0x000B2E93 File Offset: 0x000B1293
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06001416 RID: 5142 RVA: 0x000B2EA7 File Offset: 0x000B12A7
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.StartParryCooldown();
		this.anim.Play("End");
	}

	// Token: 0x04001D40 RID: 7488
	[SerializeField]
	private Animator anim;
}
