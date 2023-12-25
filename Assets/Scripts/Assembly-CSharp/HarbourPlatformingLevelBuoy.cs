using System;
using UnityEngine;

// Token: 0x020008C4 RID: 2244
public class HarbourPlatformingLevelBuoy : AbstractPausableComponent
{
	// Token: 0x0600346B RID: 13419 RVA: 0x001E7070 File Offset: 0x001E5470
	private void Start()
	{
		this.parrySwitch.OnActivate += this.ParrySoundSFX;
		this.parrySwitch.OnActivate += this.parrySwitch.StartParryCooldown;
	}

	// Token: 0x0600346C RID: 13420 RVA: 0x001E70A8 File Offset: 0x001E54A8
	private void PlayIdle()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)) && !AudioManager.CheckIfPlaying("harbour_buoy_idle"))
		{
			AudioManager.Play("harbour_buoy_idle");
			this.emitAudioFromObject.Add("harbour_buoy_idle");
		}
	}

	// Token: 0x0600346D RID: 13421 RVA: 0x001E710C File Offset: 0x001E550C
	private void ParrySoundSFX()
	{
		AudioManager.Play("harbour_buoy_parry");
		this.emitAudioFromObject.Add("harbour_buoy_parry");
	}

	// Token: 0x04003C96 RID: 15510
	[SerializeField]
	private ParrySwitch parrySwitch;

	// Token: 0x04003C97 RID: 15511
	private const float ON_SCREEN_SOUND_PADDING = 100f;
}
