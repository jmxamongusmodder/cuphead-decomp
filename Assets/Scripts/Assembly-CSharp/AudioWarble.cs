using System;
using UnityEngine;

// Token: 0x020003CF RID: 975
public class AudioWarble : AbstractPausableComponent
{
	// Token: 0x06000CB1 RID: 3249 RVA: 0x0008911C File Offset: 0x0008751C
	public void HandleWarble()
	{
		float[] array = new float[this.warbles.Length];
		float[] array2 = new float[this.warbles.Length];
		float[] array3 = new float[this.warbles.Length];
		float[] array4 = new float[this.warbles.Length];
		for (int i = 0; i < this.warbles.Length; i++)
		{
			array[i] = this.warbles[i].minVal;
			array2[i] = this.warbles[i].maxVal;
			array3[i] = this.warbles[i].warbleTime;
			array4[i] = this.warbles[i].playTime;
		}
		AudioManager.WarbleBGMPitch(this.warbles.Length, array, array2, array3, array4);
	}

	// Token: 0x0400163D RID: 5693
	[SerializeField]
	private AudioWarble.WarbleAttributes[] warbles;

	// Token: 0x020003D0 RID: 976
	[Serializable]
	public class WarbleAttributes
	{
		// Token: 0x0400163E RID: 5694
		public float minVal;

		// Token: 0x0400163F RID: 5695
		public float maxVal;

		// Token: 0x04001640 RID: 5696
		public float warbleTime;

		// Token: 0x04001641 RID: 5697
		public float playTime;
	}
}
