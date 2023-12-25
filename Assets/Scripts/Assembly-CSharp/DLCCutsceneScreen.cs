using System;
using UnityEngine;

// Token: 0x020003FB RID: 1019
public class DLCCutsceneScreen : AbstractMonoBehaviour
{
	// Token: 0x06000DF0 RID: 3568 RVA: 0x00090ACD File Offset: 0x0008EECD
	private void AniEvent_ShowText()
	{
		this.cutscene.ShowText();
	}

	// Token: 0x06000DF1 RID: 3569 RVA: 0x00090ADA File Offset: 0x0008EEDA
	private void AniEvent_ShowArrow()
	{
		this.cutscene.ShowArrow();
	}

	// Token: 0x06000DF2 RID: 3570 RVA: 0x00090AE7 File Offset: 0x0008EEE7
	private void AniEvent_IrisIn()
	{
		this.cutscene.IrisIn();
	}

	// Token: 0x06000DF3 RID: 3571 RVA: 0x00090AF4 File Offset: 0x0008EEF4
	private void AniEvent_IrisOut()
	{
		this.cutscene.IrisOut();
	}

	// Token: 0x06000DF4 RID: 3572 RVA: 0x00090B01 File Offset: 0x0008EF01
	private void AnimEvent_SFX_IntroStart_SeagullCall_1()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_1");
	}

	// Token: 0x06000DF5 RID: 3573 RVA: 0x00090B0D File Offset: 0x0008EF0D
	private void AnimEvent_SFX_IntroStart_SeagullCall_2()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_2");
	}

	// Token: 0x06000DF6 RID: 3574 RVA: 0x00090B19 File Offset: 0x0008EF19
	private void AnimEvent_SFX_IntroStart_SeagullCall_3()
	{
		AudioManager.Play("sfx_dlc_intro_seagullcall_3");
	}

	// Token: 0x06000DF7 RID: 3575 RVA: 0x00090B25 File Offset: 0x0008EF25
	private void SFX_IntroStart_OceanAmbLoopStart()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0.5f, 1f);
	}

	// Token: 0x06000DF8 RID: 3576 RVA: 0x00090B3B File Offset: 0x0008EF3B
	private void SFX_IntroStart_OceanAmbLoopStop()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_intro_oceanamb_loop", 0f, 0.1f);
	}

	// Token: 0x06000DF9 RID: 3577 RVA: 0x00090B51 File Offset: 0x0008EF51
	private void AnimEvent_SFX_Intro_ChalliceAppear()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceappear");
	}

	// Token: 0x06000DFA RID: 3578 RVA: 0x00090B5D File Offset: 0x0008EF5D
	private void AnimEvent_SFX_Intro_Challiceglows()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_challiceglows");
	}

	// Token: 0x06000DFB RID: 3579 RVA: 0x00090B69 File Offset: 0x0008EF69
	private void AnimEvent_SFX_Intro_EatCookie()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_eatcookie");
	}

	// Token: 0x06000DFC RID: 3580 RVA: 0x00090B75 File Offset: 0x0008EF75
	private void AnimEvent_SFX_Intro_EnterBakery()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_enterbakery");
	}

	// Token: 0x06000DFD RID: 3581 RVA: 0x00090B81 File Offset: 0x0008EF81
	private void AnimEvent_SFX_Intro_FollowChallice()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_followchallice");
	}

	// Token: 0x06000DFE RID: 3582 RVA: 0x00090B8D File Offset: 0x0008EF8D
	private void AnimEvent_SFX_Intro_Recipeaccept()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_recipeaccept");
	}

	// Token: 0x06000DFF RID: 3583 RVA: 0x00090B99 File Offset: 0x0008EF99
	private void AnimEvent_SFX_Intro_SaltBakerRecipe()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_saltbakerrecipe");
	}

	// Token: 0x06000E00 RID: 3584 RVA: 0x00090BA5 File Offset: 0x0008EFA5
	private void AnimEvent_SFX_Intro_CookieMagic()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_cookiemagic");
	}

	// Token: 0x06000E01 RID: 3585 RVA: 0x00090BB1 File Offset: 0x0008EFB1
	private void AnimEvent_SFX_Intro_FirstSwapGhost()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_firstswapghost");
	}

	// Token: 0x06000E02 RID: 3586 RVA: 0x00090BBD File Offset: 0x0008EFBD
	private void AnimEvent_SFX_Intro_SecondSwapGhost()
	{
		AudioManager.Play("sfx_dlc_cutscene_intro_secondswapghost");
	}

	// Token: 0x06000E03 RID: 3587 RVA: 0x00090BC9 File Offset: 0x0008EFC9
	private void AnimEvent_SFX_SaltBakerPre_ChaliceReveal()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_chalicereveal");
	}

	// Token: 0x06000E04 RID: 3588 RVA: 0x00090BD5 File Offset: 0x0008EFD5
	private void AnimEvent_SFX_SaltBakerPre_EndLeanIn()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_endleanin");
	}

	// Token: 0x06000E05 RID: 3589 RVA: 0x00090BE1 File Offset: 0x0008EFE1
	private void AnimEvent_SFX_SaltBakerPre_HelpClose()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_helpclose");
	}

	// Token: 0x06000E06 RID: 3590 RVA: 0x00090BED File Offset: 0x0008EFED
	private void AnimEvent_SFX_SaltBakerPre_KnifeOakTableLol()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifedefinitelyoaktable");
	}

	// Token: 0x06000E07 RID: 3591 RVA: 0x00090BF9 File Offset: 0x0008EFF9
	private void AnimEvent_SFX_SaltBakerPre_KnifeSwipe()
	{
		AudioManager.Play("sfx_dlc_cutscene_saltbakerpre_knifeswipe");
	}

	// Token: 0x04001755 RID: 5973
	[SerializeField]
	protected DLCGenericCutscene cutscene;
}
