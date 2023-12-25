using System;

// Token: 0x02000A8F RID: 2703
public class ParryAttackSpark : Effect
{
	// Token: 0x1700059D RID: 1437
	// (set) Token: 0x060040A2 RID: 16546 RVA: 0x00232B71 File Offset: 0x00230F71
	public bool IsCuphead
	{
		set
		{
			base.animator.SetBool("IsCuphead", value);
		}
	}
}
