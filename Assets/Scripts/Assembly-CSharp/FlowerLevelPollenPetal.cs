using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000610 RID: 1552
public class FlowerLevelPollenPetal : Effect
{
	// Token: 0x06001F4D RID: 8013 RVA: 0x0011F940 File Offset: 0x0011DD40
	private void Start()
	{
		string stateName = (!Rand.Bool()) ? "Petal_B" : "Petal_A";
		base.animator.Play(stateName);
		base.StartCoroutine(this.fall_cr());
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x06001F4E RID: 8014 RVA: 0x0011F990 File Offset: 0x0011DD90
	private IEnumerator fall_cr()
	{
		float fallSpeed = 100f;
		for (;;)
		{
			base.transform.position -= Vector3.up * fallSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001F4F RID: 8015 RVA: 0x0011F9AC File Offset: 0x0011DDAC
	private IEnumerator fade_cr()
	{
		float t = 0f;
		float time = 2f;
		Color currentColor = base.GetComponent<SpriteRenderer>().color;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		while (t < time)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		this.OnEffectComplete();
		yield return null;
		yield break;
	}
}
