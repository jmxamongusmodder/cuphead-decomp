using System;
using UnityEngine;

// Token: 0x02000658 RID: 1624
public class FlyingCowboyLevelOverlayToggle : MonoBehaviour
{
	// Token: 0x060021DA RID: 8666 RVA: 0x0013B7B4 File Offset: 0x00139BB4
	private void Start()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in this.overlayRenderers)
		{
			if (UnityEngine.Random.value < this.overlayProbability)
			{
				spriteRenderer.enabled = true;
				spriteRenderer.sortingOrder = component.sortingOrder + 1;
			}
			else
			{
				spriteRenderer.enabled = false;
			}
		}
	}

	// Token: 0x04002A92 RID: 10898
	[SerializeField]
	[Range(0f, 1f)]
	private float overlayProbability;

	// Token: 0x04002A93 RID: 10899
	[SerializeField]
	private SpriteRenderer[] overlayRenderers;
}
