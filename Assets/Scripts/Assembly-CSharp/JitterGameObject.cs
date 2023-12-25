using System;
using UnityEngine;

// Token: 0x0200045E RID: 1118
public class JitterGameObject : MonoBehaviour
{
	// Token: 0x060010F8 RID: 4344 RVA: 0x000A2665 File Offset: 0x000A0A65
	private void Start()
	{
		this.jitterDelay = 0.083333336f;
		this.tr = base.transform;
		this.startingPosition = this.tr.position;
	}

	// Token: 0x060010F9 RID: 4345 RVA: 0x000A2690 File Offset: 0x000A0A90
	private void Update()
	{
		this.currentJitterDelay -= CupheadTime.Delta;
		if (this.currentJitterDelay <= 0f)
		{
			this.currentJitterDelay = this.jitterDelay;
			float f = UnityEngine.Random.Range(0f, 6.2831855f);
			this.tr.position = this.startingPosition + new Vector3(Mathf.Cos(f), Mathf.Sin(f), 0f) * this.jitterAmplitude;
		}
	}

	// Token: 0x04001A5D RID: 6749
	public float jitterAmplitude = 0.1f;

	// Token: 0x04001A5E RID: 6750
	private float jitterDelay = 0.1f;

	// Token: 0x04001A5F RID: 6751
	private float currentJitterDelay;

	// Token: 0x04001A60 RID: 6752
	private Transform tr;

	// Token: 0x04001A61 RID: 6753
	private Vector3 startingPosition;
}
