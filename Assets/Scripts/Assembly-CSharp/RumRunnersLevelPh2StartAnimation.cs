using System;

// Token: 0x02000798 RID: 1944
public class RumRunnersLevelPh2StartAnimation : AbstractPausableComponent
{
	// Token: 0x170003F8 RID: 1016
	// (get) Token: 0x06002B2A RID: 11050 RVA: 0x00192BBF File Offset: 0x00190FBF
	// (set) Token: 0x06002B2B RID: 11051 RVA: 0x00192BC7 File Offset: 0x00190FC7
	public bool showBug { get; private set; }

	// Token: 0x170003F9 RID: 1017
	// (get) Token: 0x06002B2C RID: 11052 RVA: 0x00192BD0 File Offset: 0x00190FD0
	// (set) Token: 0x06002B2D RID: 11053 RVA: 0x00192BD8 File Offset: 0x00190FD8
	public bool dropped { get; private set; }

	// Token: 0x06002B2E RID: 11054 RVA: 0x00192BE1 File Offset: 0x00190FE1
	private void animationEvent_StartWeb()
	{
		base.animator.Play("Loop", 1);
	}

	// Token: 0x06002B2F RID: 11055 RVA: 0x00192BF4 File Offset: 0x00190FF4
	private void animationEvent_EndWeb()
	{
		base.animator.Play("Off", 1);
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x00192C07 File Offset: 0x00191007
	private void animationEvent_ShowBug()
	{
		this.showBug = true;
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x00192C10 File Offset: 0x00191010
	private void animationEvent_RopeDrop()
	{
		this.dropped = true;
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x00192C19 File Offset: 0x00191019
	private void AnimationEvent_SFX_RUMRUN_ExitPhase1_SpiderReturns()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns");
		this.emitAudioFromObject.Add("sfx_DLC_RUMRUN_ExitPhase1_SpiderReturns");
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x00192C35 File Offset: 0x00191035
	private void AnimationEvent_SFX_RUMRUN_ExitPhase1_GrammoDrop()
	{
		AudioManager.Play("sfx_dlc_rumrun_exitphase1_grammodrop");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_exitphase1_grammodrop");
	}
}
