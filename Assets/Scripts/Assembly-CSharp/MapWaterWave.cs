using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000966 RID: 2406
public class MapWaterWave : AbstractPausableComponent
{
	// Token: 0x06003815 RID: 14357 RVA: 0x002016C3 File Offset: 0x001FFAC3
	private void Start()
	{
		base.StartCoroutine(this.wave_cr());
	}

	// Token: 0x06003816 RID: 14358 RVA: 0x002016D4 File Offset: 0x001FFAD4
	private IEnumerator wave_cr()
	{
		for (;;)
		{
			base.animator.Play("Wave", 0, this.offsetRange.RandomFloat());
			yield return base.animator.WaitForAnimationToEnd(this, "Wave", false, true);
			yield return CupheadTime.WaitForSeconds(this, this.delayRange.RandomFloat());
		}
		yield break;
	}

	// Token: 0x04003FF1 RID: 16369
	[SerializeField]
	public MinMax offsetRange;

	// Token: 0x04003FF2 RID: 16370
	[SerializeField]
	public MinMax delayRange;
}
