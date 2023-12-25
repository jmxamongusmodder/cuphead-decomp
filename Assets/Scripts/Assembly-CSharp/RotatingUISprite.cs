using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B16 RID: 2838
public class RotatingUISprite : MonoBehaviour
{
	// Token: 0x060044C9 RID: 17609 RVA: 0x0024699C File Offset: 0x00244D9C
	private void Start()
	{
		base.StartCoroutine(this.rotate_cr());
	}

	// Token: 0x060044CA RID: 17610 RVA: 0x002469AC File Offset: 0x00244DAC
	private IEnumerator rotate_cr()
	{
		for (;;)
		{
			base.transform.Rotate(new Vector3(0f, 0f, this.speed / (float)this.frameRate));
			yield return CupheadTime.WaitForSeconds(this, 1f / (float)this.frameRate);
		}
		yield break;
	}

	// Token: 0x04004A83 RID: 19075
	[SerializeField]
	private float speed = 1f;

	// Token: 0x04004A84 RID: 19076
	[SerializeField]
	private int frameRate = 12;
}
