using System;

// Token: 0x020007AD RID: 1965
public class SallyStagePlayLevelFirewheel : AbstractPausableComponent
{
	// Token: 0x06002C2C RID: 11308 RVA: 0x0019FA00 File Offset: 0x0019DE00
	public void PlaySound()
	{
		AudioManager.Play("sally_cherub_fireprop_move");
		this.emitAudioFromObject.Add("sally_cherub_fireprop_move");
	}
}
