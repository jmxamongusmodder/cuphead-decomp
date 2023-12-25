using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200090A RID: 2314
public class PlatformingLevelReverseGravitySwitch : ParrySwitch
{
	// Token: 0x0600364D RID: 13901 RVA: 0x001F7B8C File Offset: 0x001F5F8C
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		LevelPlayerController levelPlayerController = player as LevelPlayerController;
		levelPlayerController.motor.SetGravityReversed(!levelPlayerController.motor.GravityReversed);
		base.StartCoroutine(this.start_spin_cr(levelPlayerController));
		base.StartParryCooldown();
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x001F7BD4 File Offset: 0x001F5FD4
	private IEnumerator start_spin_cr(LevelPlayerController player)
	{
		base.animator.SetBool("IsSpin", true);
		base.animator.SetBool("IsUp", player.motor.GravityReversed);
		float t = 0f;
		while (t < this.spinTimer)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetBool("IsSpin", false);
		yield return null;
		yield break;
	}

	// Token: 0x04003E41 RID: 15937
	[SerializeField]
	private float spinTimer;
}
