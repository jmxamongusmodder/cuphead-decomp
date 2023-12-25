using System;

// Token: 0x02000AA2 RID: 2722
public class PlanePlayerParryEffect : AbstractParryEffect
{
	// Token: 0x170005B5 RID: 1461
	// (get) Token: 0x06004148 RID: 16712 RVA: 0x00236954 File Offset: 0x00234D54
	protected override bool IsHit
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06004149 RID: 16713 RVA: 0x00236957 File Offset: 0x00234D57
	protected override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		this.planePlayer = (player as PlanePlayerController);
	}

	// Token: 0x0600414A RID: 16714 RVA: 0x0023696C File Offset: 0x00234D6C
	protected override void OnSuccess()
	{
		base.OnSuccess();
		this.planePlayer.parryController.OnParrySuccess();
	}

	// Token: 0x040047DF RID: 18399
	private PlanePlayerController planePlayer;
}
