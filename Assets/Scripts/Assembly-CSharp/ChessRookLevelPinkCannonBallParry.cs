using System;
using UnityEngine;

// Token: 0x02000552 RID: 1362
public class ChessRookLevelPinkCannonBallParry : AbstractProjectile
{
	// Token: 0x17000344 RID: 836
	// (get) Token: 0x06001957 RID: 6487 RVA: 0x000E5E4C File Offset: 0x000E424C
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x000E5E53 File Offset: 0x000E4253
	protected override void RandomizeVariant()
	{
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x000E5E55 File Offset: 0x000E4255
	protected override void SetTrigger(string trigger)
	{
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x000E5E57 File Offset: 0x000E4257
	public override void OnParry(AbstractPlayerController player)
	{
		this.main.GotParried(player);
	}

	// Token: 0x04002275 RID: 8821
	[SerializeField]
	private ChessRookLevelPinkCannonBall main;
}
