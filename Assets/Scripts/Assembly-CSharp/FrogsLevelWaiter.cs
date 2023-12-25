using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A6 RID: 1702
public class FrogsLevelWaiter : AbstractMonoBehaviour
{
	// Token: 0x06002410 RID: 9232 RVA: 0x00152B5E File Offset: 0x00150F5E
	private void Start()
	{
		base.StartCoroutine(this.waiter_cr());
	}

	// Token: 0x06002411 RID: 9233 RVA: 0x00152B70 File Offset: 0x00150F70
	private IEnumerator waiter_cr()
	{
		float x = base.transform.localPosition.x;
		for (;;)
		{
			base.transform.SetScale(new float?(1f), null, null);
			yield return base.StartCoroutine(this.move_cr(x, -x));
			yield return CupheadTime.WaitForSeconds(this, 2f);
			base.transform.SetScale(new float?(-1f), null, null);
			yield return base.StartCoroutine(this.move_cr(-x, x));
			yield return CupheadTime.WaitForSeconds(this, 2f);
		}
		yield break;
	}

	// Token: 0x06002412 RID: 9234 RVA: 0x00152B8C File Offset: 0x00150F8C
	private IEnumerator move_cr(float start, float end)
	{
		float t = 0f;
		base.transform.SetLocalPosition(new float?(start), null, null);
		while (t < 8f)
		{
			float val = t / 8f;
			base.transform.SetLocalPosition(new float?(Mathf.Lerp(start, end, val)), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(end), null, null);
		yield break;
	}

	// Token: 0x04002CE2 RID: 11490
	private const float TIME = 8f;
}
