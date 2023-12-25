using System;
using UnityEngine;

// Token: 0x020005BD RID: 1469
public class DicePalaceDominoLevelRandomTile : AbstractMonoBehaviour
{
	// Token: 0x06001C90 RID: 7312 RVA: 0x001050A8 File Offset: 0x001034A8
	public void ChangeTile()
	{
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		if (component == null)
		{
			return;
		}
		component.sprite = this.sprites[UnityEngine.Random.Range(0, this.sprites.Length)];
	}

	// Token: 0x0400257D RID: 9597
	public Sprite[] sprites;
}
