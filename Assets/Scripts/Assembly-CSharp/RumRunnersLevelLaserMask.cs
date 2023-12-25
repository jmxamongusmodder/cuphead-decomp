using System;
using UnityEngine;

// Token: 0x02000791 RID: 1937
public class RumRunnersLevelLaserMask : MonoBehaviour
{
	// Token: 0x06002AEF RID: 10991 RVA: 0x00190B78 File Offset: 0x0018EF78
	public void Setup(int layerID, int lowestLayerOrder)
	{
		foreach (SpriteRenderer spriteRenderer in this.maskRenderers)
		{
			spriteRenderer.sortingLayerID = layerID;
			spriteRenderer.sortingOrder = lowestLayerOrder - 1;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.clearRenderers)
		{
			spriteRenderer2.sortingLayerID = layerID;
			spriteRenderer2.sortingOrder = lowestLayerOrder + 4;
		}
	}

	// Token: 0x040033A4 RID: 13220
	[SerializeField]
	private SpriteRenderer[] maskRenderers;

	// Token: 0x040033A5 RID: 13221
	[SerializeField]
	private SpriteRenderer[] clearRenderers;
}
