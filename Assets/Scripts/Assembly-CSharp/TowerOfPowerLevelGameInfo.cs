using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000802 RID: 2050
public class TowerOfPowerLevelGameInfo : AbstractMonoBehaviour
{
	// Token: 0x17000418 RID: 1048
	// (get) Token: 0x06002F5F RID: 12127 RVA: 0x001C1620 File Offset: 0x001BFA20
	public static TowerOfPowerLevelGameInfo GameInfo
	{
		get
		{
			if (TowerOfPowerLevelGameInfo.gameInfo == null)
			{
				TowerOfPowerLevelGameInfo.gameInfo = new GameObject
				{
					name = "GameInfo"
				}.AddComponent<TowerOfPowerLevelGameInfo>();
			}
			return TowerOfPowerLevelGameInfo.gameInfo;
		}
	}

	// Token: 0x06002F60 RID: 12128 RVA: 0x001C165E File Offset: 0x001BFA5E
	protected override void Awake()
	{
		base.Awake();
		TowerOfPowerLevelGameInfo.gameInfo = this;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06002F61 RID: 12129 RVA: 0x001C1682 File Offset: 0x001BFA82
	public void CleanUp()
	{
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0;
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002F62 RID: 12130 RVA: 0x001C16BB File Offset: 0x001BFABB
	public static void ResetTowerOfPower()
	{
		TowerOfPowerLevelGameInfo.TURN_COUNTER = 0;
		TowerOfPowerLevelGameInfo.CURRENT_TURN = 0;
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = null;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = null;
	}

	// Token: 0x06002F63 RID: 12131 RVA: 0x001C16F0 File Offset: 0x001BFAF0
	public static void InitAddedPlayer(PlayerId playerId, int startingToken)
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN == 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 3;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 3;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = 0f;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount = startingToken;
		}
		else
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 1;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 0;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = 0f;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount = 0;
		}
	}

	// Token: 0x06002F64 RID: 12132 RVA: 0x001C1788 File Offset: 0x001BFB88
	public static void ResetWeapons(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			return;
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].basePrimaryWeapon;
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].baseSecondaryWeapon;
		playerLoadout.super = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseSuper;
		playerLoadout.charm = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm;
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x06002F65 RID: 12133 RVA: 0x001C1804 File Offset: 0x001BFC04
	public static void InitEquipment(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].basePrimaryWeapon = playerLoadout.primaryWeapon;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].baseSecondaryWeapon = playerLoadout.secondaryWeapon;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseSuper = playerLoadout.super;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm = playerLoadout.charm;
	}

	// Token: 0x06002F66 RID: 12134 RVA: 0x001C1884 File Offset: 0x001BFC84
	public static void SetPlayersStats(PlayerId playerId)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId] = new PlayersStatsBossesHub();
		}
		PlayerStatsManager stats = PlayerManager.GetPlayer(playerId).stats;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = stats.Health;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].SuperCharge = stats.SuperMeter;
	}

	// Token: 0x06002F67 RID: 12135 RVA: 0x001C18DC File Offset: 0x001BFCDC
	public static void AddToken()
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null)
		{
			return;
		}
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP > 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount++;
		}
		if (PlayerManager.Multiplayer && TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP > 0)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount++;
		}
	}

	// Token: 0x06002F68 RID: 12136 RVA: 0x001C194C File Offset: 0x001BFD4C
	public static void ReduceToken()
	{
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount--;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount--;
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount = Mathf.Max(0, TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount);
		}
	}

	// Token: 0x06002F69 RID: 12137 RVA: 0x001C19C7 File Offset: 0x001BFDC7
	public static void ReduceToken(int playerNum)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum] == null || TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount == 0)
		{
			return;
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount--;
	}

	// Token: 0x06002F6A RID: 12138 RVA: 0x001C19FC File Offset: 0x001BFDFC
	public static void SetDefaultToken(int defaultTokenCount)
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null)
		{
			TowerOfPowerLevelGameInfo.PLAYER_STATS[0] = new PlayersStatsBossesHub();
		}
		TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount = defaultTokenCount;
		if (PlayerManager.Multiplayer)
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[1] == null)
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[1] = new PlayersStatsBossesHub();
			}
			TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount = defaultTokenCount;
		}
	}

	// Token: 0x06002F6B RID: 12139 RVA: 0x001C1A60 File Offset: 0x001BFE60
	public static bool IsTokenLeft()
	{
		if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] == null && TowerOfPowerLevelGameInfo.PLAYER_STATS[1] == null)
		{
			return false;
		}
		int tokenCount = TowerOfPowerLevelGameInfo.PLAYER_STATS[0].tokenCount;
		int num = (!PlayerManager.Multiplayer) ? 0 : TowerOfPowerLevelGameInfo.PLAYER_STATS[1].tokenCount;
		return tokenCount > 0 || num > 0;
	}

	// Token: 0x06002F6C RID: 12140 RVA: 0x001C1ABF File Offset: 0x001BFEBF
	public static bool IsTokenLeft(int playerNum)
	{
		return TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum] != null && TowerOfPowerLevelGameInfo.PLAYER_STATS[playerNum].tokenCount != 0;
	}

	// Token: 0x06002F6D RID: 12141 RVA: 0x001C1AE1 File Offset: 0x001BFEE1
	private void OnDestroy()
	{
		TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.ResetWeapons(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x0400382E RID: 14382
	private static TowerOfPowerLevelGameInfo gameInfo;

	// Token: 0x0400382F RID: 14383
	public static List<Levels> allStageSpaces = new List<Levels>();

	// Token: 0x04003830 RID: 14384
	public static Level.Mode[] difficultyByBossIndex;

	// Token: 0x04003831 RID: 14385
	public static List<string> SlotOne = new List<string>();

	// Token: 0x04003832 RID: 14386
	public static List<string> SlotTwo = new List<string>();

	// Token: 0x04003833 RID: 14387
	public static List<string> SlotThree = new List<string>();

	// Token: 0x04003834 RID: 14388
	public static List<string> SlotThreeChalice = new List<string>();

	// Token: 0x04003835 RID: 14389
	public static List<string> SlotFour = new List<string>();

	// Token: 0x04003836 RID: 14390
	public static List<string> SlotFourChalice = new List<string>();

	// Token: 0x04003837 RID: 14391
	public static Level.Mode baseDifficulty;

	// Token: 0x04003838 RID: 14392
	public static int CURRENT_TURN;

	// Token: 0x04003839 RID: 14393
	public static int TURN_COUNTER;

	// Token: 0x0400383A RID: 14394
	public static int MIN_RANK_NEED_TO_GET_TOKEN;

	// Token: 0x0400383B RID: 14395
	public static PlayersStatsBossesHub[] PLAYER_STATS = new PlayersStatsBossesHub[2];
}
