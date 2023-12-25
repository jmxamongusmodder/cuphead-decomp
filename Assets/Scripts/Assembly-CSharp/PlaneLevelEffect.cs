using System;
using UnityEngine;

// Token: 0x02000B14 RID: 2836
public class PlaneLevelEffect : Effect
{
	// Token: 0x060044C5 RID: 17605 RVA: 0x002468E3 File Offset: 0x00244CE3
	private void Update()
	{
		base.transform.AddPosition(-300f * CupheadTime.Delta * this.speed, 0f, 0f);
	}

	// Token: 0x04004A80 RID: 19072
	public const float SPEED = 300f;

	// Token: 0x04004A81 RID: 19073
	[Range(0f, 2f)]
	public float speed = 1f;
}
