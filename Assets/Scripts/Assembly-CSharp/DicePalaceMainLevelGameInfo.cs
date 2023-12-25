using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D4 RID: 1492
public class DicePalaceMainLevelGameInfo : AbstractMonoBehaviour
{
	// Token: 0x1700036D RID: 877
	// (get) Token: 0x06001D59 RID: 7513 RVA: 0x0010D010 File Offset: 0x0010B410
	public static DicePalaceMainLevelGameInfo GameInfo
	{
		get
		{
			if (DicePalaceMainLevelGameInfo.gameInfo == null)
			{
				DicePalaceMainLevelGameInfo.gameInfo = new GameObject
				{
					name = "GameInfo"
				}.AddComponent<DicePalaceMainLevelGameInfo>();
			}
			return DicePalaceMainLevelGameInfo.gameInfo;
		}
	}

	// Token: 0x06001D5A RID: 7514 RVA: 0x0010D04E File Offset: 0x0010B44E
	protected override void Awake()
	{
		base.Awake();
		DicePalaceMainLevelGameInfo.gameInfo = this;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
		DicePalaceMainLevelGameInfo.SAFE_INDEXES = new List<int>();
		DicePalaceMainLevelGameInfo.ChooseHearts();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06001D5B RID: 7515 RVA: 0x0010D078 File Offset: 0x0010B478
	public void CleanUp()
	{
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear();
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0;
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
		DicePalaceMainLevelGameInfo.ChooseHearts();
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = false;
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001D5C RID: 7516 RVA: 0x0010D0C9 File Offset: 0x0010B4C9
	public static void CleanUpRetry()
	{
		DicePalaceMainLevelGameInfo.SAFE_INDEXES.Clear();
		DicePalaceMainLevelGameInfo.TURN_COUNTER = 0;
		DicePalaceMainLevelGameInfo.PLAYER_SPACES_MOVED = 0;
		DicePalaceMainLevelGameInfo.ChooseHearts();
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = null;
		DicePalaceMainLevelGameInfo.PLAYED_INTRO_SFX = false;
		DicePalaceMainLevelGameInfo.CHALICE_PLAYER = -1;
		DicePalaceMainLevelGameInfo.IS_FIRST_ENTRY = true;
	}

	// Token: 0x06001D5D RID: 7517 RVA: 0x0010D104 File Offset: 0x0010B504
	private static void ChooseHearts()
	{
		DicePalaceMainLevelGameInfo.HEART_INDEXES[0] = UnityEngine.Random.Range(0, 3);
		DicePalaceMainLevelGameInfo.HEART_INDEXES[1] = UnityEngine.Random.Range(4, 7);
		DicePalaceMainLevelGameInfo.HEART_INDEXES[2] = UnityEngine.Random.Range(8, 11);
	}

	// Token: 0x06001D5E RID: 7518 RVA: 0x0010D134 File Offset: 0x0010B534
	public static void SetPlayersStats()
	{
		if (DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS == null)
		{
			DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS = new PlayersStatsBossesHub();
		}
		PlayerStatsManager stats = PlayerManager.GetPlayer(PlayerId.PlayerOne).stats;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHP = stats.HealerHP;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPReceived = stats.HealerHPReceived;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.healerHPCounter = stats.HealerHPCounter;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.HP = stats.Health;
		DicePalaceMainLevelGameInfo.PLAYER_ONE_STATS.SuperCharge = stats.SuperMeter;
		if (PlayerManager.Multiplayer)
		{
			if (DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS == null)
			{
				DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS = new PlayersStatsBossesHub();
			}
			PlayerStatsManager stats2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo).stats;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHP = stats2.HealerHP;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPReceived = stats2.HealerHPReceived;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.healerHPCounter = stats2.HealerHPCounter;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.HP = stats2.Health;
			DicePalaceMainLevelGameInfo.PLAYER_TWO_STATS.SuperCharge = stats2.SuperMeter;
		}
	}

	// Token: 0x0400263A RID: 9786
	private static DicePalaceMainLevelGameInfo gameInfo;

	// Token: 0x0400263B RID: 9787
	public static int TURN_COUNTER;

	// Token: 0x0400263C RID: 9788
	public static int PLAYER_SPACES_MOVED;

	// Token: 0x0400263D RID: 9789
	public static List<int> SAFE_INDEXES;

	// Token: 0x0400263E RID: 9790
	public static int[] HEART_INDEXES = new int[3];

	// Token: 0x0400263F RID: 9791
	public static PlayersStatsBossesHub PLAYER_ONE_STATS;

	// Token: 0x04002640 RID: 9792
	public static PlayersStatsBossesHub PLAYER_TWO_STATS;

	// Token: 0x04002641 RID: 9793
	public static bool PLAYED_INTRO_SFX;

	// Token: 0x04002642 RID: 9794
	public static bool IS_FIRST_ENTRY = true;

	// Token: 0x04002643 RID: 9795
	public static int CHALICE_PLAYER = -1;
}
