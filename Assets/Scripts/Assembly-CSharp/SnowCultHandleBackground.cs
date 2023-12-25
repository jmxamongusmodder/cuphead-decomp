using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007E4 RID: 2020
public class SnowCultHandleBackground : AbstractPausableComponent
{
	// Token: 0x06002E3F RID: 11839 RVA: 0x001B44B0 File Offset: 0x001B28B0
	private void Update()
	{
		this.fadeTimer += CupheadTime.Delta;
		for (int i = 0; i < this.fadeRenderers.Length; i++)
		{
			this.fadeRenderers[i].color = new Color(1f, 1f, 1f, Mathf.Lerp(this.fadeMin[i], this.fadeMax[i], Mathf.Abs((this.fadeTimer + this.fadeOffset[i] * this.fadePeriod[i]) % this.fadePeriod[i] - this.fadePeriod[i] / 2f)) / (this.fadePeriod[i] / 2f));
		}
		this.glimmerTimer -= CupheadTime.Delta;
		if (this.glimmerTimer <= 0f)
		{
			this.glimmer.Play("Glimmer", 0, 0f);
			this.glimmerTimer += UnityEngine.Random.Range(3.5f, 6.5f);
		}
		this.sparkleTimer -= CupheadTime.Delta;
		if (this.sparkleTimer <= 0f)
		{
			if (this.sparkleList.Count == 0)
			{
				for (int j = 0; j < this.sparkles.Length; j++)
				{
					this.sparkleList.Add(j);
				}
			}
			int index = UnityEngine.Random.Range(0, this.sparkleList.Count);
			this.sparkles[this.sparkleList[index]].Play("Sparkle", 0, 0f);
			this.sparkleList.RemoveAt(index);
			this.sparkleTimer = UnityEngine.Random.Range(0.25f, 0.75f);
		}
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x001B4674 File Offset: 0x001B2A74
	public void CandleGust()
	{
		foreach (Animator animator in this.candles)
		{
			animator.SetTrigger("OnGust");
		}
	}

	// Token: 0x040036B9 RID: 14009
	private float fadeTimer;

	// Token: 0x040036BA RID: 14010
	[SerializeField]
	private SpriteRenderer[] fadeRenderers;

	// Token: 0x040036BB RID: 14011
	[SerializeField]
	private float[] fadePeriod;

	// Token: 0x040036BC RID: 14012
	[SerializeField]
	private float[] fadeOffset;

	// Token: 0x040036BD RID: 14013
	[SerializeField]
	private float[] fadeMin;

	// Token: 0x040036BE RID: 14014
	[SerializeField]
	private float[] fadeMax;

	// Token: 0x040036BF RID: 14015
	[SerializeField]
	private Animator[] candles;

	// Token: 0x040036C0 RID: 14016
	[SerializeField]
	private Animator glimmer;

	// Token: 0x040036C1 RID: 14017
	private float glimmerTimer = 2f;

	// Token: 0x040036C2 RID: 14018
	[SerializeField]
	private Animator[] sparkles;

	// Token: 0x040036C3 RID: 14019
	private float sparkleTimer = 1f;

	// Token: 0x040036C4 RID: 14020
	private List<int> sparkleList = new List<int>();
}
