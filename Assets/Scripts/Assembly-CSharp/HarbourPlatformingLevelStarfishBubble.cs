using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D2 RID: 2258
public class HarbourPlatformingLevelStarfishBubble : Effect
{
	// Token: 0x060034DD RID: 13533 RVA: 0x001EBDB7 File Offset: 0x001EA1B7
	private void Start()
	{
		this.sinWaveStrength = UnityEngine.Random.Range(0.4f, 0.9f);
		this.speed = UnityEngine.Random.Range(50f, 100f);
		base.StartCoroutine(this.deathrotation_cr());
	}

	// Token: 0x060034DE RID: 13534 RVA: 0x001EBDF0 File Offset: 0x001EA1F0
	private IEnumerator deathrotation_cr()
	{
		float totalTime = 0f;
		float maxTime = UnityEngine.Random.Range(4f, 7f);
		float t = UnityEngine.Random.Range(0f, 6.2831855f);
		while (totalTime < maxTime)
		{
			totalTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(base.transform.position.x + Mathf.Sin(t) * this.sinWaveStrength * CupheadTime.Delta * 60f), null, null);
			base.transform.AddPosition(0f, this.speed * CupheadTime.Delta, 0f);
			yield return null;
		}
		base.animator.Play("Pop");
		yield return null;
		yield break;
	}

	// Token: 0x04003D06 RID: 15622
	private const float ROTATE_FRAME_TIME = 0.083333336f;

	// Token: 0x04003D07 RID: 15623
	private Vector3 pos;

	// Token: 0x04003D08 RID: 15624
	private float speed;

	// Token: 0x04003D09 RID: 15625
	private float sinWaveStrength;
}
