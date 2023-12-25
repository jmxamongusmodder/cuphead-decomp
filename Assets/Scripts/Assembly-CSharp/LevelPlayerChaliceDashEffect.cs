using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A17 RID: 2583
public class LevelPlayerChaliceDashEffect : Effect
{
	// Token: 0x06003D3F RID: 15679 RVA: 0x0021E1EC File Offset: 0x0021C5EC
	private void Start()
	{
		base.StartCoroutine(this.MoveUntilTail());
	}

	// Token: 0x06003D40 RID: 15680 RVA: 0x0021E1FC File Offset: 0x0021C5FC
	private IEnumerator MoveUntilTail()
	{
		yield return new WaitForEndOfFrame();
		float xFacing = base.transform.parent.transform.localScale.x;
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + ".Start");
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target && base.transform.parent.transform.localScale.x == xFacing)
		{
			yield return null;
		}
		if (base.transform.parent.transform.localScale.x != xFacing)
		{
			base.transform.localPosition = new Vector3(-base.transform.localPosition.x, base.transform.localPosition.y);
		}
		base.transform.parent = null;
		base.transform.localScale = new Vector3(xFacing, 1f);
		yield break;
	}

	// Token: 0x04004491 RID: 17553
	private float t;
}
