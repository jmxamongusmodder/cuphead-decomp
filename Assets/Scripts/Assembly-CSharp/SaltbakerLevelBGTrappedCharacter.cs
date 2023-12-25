using System;
using UnityEngine;

// Token: 0x020007C1 RID: 1985
public class SaltbakerLevelBGTrappedCharacter : MonoBehaviour
{
	// Token: 0x06002CE8 RID: 11496 RVA: 0x001A7094 File Offset: 0x001A5494
	public void Setup()
	{
		if (PlayerManager.Multiplayer)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice || PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats.isChalice)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
				{
					this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Cuphead : SaltbakerLevelBGTrappedCharacter.Character.Mugman);
				}
				else
				{
					this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Mugman : SaltbakerLevelBGTrappedCharacter.Character.Cuphead);
				}
			}
			else
			{
				this.charID = SaltbakerLevelBGTrappedCharacter.Character.Chalice;
			}
		}
		else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).stats.isChalice)
		{
			this.charID = ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Cuphead : SaltbakerLevelBGTrappedCharacter.Character.Mugman);
		}
		else
		{
			this.charID = ((PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm != Charm.charm_chalice) ? SaltbakerLevelBGTrappedCharacter.Character.Chalice : ((!PlayerManager.player1IsMugman) ? SaltbakerLevelBGTrappedCharacter.Character.Mugman : SaltbakerLevelBGTrappedCharacter.Character.Cuphead));
		}
		for (int i = 0; i < 3; i++)
		{
			this.characters[i].SetActive(i == (int)this.charID);
		}
	}

	// Token: 0x0400355F RID: 13663
	[SerializeField]
	private GameObject[] characters;

	// Token: 0x04003560 RID: 13664
	private SaltbakerLevelBGTrappedCharacter.Character charID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x04003561 RID: 13665
	private SaltbakerLevelBGTrappedCharacter.Character pOneID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x04003562 RID: 13666
	private SaltbakerLevelBGTrappedCharacter.Character pTwoID = SaltbakerLevelBGTrappedCharacter.Character.None;

	// Token: 0x020007C2 RID: 1986
	private enum Character
	{
		// Token: 0x04003564 RID: 13668
		None = -1,
		// Token: 0x04003565 RID: 13669
		Cuphead,
		// Token: 0x04003566 RID: 13670
		Mugman,
		// Token: 0x04003567 RID: 13671
		Chalice
	}
}
