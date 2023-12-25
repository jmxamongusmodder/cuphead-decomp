using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005EB RID: 1515
public class DragonLevelBackgroundFlash : MonoBehaviour
{
	// Token: 0x06001E0D RID: 7693 RVA: 0x00114B34 File Offset: 0x00112F34
	public void SetFlash1()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.flashSprite1;
		}
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x00114B78 File Offset: 0x00112F78
	public void SetFlash2()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.flashSprite2;
		}
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00114BBC File Offset: 0x00112FBC
	public void SetNormal()
	{
		List<SpriteRenderer> copyRenderers = this.scrollSprite.copyRenderers;
		for (int i = 0; i < copyRenderers.Count; i++)
		{
			copyRenderers[i].sprite = this.normalSprite;
		}
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x00114BFE File Offset: 0x00112FFE
	private void OnDestroy()
	{
		this.normalSprite = null;
		this.flashSprite1 = null;
		this.flashSprite2 = null;
	}

	// Token: 0x040026D8 RID: 9944
	[SerializeField]
	private Sprite normalSprite;

	// Token: 0x040026D9 RID: 9945
	[SerializeField]
	private Sprite flashSprite1;

	// Token: 0x040026DA RID: 9946
	[SerializeField]
	private Sprite flashSprite2;

	// Token: 0x040026DB RID: 9947
	[SerializeField]
	private ScrollingSprite scrollSprite;
}
