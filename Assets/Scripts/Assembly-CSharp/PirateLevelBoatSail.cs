using System;
using UnityEngine;

// Token: 0x02000721 RID: 1825
public class PirateLevelBoatSail : AbstractMonoBehaviour
{
	// Token: 0x060027BB RID: 10171 RVA: 0x0017445C File Offset: 0x0017285C
	private void RegularEnded()
	{
		if (this.reg >= this.regTarget)
		{
			this.StartFast();
			return;
		}
		this.reg++;
	}

	// Token: 0x060027BC RID: 10172 RVA: 0x00174484 File Offset: 0x00172884
	private void FastEnded()
	{
		if (this.fast >= this.fastTarget)
		{
			this.StartReg();
			return;
		}
		this.fast++;
	}

	// Token: 0x060027BD RID: 10173 RVA: 0x001744AC File Offset: 0x001728AC
	private void StartReg()
	{
		this.regTarget = UnityEngine.Random.Range(this.regularLoopsMin, this.regularLoopsMax + 1);
		this.reg = 0;
		base.animator.SetBool("Fast", false);
	}

	// Token: 0x060027BE RID: 10174 RVA: 0x001744DF File Offset: 0x001728DF
	private void StartFast()
	{
		this.fastTarget = UnityEngine.Random.Range(this.fastLoopsMin, this.fastLoopsMax + 1);
		this.fast = 0;
		base.animator.SetBool("Fast", true);
	}

	// Token: 0x04003075 RID: 12405
	[Space(10f)]
	[Range(1f, 20f)]
	[SerializeField]
	private int regularLoopsMin = 3;

	// Token: 0x04003076 RID: 12406
	[Range(1f, 20f)]
	[SerializeField]
	private int regularLoopsMax = 5;

	// Token: 0x04003077 RID: 12407
	[Space(10f)]
	[Range(1f, 20f)]
	[SerializeField]
	private int fastLoopsMin = 5;

	// Token: 0x04003078 RID: 12408
	[Range(1f, 20f)]
	[SerializeField]
	private int fastLoopsMax = 9;

	// Token: 0x04003079 RID: 12409
	private int reg;

	// Token: 0x0400307A RID: 12410
	private int fast;

	// Token: 0x0400307B RID: 12411
	private int regTarget = 4;

	// Token: 0x0400307C RID: 12412
	private int fastTarget = 7;
}
