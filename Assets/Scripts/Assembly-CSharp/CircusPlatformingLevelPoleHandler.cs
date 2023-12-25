using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008A9 RID: 2217
public class CircusPlatformingLevelPoleHandler : AbstractPausableComponent
{
	// Token: 0x060033AF RID: 13231 RVA: 0x001E052D File Offset: 0x001DE92D
	private void Start()
	{
		this.SetupBots();
	}

	// Token: 0x060033B0 RID: 13232 RVA: 0x001E0538 File Offset: 0x001DE938
	private void SetupBots()
	{
		this.poleBots = new List<CircusPlatformingLevelPoleBot>();
		float y = this.poleBot.GetComponent<BoxCollider2D>().size.y;
		for (int i = 0; i < this.poleBotCount; i++)
		{
			Vector2 v = new Vector2(this.poleRoot.transform.position.x, this.poleRoot.transform.position.y + y * 1.38f * (float)i);
			this.poleBots.Add(this.poleBot.Spawn(v));
		}
		base.StartCoroutine(this.check_to_slide_cr());
	}

	// Token: 0x060033B1 RID: 13233 RVA: 0x001E05F0 File Offset: 0x001DE9F0
	private IEnumerator check_to_slide_cr()
	{
		int indexToSlide = 1000;
		for (;;)
		{
			for (int i = this.poleBots.Count - 1; i >= 0; i--)
			{
				if (this.poleBots[i].isDying)
				{
					this.poleBots.RemoveAt(i);
					indexToSlide = i;
					break;
				}
				if (i >= indexToSlide)
				{
					while (this.poleBots[i].isSliding)
					{
						yield return null;
					}
					this.poleBots[i].SlideDown();
				}
				if (i == 0)
				{
					indexToSlide = 1000;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003BFC RID: 15356
	[SerializeField]
	private int poleBotCount;

	// Token: 0x04003BFD RID: 15357
	[SerializeField]
	private Transform poleRoot;

	// Token: 0x04003BFE RID: 15358
	[SerializeField]
	private CircusPlatformingLevelPoleBot poleBot;

	// Token: 0x04003BFF RID: 15359
	private List<CircusPlatformingLevelPoleBot> poleBots;
}
