using System;
using UnityEngine;

// Token: 0x02000950 RID: 2384
public class MapNPCCompetitor : AbstractMonoBehaviour
{
	// Token: 0x060037B6 RID: 14262 RVA: 0x001FFB24 File Offset: 0x001FDF24
	private void Update()
	{
		base.animator.SetBool("PlayerClose", this.interaction.PlayerWithinDistance(0) || (PlayerManager.Multiplayer && this.interaction.PlayerWithinDistance(1)) || this.interaction.currentlySpeaking);
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x04003FB9 RID: 16313
	[SerializeField]
	private MapDialogueInteraction interaction;

	// Token: 0x04003FBA RID: 16314
	[SerializeField]
	private MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04003FBB RID: 16315
	private float blinkTimer;
}
