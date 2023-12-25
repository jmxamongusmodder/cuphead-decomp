using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000822 RID: 2082
public class TrainLevelPassenger : AbstractPausableComponent
{
	// Token: 0x0600305D RID: 12381 RVA: 0x001C80C3 File Offset: 0x001C64C3
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x0600305E RID: 12382 RVA: 0x001C80D8 File Offset: 0x001C64D8
	private IEnumerator main_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(3f, 8f));
			base.animator.SetTrigger("Continue");
		}
		yield break;
	}
}
