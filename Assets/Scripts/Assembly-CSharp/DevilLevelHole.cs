using System;
using System.Collections;

// Token: 0x0200057A RID: 1402
public class DevilLevelHole : AbstractCollidableObject
{
	// Token: 0x1700034F RID: 847
	// (get) Token: 0x06001AB0 RID: 6832 RVA: 0x000F4BCE File Offset: 0x000F2FCE
	// (set) Token: 0x06001AB1 RID: 6833 RVA: 0x000F4BD5 File Offset: 0x000F2FD5
	public static bool PHASE_1_COMPLETE { get; private set; }

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000F4BDD File Offset: 0x000F2FDD
	private void Start()
	{
		DevilLevelHole.PHASE_1_COMPLETE = false;
		base.StartCoroutine(this.check_player_cr());
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000F4BF4 File Offset: 0x000F2FF4
	private IEnumerator check_player_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
			{
				if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
				{
					if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < base.transform.position.y)
					{
						break;
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position.y < base.transform.position.y && PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
				{
					goto Block_6;
				}
			}
			else if (PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position.y < base.transform.position.y)
			{
				goto Block_7;
			}
			yield return null;
		}
		DevilLevelHole.PHASE_1_COMPLETE = true;
		goto IL_1A4;
		Block_6:
		DevilLevelHole.PHASE_1_COMPLETE = true;
		goto IL_1A4;
		Block_7:
		DevilLevelHole.PHASE_1_COMPLETE = true;
		IL_1A4:
		yield break;
	}
}
