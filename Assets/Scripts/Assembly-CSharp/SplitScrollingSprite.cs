using System;
using UnityEngine;

// Token: 0x02000B1C RID: 2844
public class SplitScrollingSprite : ScrollingSprite
{
	// Token: 0x060044DF RID: 17631 RVA: 0x00247288 File Offset: 0x00245688
	protected override void Start()
	{
		base.Start();
		foreach (SpriteRenderer spriteRenderer in base.copyRenderers)
		{
			if (!this.ignoreSelfWhenHandlingSplitSprites || !(spriteRenderer.gameObject == base.gameObject))
			{
				foreach (Sprite sprite in this.splitSprites)
				{
					GameObject gameObject = new GameObject(sprite.name);
					SpriteRenderer spriteRenderer2 = gameObject.AddComponent<SpriteRenderer>();
					spriteRenderer2.sprite = sprite;
					spriteRenderer2.sortingLayerID = spriteRenderer.sortingLayerID;
					spriteRenderer2.sortingOrder = spriteRenderer.sortingOrder;
					gameObject.transform.SetParent(spriteRenderer.transform, false);
					gameObject.transform.localPosition = this.splitOffset;
				}
			}
		}
	}

	// Token: 0x04004AAB RID: 19115
	[SerializeField]
	private bool ignoreSelfWhenHandlingSplitSprites;

	// Token: 0x04004AAC RID: 19116
	[SerializeField]
	private Vector2 splitOffset;

	// Token: 0x04004AAD RID: 19117
	[SerializeField]
	private Sprite[] splitSprites;
}
