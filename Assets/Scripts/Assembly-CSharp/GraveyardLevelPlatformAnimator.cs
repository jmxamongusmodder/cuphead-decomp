using System;
using UnityEngine;

// Token: 0x020006C6 RID: 1734
public class GraveyardLevelPlatformAnimator : AbstractMonoBehaviour
{
	// Token: 0x060024D8 RID: 9432 RVA: 0x00159414 File Offset: 0x00157814
	private void Start()
	{
		for (int i = 0; i < this.trailSparks.Length; i++)
		{
			this.trailSparks[i].transform.parent = null;
		}
		for (int j = 0; j < this.xPositionBuffer.Length; j++)
		{
			this.xPositionBuffer[j] = base.transform.position.x;
		}
	}

	// Token: 0x060024D9 RID: 9433 RVA: 0x00159484 File Offset: 0x00157884
	private void LateUpdate()
	{
		this.strikeSpark.transform.position = new Vector3(this.strikeSpark.transform.position.x, (float)Level.Current.Ground);
		for (int i = this.xPositionBuffer.Length - 1; i > 0; i--)
		{
			this.xPositionBuffer[i] = this.xPositionBuffer[i - 1];
		}
		this.xPositionBuffer[0] = base.transform.position.x;
		for (int j = 0; j < 3; j++)
		{
			this.trailSparks[j].transform.position = new Vector3(this.xPositionBuffer[j * 2 + 1], (float)Level.Current.Ground);
			this.trailSparks[j].transform.localScale = new Vector3(Mathf.Sign(this.xPositionBuffer[1] - this.xPositionBuffer[0]), 1f);
		}
	}

	// Token: 0x04002D78 RID: 11640
	[SerializeField]
	private SpriteRenderer strikeSpark;

	// Token: 0x04002D79 RID: 11641
	[SerializeField]
	private SpriteRenderer[] trailSparks;

	// Token: 0x04002D7A RID: 11642
	private float[] xPositionBuffer = new float[8];
}
