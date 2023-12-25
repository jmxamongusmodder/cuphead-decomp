using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004F2 RID: 1266
public class BaronessLevelJawbreakerGhost : AbstractCollidableObject
{
	// Token: 0x0600162E RID: 5678 RVA: 0x000C71A0 File Offset: 0x000C55A0
	private void Start()
	{
		base.StartCoroutine(this.deathrotation_cr());
	}

	// Token: 0x0600162F RID: 5679 RVA: 0x000C71B0 File Offset: 0x000C55B0
	private IEnumerator deathrotation_cr()
	{
		float frameTime = 0f;
		float t = 0f;
		for (;;)
		{
			frameTime += CupheadTime.Delta;
			this.deathPosition = new Vector3(base.transform.position.x + Mathf.Sin(t) * this.sinWaveStrength * CupheadTime.Delta * 60f, base.transform.position.y + this.deathSpeed, 0f);
			t += CupheadTime.Delta;
			if (frameTime > 0.083333336f)
			{
				frameTime -= 0.083333336f;
				base.transform.up = (this.deathPosition - base.transform.position).normalized * CupheadTime.Delta;
			}
			if (CupheadTime.Delta != 0f)
			{
				base.transform.position = this.deathPosition;
			}
			if (base.transform.position.y > 540f)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, CupheadTime.Delta);
		}
		this.Die();
		yield break;
	}

	// Token: 0x06001630 RID: 5680 RVA: 0x000C71CB File Offset: 0x000C55CB
	private void Die()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04001F7A RID: 8058
	private const float ROTATE_FRAME_TIME = 0.083333336f;

	// Token: 0x04001F7B RID: 8059
	private Vector3 deathPosition;

	// Token: 0x04001F7C RID: 8060
	private float deathSpeed = 2.3f;

	// Token: 0x04001F7D RID: 8061
	private float sinWaveStrength = 0.4f;
}
