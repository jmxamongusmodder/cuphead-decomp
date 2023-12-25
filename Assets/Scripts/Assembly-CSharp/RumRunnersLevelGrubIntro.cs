using System;
using UnityEngine;

// Token: 0x0200078E RID: 1934
public class RumRunnersLevelGrubIntro : AbstractPausableComponent
{
	// Token: 0x06002AD7 RID: 10967 RVA: 0x0018FF68 File Offset: 0x0018E368
	private void animationEvent_StartExit()
	{
		Animator component = base.GetComponent<Animator>();
		component.SetLayerWeight(1, 0f);
	}

	// Token: 0x06002AD8 RID: 10968 RVA: 0x0018FF88 File Offset: 0x0018E388
	private void AnimationEvent_MoveToForeground()
	{
		this.rend.sortingLayerName = "Foreground";
		this.rend.sortingOrder = 200;
	}

	// Token: 0x06002AD9 RID: 10969 RVA: 0x0018FFAA File Offset: 0x0018E3AA
	private void AnimationEvent_SFX_RUMRUN_FakeAnnouncer_BeginAhhh()
	{
		AudioManager.FadeSFXVolume("sfx_dlc_rumrun_vx_fakeannouncer_begin", 0f, 0.1f);
		AudioManager.Play("sfx_dlc_rumrun_vx_fakeannouncer_begin_ahhh");
	}

	// Token: 0x06002ADA RID: 10970 RVA: 0x0018FFCA File Offset: 0x0018E3CA
	private void AnimationEvent_SFX_RUMRUN_Intro_GrubFliesAway()
	{
		AudioManager.Play("sfx_dlc_rumrun_intro_grubfliesaway");
	}

	// Token: 0x04003399 RID: 13209
	[SerializeField]
	private SpriteRenderer rend;
}
