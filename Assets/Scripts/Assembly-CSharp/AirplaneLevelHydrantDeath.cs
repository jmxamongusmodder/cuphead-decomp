using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004BC RID: 1212
public class AirplaneLevelHydrantDeath : MonoBehaviour
{
	// Token: 0x06001412 RID: 5138 RVA: 0x000B29D5 File Offset: 0x000B0DD5
	private void Start()
	{
		if (this.pieces)
		{
			base.StartCoroutine(this.recede_cr());
		}
	}

	// Token: 0x06001413 RID: 5139 RVA: 0x000B29F4 File Offset: 0x000B0DF4
	private IEnumerator recede_cr()
	{
		Vector3 startPos = base.transform.position;
		Vector3 endPos = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100f, base.transform.position.z);
		endPos = Vector3.Lerp(startPos, endPos, 0.35f);
		for (;;)
		{
			float t = this.anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
			this.pieces.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0f, 1f, t));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001D3E RID: 7486
	[SerializeField]
	private Animator anim;

	// Token: 0x04001D3F RID: 7487
	[SerializeField]
	private GameObject pieces;
}
