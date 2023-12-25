using System;
using UnityEngine;

// Token: 0x02000400 RID: 1024
public class DLCIntroBoat : AbstractPausableComponent
{
	// Token: 0x06000E38 RID: 3640 RVA: 0x00091AEC File Offset: 0x0008FEEC
	private void FixedUpdate()
	{
		this.curSpeed = Mathf.Lerp(this.speed.GetFloatAt(1f - this.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f), this.speed.GetFloatAt((1.1f - this.boatmanAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f) % 1f), 0.5f);
		base.transform.position += Vector3.right * this.curSpeed * CupheadTime.FixedDelta;
	}

	// Token: 0x0400177A RID: 6010
	[SerializeField]
	private Animator boatmanAnimator;

	// Token: 0x0400177B RID: 6011
	[SerializeField]
	private MinMax speed;

	// Token: 0x0400177C RID: 6012
	private float curSpeed;
}
