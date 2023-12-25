using System;
using UnityEngine;

// Token: 0x02000563 RID: 1379
public class ClownLevelCoasterKnob : ParrySwitch
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x000ED87E File Offset: 0x000EBC7E
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		player.stats.ParryOneQuarter();
		this.sprite.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04002319 RID: 8985
	[SerializeField]
	private SpriteRenderer sprite;
}
