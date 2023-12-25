using System;

// Token: 0x02000724 RID: 1828
public class PirateLevelDogFishScope : AbstractMonoBehaviour
{
	// Token: 0x060027CF RID: 10191 RVA: 0x00174B03 File Offset: 0x00172F03
	public void In()
	{
		base.animator.Play("In");
	}

	// Token: 0x060027D0 RID: 10192 RVA: 0x00174B15 File Offset: 0x00172F15
	private void SoundDogfishPeriStart()
	{
		AudioManager.Play("level_pirate_periscope_warning");
	}
}
