using System;

// Token: 0x02000401 RID: 1025
public class DLCIntroBoatman : AbstractPausableComponent
{
	// Token: 0x06000E3A RID: 3642 RVA: 0x00091B9D File Offset: 0x0008FF9D
	private void AniEvent_Paddle_SFX()
	{
		AudioManager.Play("sfx_DLC_Intro_PaddleWater");
		this.emitAudioFromObject.Add("sfx_DLC_Intro_PaddleWater");
	}
}
