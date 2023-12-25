using System;
using UnityEngine;

// Token: 0x02000B0F RID: 2831
public class EffectLayerRotationRandomizer : MonoBehaviour
{
	// Token: 0x060044B4 RID: 17588 RVA: 0x00246240 File Offset: 0x00244640
	private void Awake()
	{
		if (this.randomizeRotation)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		}
		base.transform.localScale = new Vector3((!this.randomizeXFlip) ? base.transform.localScale.x : ((float)MathUtils.PlusOrMinus()), (!this.randomizeYFlip) ? base.transform.localScale.y : ((float)MathUtils.PlusOrMinus()));
		base.enabled = false;
	}

	// Token: 0x04004A6C RID: 19052
	[SerializeField]
	private bool randomizeRotation = true;

	// Token: 0x04004A6D RID: 19053
	[SerializeField]
	private bool randomizeXFlip = true;

	// Token: 0x04004A6E RID: 19054
	[SerializeField]
	private bool randomizeYFlip = true;
}
