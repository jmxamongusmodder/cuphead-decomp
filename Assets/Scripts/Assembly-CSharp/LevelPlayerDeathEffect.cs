using System;
using UnityEngine;

// Token: 0x02000A1E RID: 2590
public class LevelPlayerDeathEffect : Effect
{
	// Token: 0x06003D70 RID: 15728 RVA: 0x0021EF88 File Offset: 0x0021D388
	public void Init(Vector2 pos, PlayerId id, bool playerGrounded)
	{
		base.transform.position = pos;
		if (id == PlayerId.PlayerOne || id != PlayerId.PlayerTwo)
		{
			if (PlayerManager.GetPlayer(id).stats.isChalice)
			{
				this.chalice.enabled = true;
			}
			else if (PlayerManager.player1IsMugman)
			{
				this.mugman.enabled = true;
			}
			else
			{
				this.cuphead.enabled = true;
			}
		}
		else if (PlayerManager.GetPlayer(id).stats.isChalice)
		{
			this.chalice.enabled = true;
		}
		else if (PlayerManager.player1IsMugman)
		{
			this.cuphead.enabled = true;
		}
		else
		{
			this.mugman.enabled = true;
		}
		if (playerGrounded)
		{
			this.shadow.enabled = true;
		}
	}

	// Token: 0x06003D71 RID: 15729 RVA: 0x0021F06E File Offset: 0x0021D46E
	public void Init(Vector2 pos)
	{
		base.transform.position = pos;
	}

	// Token: 0x040044B1 RID: 17585
	[SerializeField]
	private SpriteRenderer cuphead;

	// Token: 0x040044B2 RID: 17586
	[SerializeField]
	private SpriteRenderer mugman;

	// Token: 0x040044B3 RID: 17587
	[SerializeField]
	private SpriteRenderer chalice;

	// Token: 0x040044B4 RID: 17588
	[SerializeField]
	private SpriteRenderer shadow;
}
