using System;

// Token: 0x02000538 RID: 1336
public class ChessBishopLevelBishopIntro : AbstractCollidableObject
{
	// Token: 0x06001841 RID: 6209 RVA: 0x000DBCA8 File Offset: 0x000DA0A8
	private void AniEvent_BishopIntroSFX()
	{
	}

	// Token: 0x06001842 RID: 6210 RVA: 0x000DBCAA File Offset: 0x000DA0AA
	private void AnimationEvent_SFX_KOG_Bishop_Intro_Vocal()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_intro_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_vocal");
	}

	// Token: 0x06001843 RID: 6211 RVA: 0x000DBCC6 File Offset: 0x000DA0C6
	private void AnimationEvent_SFX_KOG_Bishop_Intro_Sfx()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_intro_sfx");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_intro_sfx");
	}
}
