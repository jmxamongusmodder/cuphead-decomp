using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000662 RID: 1634
public class FlyingGenieLevelBackgroundShade : AbstractPausableComponent
{
	// Token: 0x06002208 RID: 8712 RVA: 0x0013D0AA File Offset: 0x0013B4AA
	private void Start()
	{
		base.FrameDelayedCallback(new Action(this.GetSprites), 1);
	}

	// Token: 0x06002209 RID: 8713 RVA: 0x0013D0C0 File Offset: 0x0013B4C0
	private void GetSprites()
	{
		this.darkClones = this.darkSprite.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
		base.StartCoroutine(this.fade_sprite_cr());
	}

	// Token: 0x0600220A RID: 8714 RVA: 0x0013D0EC File Offset: 0x0013B4EC
	private IEnumerator fade_sprite_cr()
	{
		for (;;)
		{
			float t = FlyingGenieLevel.mainTimer;
			float period = 12f;
			float shade = (Mathf.Sin(t * 3.1415927f * 2f / period) + 1f) / 2f;
			shade = Mathf.Lerp(this.fullSunOpacity, this.fullShadeOpactity, shade);
			foreach (SpriteRenderer spriteRenderer in this.darkClones)
			{
				spriteRenderer.color = new Color(spriteRenderer.GetComponent<SpriteRenderer>().color.r, spriteRenderer.GetComponent<SpriteRenderer>().color.g, spriteRenderer.GetComponent<SpriteRenderer>().color.b, shade);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002AB6 RID: 10934
	[SerializeField]
	private SpriteRenderer darkSprite;

	// Token: 0x04002AB7 RID: 10935
	private SpriteRenderer[] darkClones;

	// Token: 0x04002AB8 RID: 10936
	[SerializeField]
	private float fullSunOpacity;

	// Token: 0x04002AB9 RID: 10937
	[SerializeField]
	private float fullShadeOpactity = 1f;
}
