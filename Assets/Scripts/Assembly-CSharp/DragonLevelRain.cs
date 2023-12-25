using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FE RID: 1534
public class DragonLevelRain : AbstractPausableComponent
{
	// Token: 0x06001E84 RID: 7812 RVA: 0x00119666 File Offset: 0x00117A66
	public void StartRain()
	{
		base.gameObject.SetActive(true);
		base.StartCoroutine(this.fade_cr());
	}

	// Token: 0x06001E85 RID: 7813 RVA: 0x00119684 File Offset: 0x00117A84
	private IEnumerator fade_cr()
	{
		float alpha = 0f;
		while (alpha < 1f)
		{
			for (int i = 0; i < this.rainRenderers.Length; i++)
			{
				Color color = this.rainRenderers[i].color;
				color.a = alpha;
				this.rainRenderers[i].color = color;
			}
			alpha += CupheadTime.Delta / this.fadeTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002760 RID: 10080
	[SerializeField]
	private float fadeTime;

	// Token: 0x04002761 RID: 10081
	[SerializeField]
	private SpriteRenderer[] rainRenderers;
}
