using System;
using UnityEngine;

// Token: 0x0200093F RID: 2367
public class MapLevelLoaderLadder : MapLevelLoader
{
	// Token: 0x06003761 RID: 14177 RVA: 0x001FD94C File Offset: 0x001FBD4C
	public void EnableShadow(bool enabled)
	{
		this.shadowRenderer.enabled = enabled;
	}

	// Token: 0x06003762 RID: 14178 RVA: 0x001FD95C File Offset: 0x001FBD5C
	private void animationEvent_DownStarted()
	{
		int num = UnityEngine.Random.Range(0, this.smokeRenderers.Length);
		for (int i = 0; i < this.smokeRenderers.Length; i++)
		{
			this.smokeRenderers[i].enabled = (i == num);
		}
	}

	// Token: 0x04003F76 RID: 16246
	[SerializeField]
	private SpriteRenderer shadowRenderer;

	// Token: 0x04003F77 RID: 16247
	[SerializeField]
	private SpriteRenderer[] smokeRenderers;
}
