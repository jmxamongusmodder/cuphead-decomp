using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000556 RID: 1366
public class ClownLevelBackgroundLights : AbstractPausableComponent
{
	// Token: 0x0600197F RID: 6527 RVA: 0x000E7754 File Offset: 0x000E5B54
	private void Start()
	{
		this.lightVersion = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.lights_cr());
		if (this.occasionalFlicker)
		{
			base.StartCoroutine(this.flicker_cr());
		}
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x000E7788 File Offset: 0x000E5B88
	private IEnumerator lights_cr()
	{
		for (;;)
		{
			this.fadeTime = UnityEngine.Random.Range(this.fadeDurationMin, this.fadeDurationMax);
			this.getSecond = UnityEngine.Random.Range(this.waitMinSecond, this.waitMaxSecond);
			if (this.fadeIn)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					this.lightVersion.color = new Color(1f, 1f, 1f, t / this.fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.lightVersion.color = new Color(1f, 1f, 1f, 1f);
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					this.lightVersion.color = new Color(1f, 1f, 1f, 1f - t2 / this.fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
				this.lightVersion.color = new Color(1f, 1f, 1f, 0f);
				yield return CupheadTime.WaitForSeconds(this, this.getSecond);
			}
			this.fadeIn = !this.fadeIn;
		}
		yield break;
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x000E77A4 File Offset: 0x000E5BA4
	private IEnumerator flicker_cr()
	{
		for (;;)
		{
			float waitTime = UnityEngine.Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float flickerTime = UnityEngine.Random.Range(this.flickerMinTime, this.flickerMaxTime);
			float t = 0f;
			while (t < flickerTime)
			{
				this.fadeTime = 0.08f;
				t += CupheadTime.Delta;
				yield return null;
			}
			this.fadeTime = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
		}
		yield break;
	}

	// Token: 0x04002298 RID: 8856
	private float waitMinSecond;

	// Token: 0x04002299 RID: 8857
	private float waitMaxSecond = 1f;

	// Token: 0x0400229A RID: 8858
	private float fadeDurationMin = 0.5f;

	// Token: 0x0400229B RID: 8859
	private float fadeDurationMax = 2f;

	// Token: 0x0400229C RID: 8860
	[SerializeField]
	private bool occasionalFlicker;

	// Token: 0x0400229D RID: 8861
	private float fadeWaitMinSecond = 5f;

	// Token: 0x0400229E RID: 8862
	private float fadeWaitMaxSecond = 10f;

	// Token: 0x0400229F RID: 8863
	private float flickerMinTime = 1f;

	// Token: 0x040022A0 RID: 8864
	private float flickerMaxTime = 5f;

	// Token: 0x040022A1 RID: 8865
	private SpriteRenderer lightVersion;

	// Token: 0x040022A2 RID: 8866
	private float getSecond;

	// Token: 0x040022A3 RID: 8867
	private float fadeTime;

	// Token: 0x040022A4 RID: 8868
	private bool fadeIn;
}
