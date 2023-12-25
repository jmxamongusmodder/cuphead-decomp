using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007DB RID: 2011
public class SlimeLevelLightRay : AbstractPausableComponent
{
	// Token: 0x06002DE7 RID: 11751 RVA: 0x001B0FDA File Offset: 0x001AF3DA
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002DE8 RID: 11752 RVA: 0x001B0FF0 File Offset: 0x001AF3F0
	private IEnumerator main_cr()
	{
		bool fadingOut = this.startVisible;
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		if (!this.startVisible)
		{
			sprite.color = new Color(1f, 1f, 1f, 0f);
		}
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.holdTime);
			if (fadingOut)
			{
				float t = 0f;
				while (t < this.fadeTime)
				{
					sprite.color = new Color(1f, 1f, 1f, 1f - t / this.fadeTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				sprite.color = new Color(1f, 1f, 1f, 0f);
			}
			else
			{
				float t2 = 0f;
				while (t2 < this.fadeTime)
				{
					sprite.color = new Color(1f, 1f, 1f, t2 / this.fadeTime);
					t2 += CupheadTime.Delta;
					yield return null;
				}
				sprite.color = new Color(1f, 1f, 1f, 1f);
			}
			fadingOut = !fadingOut;
		}
		yield break;
	}

	// Token: 0x04003668 RID: 13928
	[SerializeField]
	private float holdTime;

	// Token: 0x04003669 RID: 13929
	[SerializeField]
	private float fadeTime;

	// Token: 0x0400366A RID: 13930
	[SerializeField]
	private bool startVisible;
}
