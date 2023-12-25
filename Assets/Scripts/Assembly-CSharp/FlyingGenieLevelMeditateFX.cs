using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200066F RID: 1647
public class FlyingGenieLevelMeditateFX : Effect
{
	// Token: 0x060022A3 RID: 8867 RVA: 0x0014569C File Offset: 0x00143A9C
	private void Start()
	{
		base.StartCoroutine(this.effect_cr());
	}

	// Token: 0x060022A4 RID: 8868 RVA: 0x001456AC File Offset: 0x00143AAC
	private IEnumerator effect_cr()
	{
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		sprite.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			sprite.color = new Color(1f, 1f, 1f, t / time);
			yield return null;
		}
		sprite.color = new Color(1f, 1f, 1f, 1f);
		t = 0f;
		for (;;)
		{
			this.frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (this.frameTime > 0.083333336f)
			{
				base.transform.SetEulerAngles(null, null, new float?(360f * t));
				this.frameTime -= 0.083333336f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060022A5 RID: 8869 RVA: 0x001456C7 File Offset: 0x00143AC7
	public void EndEffect()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.end_effect_cr());
	}

	// Token: 0x060022A6 RID: 8870 RVA: 0x001456DC File Offset: 0x00143ADC
	private IEnumerator end_effect_cr()
	{
		float t = 0f;
		float time = 1f;
		base.transform.SetEulerAngles(null, null, new float?(0f));
		while (t < time)
		{
			t += CupheadTime.Delta;
			base.transform.SetScale(new float?(1f - t / time), new float?(1f - t / time), null);
			yield return null;
		}
		this.OnEffectComplete();
		yield break;
	}

	// Token: 0x04002B4A RID: 11082
	private const float FRAME_TIME = 0.083333336f;

	// Token: 0x04002B4B RID: 11083
	private float frameTime;
}
