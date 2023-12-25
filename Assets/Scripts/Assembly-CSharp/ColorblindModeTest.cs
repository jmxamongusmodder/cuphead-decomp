using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200047D RID: 1149
[RequireComponent(typeof(SpriteRenderer))]
public class ColorblindModeTest : AbstractMonoBehaviour
{
	// Token: 0x060011B2 RID: 4530 RVA: 0x000A5EB0 File Offset: 0x000A42B0
	private void Start()
	{
		this.mat = base.GetComponent<SpriteRenderer>().material;
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x060011B3 RID: 4531 RVA: 0x000A5ED0 File Offset: 0x000A42D0
	private IEnumerator flash_cr()
	{
		bool goingUp = true;
		float valMin = 0f;
		float valMax = 2f;
		float start = valMin;
		float end = valMax;
		float t = 0f;
		float time = 0.2f;
		for (;;)
		{
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.mat.SetFloat("_Intensity", Mathf.Lerp(start, end, t / time));
				yield return null;
			}
			this.mat.SetFloat("_Intensity", end);
			goingUp = !goingUp;
			start = ((!goingUp) ? valMax : valMin);
			end = ((!goingUp) ? valMin : valMax);
			t = 0f;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001B34 RID: 6964
	private Material mat;
}
