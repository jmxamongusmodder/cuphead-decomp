using System;
using UnityEngine;

// Token: 0x02000685 RID: 1669
public class FlyingMermaidLevelEelBullet : BasicProjectile
{
	// Token: 0x06002335 RID: 9013 RVA: 0x0014AD9C File Offset: 0x0014919C
	private void RotateSpark()
	{
		this.spark.transform.SetEulerAngles(null, null, new float?((float)UnityEngine.Random.Range(0, 360)));
	}

	// Token: 0x04002BE1 RID: 11233
	[SerializeField]
	private Transform spark;
}
