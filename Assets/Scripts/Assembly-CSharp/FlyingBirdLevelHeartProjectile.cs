using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000620 RID: 1568
public class FlyingBirdLevelHeartProjectile : BasicProjectile
{
	// Token: 0x06001FE9 RID: 8169 RVA: 0x001253BB File Offset: 0x001237BB
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x06001FEA RID: 8170 RVA: 0x001253D0 File Offset: 0x001237D0
	private IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.FX.Create(base.transform.position).transform.SetEulerAngles(null, null, new float?(base.transform.eulerAngles.z));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x04002868 RID: 10344
	[SerializeField]
	private Effect FX;
}
