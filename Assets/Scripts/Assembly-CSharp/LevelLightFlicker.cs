using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000439 RID: 1081
public class LevelLightFlicker : AbstractPausableComponent
{
	// Token: 0x06000FE4 RID: 4068 RVA: 0x0009D5E1 File Offset: 0x0009B9E1
	private void Start()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
	}

	// Token: 0x06000FE5 RID: 4069 RVA: 0x0009D5F0 File Offset: 0x0009B9F0
	private IEnumerator flicker_cr()
	{
		float flickerTime = 0.3f;
		for (;;)
		{
			int counter = 0;
			float waitTime = UnityEngine.Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float t = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			while (counter < this.countUntilPause)
			{
				while (t < flickerTime)
				{
					this.sprite.color = new Color(1f, 1f, 1f, 1f - t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				this.sprite.color = new Color(1f, 1f, 1f, 0f);
				while (t < flickerTime)
				{
					this.sprite.color = new Color(1f, 1f, 1f, t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.sprite.color = new Color(1f, 1f, 1f, 1f);
				counter++;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400197C RID: 6524
	[SerializeField]
	private float fadeWaitMinSecond = 8f;

	// Token: 0x0400197D RID: 6525
	[SerializeField]
	private float fadeWaitMaxSecond = 25f;

	// Token: 0x0400197E RID: 6526
	[SerializeField]
	private int countUntilPause = 3;

	// Token: 0x0400197F RID: 6527
	private SpriteRenderer sprite;
}
