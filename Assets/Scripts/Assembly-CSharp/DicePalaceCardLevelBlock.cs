using System;
using UnityEngine;

// Token: 0x020005A5 RID: 1445
public class DicePalaceCardLevelBlock : LevelPlatform
{
	// Token: 0x06001BE5 RID: 7141 RVA: 0x000FFE53 File Offset: 0x000FE253
	public override void AddChild(Transform player)
	{
	}

	// Token: 0x06001BE6 RID: 7142 RVA: 0x000FFE55 File Offset: 0x000FE255
	public void DestroyBlock()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040024F1 RID: 9457
	public DicePalaceCardLevelBlock.Suit suit;

	// Token: 0x040024F2 RID: 9458
	public int stopOffsetX;

	// Token: 0x040024F3 RID: 9459
	public DicePalaceCardLevelGridBlock[,] gridBlock;

	// Token: 0x040024F4 RID: 9460
	private float YCheck;

	// Token: 0x040024F5 RID: 9461
	private DamageDealer damageDealer;

	// Token: 0x020005A6 RID: 1446
	public enum Suit
	{
		// Token: 0x040024F7 RID: 9463
		Hearts = 1,
		// Token: 0x040024F8 RID: 9464
		Spades,
		// Token: 0x040024F9 RID: 9465
		Clubs,
		// Token: 0x040024FA RID: 9466
		Diamonds
	}
}
