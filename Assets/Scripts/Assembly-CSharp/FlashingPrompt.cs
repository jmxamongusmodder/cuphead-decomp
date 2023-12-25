using System;
using UnityEngine;

// Token: 0x0200045C RID: 1116
public class FlashingPrompt : AbstractMonoBehaviour
{
	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x060010ED RID: 4333 RVA: 0x000A2575 File Offset: 0x000A0975
	protected virtual bool ShouldShow
	{
		get
		{
			return true;
		}
	}

	// Token: 0x060010EE RID: 4334 RVA: 0x000A2578 File Offset: 0x000A0978
	private void Update()
	{
		if (this.ShouldShow)
		{
			this.flashTimer = (this.flashTimer + CupheadTime.Delta) % 1.5f;
			if (this.child != null)
			{
				this.child.SetActive(this.flashTimer < 0.75f);
			}
			else
			{
				this.childGroup.alpha = ((this.flashTimer >= 0.75f) ? 0f : 1f);
			}
		}
		else
		{
			if (this.child != null)
			{
				this.child.SetActive(false);
			}
			else
			{
				this.childGroup.alpha = 0f;
			}
			this.flashTimer = 0f;
		}
	}

	// Token: 0x04001A58 RID: 6744
	private const float FLASH_TIME = 0.75f;

	// Token: 0x04001A59 RID: 6745
	private float flashTimer;

	// Token: 0x04001A5A RID: 6746
	[SerializeField]
	private GameObject child;

	// Token: 0x04001A5B RID: 6747
	[SerializeField]
	private CanvasGroup childGroup;
}
