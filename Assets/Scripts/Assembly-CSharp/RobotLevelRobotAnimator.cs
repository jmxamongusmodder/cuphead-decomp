using System;

// Token: 0x02000773 RID: 1907
public class RobotLevelRobotAnimator : AbstractPausableComponent
{
	// Token: 0x0600299F RID: 10655 RVA: 0x00184807 File Offset: 0x00182C07
	private void ContinueMainAnimation()
	{
		base.animator.SetTrigger("StartMainAnim");
	}

	// Token: 0x060029A0 RID: 10656 RVA: 0x00184819 File Offset: 0x00182C19
	private void SyncAnimationLayers()
	{
		base.animator.SetTrigger("SyncLayers");
	}

	// Token: 0x060029A1 RID: 10657 RVA: 0x0018482B File Offset: 0x00182C2B
	private void MainAnimationStateOff()
	{
		base.animator.SetBool("MainAnimationActive", false);
	}

	// Token: 0x060029A2 RID: 10658 RVA: 0x0018483E File Offset: 0x00182C3E
	private void MainAnimationStateOn()
	{
		base.animator.SetBool("MainAnimationActive", true);
	}
}
