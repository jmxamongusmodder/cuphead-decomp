using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006C3 RID: 1731
public class AstralPlaneSpinner : MonoBehaviour
{
	// Token: 0x060024CD RID: 9421 RVA: 0x0015902F File Offset: 0x0015742F
	private void Start()
	{
		base.StartCoroutine(this.rotate_space_bg_cr());
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x00159040 File Offset: 0x00157440
	private IEnumerator rotate_space_bg_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.041666668f);
			this.spaceLayers[0].Rotate(new Vector3(0f, 0f, 0.3f));
			this.spaceLayers[1].Rotate(new Vector3(0f, 0f, 0.5f));
			this.spaceLayers[2].Rotate(new Vector3(0f, 0f, 1f));
		}
		yield break;
	}

	// Token: 0x04002D6E RID: 11630
	[SerializeField]
	private Transform[] spaceLayers;
}
