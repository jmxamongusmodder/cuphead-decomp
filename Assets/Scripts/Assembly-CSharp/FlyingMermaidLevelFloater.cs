using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068A RID: 1674
public class FlyingMermaidLevelFloater : AbstractPausableComponent
{
	// Token: 0x0600234D RID: 9037 RVA: 0x0014B793 File Offset: 0x00149B93
	private void Start()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600234E RID: 9038 RVA: 0x0014B7A4 File Offset: 0x00149BA4
	private IEnumerator loop_cr()
	{
		float waveWidth = 0f;
		if (this.trackingWater != null)
		{
			SpriteRenderer component = this.trackingWater.GetComponent<SpriteRenderer>();
			waveWidth = component.bounds.size.x / 2f;
		}
		float lastY = base.transform.localPosition.y;
		float relativeBobY = 0f;
		float originY = base.transform.localPosition.y;
		float t = 0f;
		float frameTime = 0f;
		for (;;)
		{
			if (!base.enabled)
			{
				yield return null;
			}
			else
			{
				t += CupheadTime.Delta;
				frameTime += CupheadTime.Delta;
				while (frameTime > 0.041666668f)
				{
					frameTime -= 0.041666668f;
					Quaternion rotation = base.transform.rotation;
					float num;
					float num2;
					if (this.trackingWater != null)
					{
						float x = this.trackingWater.transform.position.x;
						float x2 = base.transform.position.x;
						num = 1f - Mathf.Cos((x2 - x) * 2f * (3.1415927f / waveWidth));
						num2 = Mathf.Sin((x2 - x) * 2f * (3.1415927f / waveWidth));
					}
					else
					{
						num = 1f - Mathf.Cos(t * this.bobSpeed * 2f * 3.1415927f);
						num2 = Mathf.Sin(t * this.bobSpeed * 2f * 3.1415927f);
					}
					relativeBobY = num * this.bobAmount;
					originY += base.transform.localPosition.y - lastY;
					rotation = Quaternion.AngleAxis(this.defaultRotation + num2 * this.rotateAmount, Vector3.forward);
					base.transform.SetPosition(null, new float?(originY + relativeBobY), null);
					base.transform.rotation = rotation;
					lastY = base.transform.localPosition.y;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x04002BEC RID: 11244
	private const float BOB_FRAME_TIME = 0.041666668f;

	// Token: 0x04002BED RID: 11245
	public float bobAmount;

	// Token: 0x04002BEE RID: 11246
	public float rotateAmount;

	// Token: 0x04002BEF RID: 11247
	public float defaultRotation;

	// Token: 0x04002BF0 RID: 11248
	public GameObject trackingWater;

	// Token: 0x04002BF1 RID: 11249
	public float bobSpeed;
}
