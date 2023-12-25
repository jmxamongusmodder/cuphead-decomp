using System;

// Token: 0x02000708 RID: 1800
public class OldManLevelParryThermometer : ParrySwitch
{
	// Token: 0x170003CD RID: 973
	// (get) Token: 0x060026D0 RID: 9936 RVA: 0x0016B519 File Offset: 0x00169919
	// (set) Token: 0x060026D1 RID: 9937 RVA: 0x0016B521 File Offset: 0x00169921
	public bool isActivated { get; private set; }

	// Token: 0x060026D2 RID: 9938 RVA: 0x0016B52A File Offset: 0x0016992A
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		this.isActivated = true;
		base.OnParryPrePause(player);
	}

	// Token: 0x060026D3 RID: 9939 RVA: 0x0016B53A File Offset: 0x0016993A
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.isActivated = false;
		base.gameObject.SetActive(false);
	}
}
