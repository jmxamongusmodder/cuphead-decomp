using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200040B RID: 1035
[Serializable]
public class PlayerData
{
	// Token: 0x06000E82 RID: 3714 RVA: 0x00094118 File Offset: 0x00092518
	public PlayerData()
	{
		if (string.IsNullOrEmpty(PlayerData.emptyDialoguerState))
		{
			Dialoguer.Initialize();
			PlayerData.emptyDialoguerState = Dialoguer.GetGlobalVariablesState();
		}
		this.dialoguerState = PlayerData.emptyDialoguerState;
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06000E83 RID: 3715 RVA: 0x000941B3 File Offset: 0x000925B3
	// (set) Token: 0x06000E84 RID: 3716 RVA: 0x000941C9 File Offset: 0x000925C9
	public static int CurrentSaveFileIndex
	{
		get
		{
			return Mathf.Clamp(PlayerData._CurrentSaveFileIndex, 0, PlayerData.SAVE_FILE_KEYS.Length - 1);
		}
		set
		{
			PlayerData._CurrentSaveFileIndex = Mathf.Clamp(value, 0, PlayerData.SAVE_FILE_KEYS.Length - 1);
			PlayerData.Data.LoadDialogueVariables();
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06000E85 RID: 3717 RVA: 0x000941EA File Offset: 0x000925EA
	// (set) Token: 0x06000E86 RID: 3718 RVA: 0x000941F1 File Offset: 0x000925F1
	public static bool Initialized
	{
		get
		{
			return PlayerData._initialized;
		}
		private set
		{
			PlayerData._initialized = value;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06000E87 RID: 3719 RVA: 0x000941F9 File Offset: 0x000925F9
	public static PlayerData Data
	{
		get
		{
			return PlayerData.GetDataForSlot(PlayerData.CurrentSaveFileIndex);
		}
	}

	// Token: 0x06000E88 RID: 3720 RVA: 0x00094208 File Offset: 0x00092608
	public static PlayerData GetDataForSlot(int slot)
	{
		if (PlayerData._saveFiles == null || PlayerData._saveFiles.Length != PlayerData.SAVE_FILE_KEYS.Length)
		{
			PlayerData._saveFiles = new PlayerData[PlayerData.SAVE_FILE_KEYS.Length];
			for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
			{
				PlayerData._saveFiles[i] = new PlayerData();
			}
		}
		if (PlayerData._saveFiles[slot].curseCharmPuzzleOrder == null || PlayerData._saveFiles[slot].curseCharmPuzzleOrder.Length == 0)
		{
			PlayerData._saveFiles[slot].CreateCursePuzzleVariables();
		}
		return PlayerData._saveFiles[slot];
	}

	// Token: 0x06000E89 RID: 3721 RVA: 0x000942A0 File Offset: 0x000926A0
	private void CreateCursePuzzleVariables()
	{
		List<int> list = new List<int>
		{
			0,
			1,
			2,
			3,
			4,
			5,
			6,
			7
		};
		this.curseCharmPuzzleOrder = new int[3];
		for (int i = 0; i < this.curseCharmPuzzleOrder.Length; i++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			this.curseCharmPuzzleOrder[i] = list[index];
			list.Remove(list[index]);
		}
	}

	// Token: 0x06000E8A RID: 3722 RVA: 0x0009433E File Offset: 0x0009273E
	public static void ClearSlot(int slot)
	{
		if (PlayerData._saveFiles == null || PlayerData._saveFiles.Length != PlayerData.SAVE_FILE_KEYS.Length)
		{
			return;
		}
		PlayerData.ResetDialoguer();
		PlayerData._saveFiles[slot] = new PlayerData();
		PlayerData.Save(slot);
	}

	// Token: 0x06000E8B RID: 3723 RVA: 0x00094378 File Offset: 0x00092778
	public static void Init(PlayerData.PlayerDataInitHandler handler)
	{
		PlayerData._saveFiles = new PlayerData[PlayerData.SAVE_FILE_KEYS.Length];
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			PlayerData._saveFiles[i] = new PlayerData();
		}
		PlayerData._playerDatatInitHandler = handler;
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		PlayerId player = PlayerId.PlayerOne;
		if (PlayerData.<>f__mg$cache0 == null)
		{
			PlayerData.<>f__mg$cache0 = new InitializeCloudStoreHandler(PlayerData.OnCloudStorageInitialized);
		}
		@interface.InitializeCloudStorage(player, PlayerData.<>f__mg$cache0);
	}

	// Token: 0x06000E8C RID: 3724 RVA: 0x000943ED File Offset: 0x000927ED
	private void LoadDialogueVariables()
	{
		Dialoguer.Initialize();
		Dialoguer.EndDialogue();
		Dialoguer.SetGlobalVariablesState(this.dialoguerState);
	}

	// Token: 0x06000E8D RID: 3725 RVA: 0x00094404 File Offset: 0x00092804
	private static void OnCloudStorageInitialized(bool success)
	{
		if (!success)
		{
			PlayerData._playerDatatInitHandler(false);
			return;
		}
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		string[] save_FILE_KEYS = PlayerData.SAVE_FILE_KEYS;
		if (PlayerData.<>f__mg$cache1 == null)
		{
			PlayerData.<>f__mg$cache1 = new LoadCloudDataHandler(PlayerData.OnLoaded);
		}
		@interface.LoadCloudData(save_FILE_KEYS, PlayerData.<>f__mg$cache1);
	}

	// Token: 0x06000E8E RID: 3726 RVA: 0x00094454 File Offset: 0x00092854
	private static void OnLoaded(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			global::Debug.LogError("[PlayerData] LOAD FAILED", null);
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] save_FILE_KEYS = PlayerData.SAVE_FILE_KEYS;
			if (PlayerData.<>f__mg$cache2 == null)
			{
				PlayerData.<>f__mg$cache2 = new LoadCloudDataHandler(PlayerData.OnLoaded);
			}
			@interface.LoadCloudData(save_FILE_KEYS, PlayerData.<>f__mg$cache2);
			return;
		}
		if (result == CloudLoadResult.NoData)
		{
			global::Debug.LogError("[PlayerData] No data. Saving default data to cloud", null);
			PlayerData.SaveAll();
			return;
		}
		bool flag = false;
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			if (data[i] != null)
			{
				PlayerData playerData = null;
				try
				{
					playerData = JsonUtility.FromJson<PlayerData>(data[i]);
					if (playerData != null && !playerData.coinManager.hasMigratedCoins)
					{
						playerData = PlayerData.Migrate(playerData);
						flag = true;
					}
				}
				catch (ArgumentException ex)
				{
					global::Debug.LogError("Unable to parse player data. " + ex.StackTrace, null);
				}
				if (playerData == null)
				{
					global::Debug.LogError("[PlayerData] Data could not be unserialized for key: " + PlayerData.SAVE_FILE_KEYS[i], null);
				}
				else
				{
					PlayerData._saveFiles[i] = playerData;
				}
			}
		}
		PlayerData.Initialized = true;
		if (flag)
		{
			PlayerData.SaveAll();
		}
		if (PlayerData._playerDatatInitHandler != null)
		{
			PlayerData._playerDatatInitHandler(true);
			PlayerData._playerDatatInitHandler = null;
		}
	}

	// Token: 0x06000E8F RID: 3727 RVA: 0x00094590 File Offset: 0x00092990
	public static PlayerData Migrate(PlayerData playerData)
	{
		for (int i = 0; i < playerData.coinManager.LevelsAndCoins.Count; i++)
		{
			PlayerData.PlayerCoinManager.LevelAndCoins levelAndCoins = new PlayerData.PlayerCoinManager.LevelAndCoins();
			levelAndCoins.level = playerData.coinManager.LevelsAndCoins[i].level;
			playerData.coinManager.LevelsAndCoins[i] = levelAndCoins;
		}
		for (int j = 0; j < playerData.coinManager.coins.Count; j++)
		{
			string coinID = playerData.coinManager.coins[j].coinID;
			bool flag = false;
			for (int k = 0; k < PlayerData.platformingCoinIDs.Length; k++)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = playerData.coinManager.LevelsAndCoins;
				int index = -1;
				for (int l = 0; l < levelsAndCoins.Count; l++)
				{
					if (levelsAndCoins[l].level == PlayerData.platformingCoinIDs[k].levelId)
					{
						index = l;
					}
				}
				for (int m = 0; m < PlayerData.platformingCoinIDs[k].coinIds.Length; m++)
				{
					string coinID2 = PlayerData.platformingCoinIDs[k].coinIds[m][0];
					for (int n = 0; n < PlayerData.platformingCoinIDs[k].coinIds[m].Length; n++)
					{
						if (coinID == PlayerData.platformingCoinIDs[k].coinIds[m][n])
						{
							playerData.coinManager.coins[j].coinID = coinID2;
							flag = true;
							switch (m)
							{
							case 0:
								levelsAndCoins[index].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins[index].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins[index].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins[index].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins[index].Coin5Collected = true;
								break;
							}
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
		}
		playerData.coinManager.hasMigratedCoins = true;
		return playerData;
	}

	// Token: 0x06000E90 RID: 3728 RVA: 0x000947F3 File Offset: 0x00092BF3
	private static string GetSaveFileKey(int fileIndex)
	{
		return PlayerData.SAVE_FILE_KEYS[fileIndex];
	}

	// Token: 0x06000E91 RID: 3729 RVA: 0x000947FC File Offset: 0x00092BFC
	private static void Save(int fileIndex)
	{
		PlayerData._saveFiles[fileIndex].dialoguerState = Dialoguer.GetGlobalVariablesState();
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		dictionary[PlayerData.SAVE_FILE_KEYS[fileIndex]] = JsonUtility.ToJson(PlayerData._saveFiles[fileIndex]);
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		IDictionary<string, string> data = dictionary;
		if (PlayerData.<>f__mg$cache3 == null)
		{
			PlayerData.<>f__mg$cache3 = new SaveCloudDataHandler(PlayerData.OnSaved);
		}
		@interface.SaveCloudData(data, PlayerData.<>f__mg$cache3);
	}

	// Token: 0x06000E92 RID: 3730 RVA: 0x00094868 File Offset: 0x00092C68
	private static void SaveAll()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			dictionary[PlayerData.SAVE_FILE_KEYS[i]] = JsonUtility.ToJson(PlayerData._saveFiles[i]);
		}
		OnlineInterface @interface = OnlineManager.Instance.Interface;
		IDictionary<string, string> data = dictionary;
		if (PlayerData.<>f__mg$cache4 == null)
		{
			PlayerData.<>f__mg$cache4 = new SaveCloudDataHandler(PlayerData.OnSavedAll);
		}
		@interface.SaveCloudData(data, PlayerData.<>f__mg$cache4);
	}

	// Token: 0x06000E93 RID: 3731 RVA: 0x000948D9 File Offset: 0x00092CD9
	private static void OnSaved(bool success)
	{
		if (!success)
		{
			global::Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", null);
			PlayerData.Save(PlayerData.CurrentSaveFileIndex);
		}
	}

	// Token: 0x06000E94 RID: 3732 RVA: 0x000948FB File Offset: 0x00092CFB
	private static void OnSavedAll(bool success)
	{
		if (success)
		{
			PlayerData.Initialized = true;
			if (PlayerData._playerDatatInitHandler != null)
			{
				PlayerData._playerDatatInitHandler(true);
				PlayerData._playerDatatInitHandler = null;
			}
		}
		else
		{
			global::Debug.LogError("[PlayerData] SAVE FAILED. Retrying...", null);
			PlayerData.SaveAll();
		}
	}

	// Token: 0x06000E95 RID: 3733 RVA: 0x00094939 File Offset: 0x00092D39
	public static void SaveCurrentFile()
	{
		PlayerData.Save(PlayerData.CurrentSaveFileIndex);
	}

	// Token: 0x06000E96 RID: 3734 RVA: 0x00094945 File Offset: 0x00092D45
	public static void ResetDialoguer()
	{
		Dialoguer.SetGlobalVariablesState(PlayerData.emptyDialoguerState);
	}

	// Token: 0x06000E97 RID: 3735 RVA: 0x00094954 File Offset: 0x00092D54
	public static void ResetAll()
	{
		for (int i = 0; i < PlayerData.SAVE_FILE_KEYS.Length; i++)
		{
			PlayerData.ClearSlot(i);
		}
	}

	// Token: 0x06000E98 RID: 3736 RVA: 0x0009497F File Offset: 0x00092D7F
	public static void Unload()
	{
		PlayerData._saveFiles = null;
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06000E99 RID: 3737 RVA: 0x00094987 File Offset: 0x00092D87
	public PlayerData.PlayerLoadouts Loadouts
	{
		get
		{
			return this.loadouts;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x06000E9A RID: 3738 RVA: 0x0009498F File Offset: 0x00092D8F
	// (set) Token: 0x06000E9B RID: 3739 RVA: 0x00094997 File Offset: 0x00092D97
	public bool IsHardModeAvailable
	{
		get
		{
			return this._isHardModeAvailable;
		}
		set
		{
			this._isHardModeAvailable = value;
		}
	}

	// Token: 0x17000256 RID: 598
	// (get) Token: 0x06000E9C RID: 3740 RVA: 0x000949A0 File Offset: 0x00092DA0
	// (set) Token: 0x06000E9D RID: 3741 RVA: 0x000949A8 File Offset: 0x00092DA8
	public bool IsHardModeAvailableDLC
	{
		get
		{
			return this._isHardModeAvailableDLC;
		}
		set
		{
			this._isHardModeAvailableDLC = value;
		}
	}

	// Token: 0x17000257 RID: 599
	// (get) Token: 0x06000E9E RID: 3742 RVA: 0x000949B1 File Offset: 0x00092DB1
	// (set) Token: 0x06000E9F RID: 3743 RVA: 0x000949B9 File Offset: 0x00092DB9
	public bool IsTutorialCompleted
	{
		get
		{
			return this._isTutorialCompleted;
		}
		set
		{
			this._isTutorialCompleted = value;
		}
	}

	// Token: 0x17000258 RID: 600
	// (get) Token: 0x06000EA0 RID: 3744 RVA: 0x000949C2 File Offset: 0x00092DC2
	// (set) Token: 0x06000EA1 RID: 3745 RVA: 0x000949CA File Offset: 0x00092DCA
	public bool IsFlyingTutorialCompleted
	{
		get
		{
			return this._isFlyingTutorialCompleted;
		}
		set
		{
			this._isFlyingTutorialCompleted = value;
		}
	}

	// Token: 0x17000259 RID: 601
	// (get) Token: 0x06000EA2 RID: 3746 RVA: 0x000949D3 File Offset: 0x00092DD3
	// (set) Token: 0x06000EA3 RID: 3747 RVA: 0x000949DB File Offset: 0x00092DDB
	public bool IsChaliceTutorialCompleted
	{
		get
		{
			return this._isChaliceTutorialCompleted;
		}
		set
		{
			this._isChaliceTutorialCompleted = value;
		}
	}

	// Token: 0x06000EA4 RID: 3748 RVA: 0x000949E4 File Offset: 0x00092DE4
	public bool IsUnlocked(PlayerId player, Weapon value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x06000EA5 RID: 3749 RVA: 0x00094A70 File Offset: 0x00092E70
	public bool IsUnlocked(PlayerId player, Super value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x06000EA6 RID: 3750 RVA: 0x00094AFC File Offset: 0x00092EFC
	public bool IsUnlocked(PlayerId player, Charm value)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value);
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).IsUnlocked(value) || this.inventories.GetPlayer(PlayerId.PlayerTwo).IsUnlocked(value);
	}

	// Token: 0x06000EA7 RID: 3751 RVA: 0x00094B88 File Offset: 0x00092F88
	public bool HasNewPurchase(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase;
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return false;
		}
		return this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase || this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase;
	}

	// Token: 0x06000EA8 RID: 3752 RVA: 0x00094C10 File Offset: 0x00093010
	public void ResetHasNewPurchase(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = false;
			return;
		}
		if (player == PlayerId.PlayerTwo)
		{
			this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = false;
			return;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return;
		}
		this.inventories.GetPlayer(PlayerId.PlayerOne).newPurchase = false;
		this.inventories.GetPlayer(PlayerId.PlayerTwo).newPurchase = false;
	}

	// Token: 0x06000EA9 RID: 3753 RVA: 0x00094C90 File Offset: 0x00093090
	public bool Buy(PlayerId player, Weapon value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x06000EAA RID: 3754 RVA: 0x00094CA4 File Offset: 0x000930A4
	public bool Buy(PlayerId player, Super value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x06000EAB RID: 3755 RVA: 0x00094CB8 File Offset: 0x000930B8
	public bool Buy(PlayerId player, Charm value)
	{
		return this.inventories.GetPlayer(player).Buy(value);
	}

	// Token: 0x06000EAC RID: 3756 RVA: 0x00094CCC File Offset: 0x000930CC
	public void Gift(PlayerId player, Weapon value)
	{
		this.inventories.GetPlayer(player)._weapons.Add(value);
	}

	// Token: 0x06000EAD RID: 3757 RVA: 0x00094CE5 File Offset: 0x000930E5
	public void Gift(PlayerId player, Super value)
	{
		this.inventories.GetPlayer(player)._supers.Add(value);
	}

	// Token: 0x06000EAE RID: 3758 RVA: 0x00094CFE File Offset: 0x000930FE
	public void Gift(PlayerId player, Charm value)
	{
		this.inventories.GetPlayer(player)._charms.Add(value);
	}

	// Token: 0x06000EAF RID: 3759 RVA: 0x00094D17 File Offset: 0x00093117
	public int NumWeapons(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._weapons.Count;
	}

	// Token: 0x06000EB0 RID: 3760 RVA: 0x00094D2F File Offset: 0x0009312F
	public int NumCharms(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._charms.Count;
	}

	// Token: 0x06000EB1 RID: 3761 RVA: 0x00094D47 File Offset: 0x00093147
	public int NumSupers(PlayerId player)
	{
		return this.inventories.GetPlayer(player)._supers.Count;
	}

	// Token: 0x06000EB2 RID: 3762 RVA: 0x00094D5F File Offset: 0x0009315F
	public int GetCurrency(PlayerId player)
	{
		return this.inventories.GetPlayer(player).money;
	}

	// Token: 0x06000EB3 RID: 3763 RVA: 0x00094D72 File Offset: 0x00093172
	public void AddCurrency(PlayerId player, int value)
	{
		this.inventories.GetPlayer(player).money += value;
	}

	// Token: 0x06000EB4 RID: 3764 RVA: 0x00094D8D File Offset: 0x0009318D
	public void ResetLevelCoinManager()
	{
		this.levelCoinManager = new PlayerData.PlayerCoinManager();
	}

	// Token: 0x06000EB5 RID: 3765 RVA: 0x00094D9A File Offset: 0x0009319A
	public bool GetCoinCollected(LevelCoin coin)
	{
		return this.coinManager.GetCoinCollected(coin);
	}

	// Token: 0x06000EB6 RID: 3766 RVA: 0x00094DA8 File Offset: 0x000931A8
	public void SetLevelCoinCollected(LevelCoin coin, bool collected, PlayerId player)
	{
		this.levelCoinManager.SetCoinValue(coin, collected, player);
	}

	// Token: 0x1700025A RID: 602
	// (get) Token: 0x06000EB7 RID: 3767 RVA: 0x00094DB8 File Offset: 0x000931B8
	public int NumCoinsCollected
	{
		get
		{
			return this.coinManager.NumCoinsCollected();
		}
	}

	// Token: 0x1700025B RID: 603
	// (get) Token: 0x06000EB8 RID: 3768 RVA: 0x00094DC5 File Offset: 0x000931C5
	public int NumCoinsCollectedMainGame
	{
		get
		{
			return this.coinManager.NumCoinsCollected(false);
		}
	}

	// Token: 0x06000EB9 RID: 3769 RVA: 0x00094DD4 File Offset: 0x000931D4
	public int GetNumCoinsCollectedInLevel(Levels level)
	{
		List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = this.coinManager.LevelsAndCoins;
		for (int i = 0; i < levelsAndCoins.Count; i++)
		{
			if (levelsAndCoins[i].level == level)
			{
				int num = 0;
				if (levelsAndCoins[i].Coin1Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin2Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin3Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin4Collected)
				{
					num++;
				}
				if (levelsAndCoins[i].Coin5Collected)
				{
					num++;
				}
				return num;
			}
		}
		return 0;
	}

	// Token: 0x06000EBA RID: 3770 RVA: 0x00094E84 File Offset: 0x00093284
	public void ApplyLevelCoins()
	{
		foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.levelCoinManager.coins)
		{
			this.coinManager.SetCoinValue(playerCoinProperties.coinID, playerCoinProperties.collected, playerCoinProperties.player);
			if (playerCoinProperties.collected)
			{
				PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
				PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			}
		}
		this.levelCoinManager = new PlayerData.PlayerCoinManager();
	}

	// Token: 0x1700025C RID: 604
	// (get) Token: 0x06000EBB RID: 3771 RVA: 0x00094F2C File Offset: 0x0009332C
	public PlayerData.MapData CurrentMapData
	{
		get
		{
			return this.mapDataManager.GetCurrentMapData();
		}
	}

	// Token: 0x06000EBC RID: 3772 RVA: 0x00094F39 File Offset: 0x00093339
	public PlayerData.MapData GetMapData(Scenes map)
	{
		return this.mapDataManager.GetMapData(map);
	}

	// Token: 0x1700025D RID: 605
	// (get) Token: 0x06000EBD RID: 3773 RVA: 0x00094F47 File Offset: 0x00093347
	// (set) Token: 0x06000EBE RID: 3774 RVA: 0x00094F54 File Offset: 0x00093354
	public Scenes CurrentMap
	{
		get
		{
			return this.mapDataManager.currentMap;
		}
		set
		{
			this.mapDataManager.currentMap = value;
		}
	}

	// Token: 0x06000EBF RID: 3775 RVA: 0x00094F62 File Offset: 0x00093362
	public PlayerData.PlayerLevelDataObject GetLevelData(Levels levelID)
	{
		return this.levelDataManager.GetLevelData(levelID);
	}

	// Token: 0x06000EC0 RID: 3776 RVA: 0x00094F70 File Offset: 0x00093370
	public int CountLevelsCompleted(Levels[] levels)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000EC1 RID: 3777 RVA: 0x00094FB4 File Offset: 0x000933B4
	public bool CheckLevelsCompleted(Levels[] levels)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000EC2 RID: 3778 RVA: 0x00094FF4 File Offset: 0x000933F4
	public bool CheckLevelCompleted(Levels level)
	{
		PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(level);
		return levelData.completed;
	}

	// Token: 0x06000EC3 RID: 3779 RVA: 0x00095018 File Offset: 0x00093418
	public int CountLevelsHaveMinGrade(Levels[] levels, LevelScoringData.Grade minGrade)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed && levelData.grade >= minGrade)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000EC4 RID: 3780 RVA: 0x00095068 File Offset: 0x00093468
	public bool CheckLevelsHaveMinGrade(Levels[] levels, LevelScoringData.Grade minGrade)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed || levelData.grade < minGrade)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000EC5 RID: 3781 RVA: 0x000950B4 File Offset: 0x000934B4
	public int CountLevelsHaveMinDifficulty(Levels[] levels, Level.Mode minDifficulty)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed && levelData.difficultyBeaten >= minDifficulty)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000EC6 RID: 3782 RVA: 0x00095104 File Offset: 0x00093504
	public bool CheckLevelsHaveMinDifficulty(Levels[] levels, Level.Mode minDifficulty)
	{
		foreach (Levels levelID in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (!levelData.completed || levelData.difficultyBeaten < minDifficulty)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06000EC7 RID: 3783 RVA: 0x00095150 File Offset: 0x00093550
	public int CountLevelsChaliceCompleted(Levels[] levels, PlayerId playerId)
	{
		int num = 0;
		foreach (Levels levelID in levels)
		{
			if ((playerId == PlayerId.PlayerOne && this.GetLevelData(levelID).completedAsChaliceP1) || (playerId == PlayerId.PlayerTwo && this.GetLevelData(levelID).completedAsChaliceP2))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000EC8 RID: 3784 RVA: 0x000951B0 File Offset: 0x000935B0
	private static float CurseCharmValue(Levels level)
	{
		List<Levels> list = new List<Levels>(Level.world1BossLevels);
		if (Array.IndexOf<Levels>(Level.world1BossLevels, level) >= 0)
		{
			return 2f;
		}
		if (Array.IndexOf<Levels>(Level.world2BossLevels, level) >= 0)
		{
			return 2.5f;
		}
		if (Array.IndexOf<Levels>(Level.world3BossLevels, level) >= 0)
		{
			return 3f;
		}
		if (Array.IndexOf<Levels>(Level.world4MiniBossLevels, level) >= 0)
		{
			return 1f;
		}
		if (level == Levels.DicePalaceMain)
		{
			return 1f;
		}
		if (level == Levels.Devil)
		{
			return 4f;
		}
		if (Array.IndexOf<Levels>(Level.worldDLCBossLevels, level) >= 0)
		{
			return 3f;
		}
		if (level == Levels.Saltbaker)
		{
			return 4f;
		}
		return 0f;
	}

	// Token: 0x06000EC9 RID: 3785 RVA: 0x00095274 File Offset: 0x00093674
	private int completionPercentageOnly_CalculateCurseCharmLevel(PlayerId playerId)
	{
		if (!this.GetLevelData(Levels.Graveyard).completed)
		{
			return -1;
		}
		Levels[] levels = new Levels[]
		{
			Levels.Veggies,
			Levels.Slime,
			Levels.FlyingBlimp,
			Levels.Flower,
			Levels.Frogs,
			Levels.Baroness,
			Levels.Clown,
			Levels.FlyingGenie,
			Levels.Dragon,
			Levels.FlyingBird,
			Levels.Bee,
			Levels.Pirate,
			Levels.SallyStagePlay,
			Levels.Mouse,
			Levels.Robot,
			Levels.FlyingMermaid,
			Levels.Train,
			Levels.DicePalaceBooze,
			Levels.DicePalaceChips,
			Levels.DicePalaceCigar,
			Levels.DicePalaceDomino,
			Levels.DicePalaceEightBall,
			Levels.DicePalaceFlyingHorse,
			Levels.DicePalaceFlyingMemory,
			Levels.DicePalaceRabbit,
			Levels.DicePalaceRoulette,
			Levels.DicePalaceMain,
			Levels.Devil,
			Levels.Airplane,
			Levels.RumRunners,
			Levels.OldMan,
			Levels.SnowCult,
			Levels.FlyingCowboy,
			Levels.Saltbaker
		};
		int num = this.CalculateCurseCharmAccumulatedValue(playerId, levels);
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		for (int i = 0; i < levelThreshold.Length; i++)
		{
			if (num < levelThreshold[i])
			{
				return i - 1;
			}
		}
		return levelThreshold.Length - 1;
	}

	// Token: 0x06000ECA RID: 3786 RVA: 0x000952E0 File Offset: 0x000936E0
	private bool completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId playerId)
	{
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		return this.completionPercentageOnly_CalculateCurseCharmLevel(playerId) == levelThreshold.Length - 1;
	}

	// Token: 0x06000ECB RID: 3787 RVA: 0x00095304 File Offset: 0x00093704
	public int CalculateCurseCharmAccumulatedValue(PlayerId playerId, Levels[] levels)
	{
		float num = 0f;
		foreach (Levels levels2 in levels)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levels2);
			if (playerId == PlayerId.PlayerOne && levelData.curseCharmP1)
			{
				num += PlayerData.CurseCharmValue(levels2);
			}
			else if (playerId == PlayerId.PlayerTwo && levelData.curseCharmP2)
			{
				num += PlayerData.CurseCharmValue(levels2);
			}
		}
		return (int)num;
	}

	// Token: 0x06000ECC RID: 3788 RVA: 0x00095378 File Offset: 0x00093778
	public float GetCompletionPercentage()
	{
		List<Levels> list = new List<Levels>();
		list.AddRange(Level.world1BossLevels);
		list.AddRange(Level.world2BossLevels);
		list.AddRange(Level.world3BossLevels);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		foreach (Levels levelID in list)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
				Level.Mode difficultyBeaten = levelData.difficultyBeaten;
				if (difficultyBeaten != Level.Mode.Normal)
				{
					if (difficultyBeaten == Level.Mode.Hard)
					{
						num2++;
						num6++;
					}
				}
				else
				{
					num2++;
				}
			}
		}
		foreach (Levels levelID2 in Level.platformingLevels)
		{
			PlayerData.PlayerLevelDataObject levelData2 = this.GetLevelData(levelID2);
			if (levelData2.completed)
			{
				num3++;
			}
		}
		int num9 = this.coinManager.NumCoinsCollected(false);
		int num10 = this.NumSupers(PlayerId.PlayerOne);
		PlayerData.PlayerLevelDataObject levelData3 = this.GetLevelData(Levels.DicePalaceMain);
		if (levelData3.completed)
		{
			num4++;
			if (levelData3.difficultyBeaten == Level.Mode.Hard)
			{
				num7++;
			}
		}
		PlayerData.PlayerLevelDataObject levelData4 = this.GetLevelData(Levels.Devil);
		if (levelData4.completed)
		{
			num5++;
			if (levelData4.difficultyBeaten == Level.Mode.Hard)
			{
				num8++;
			}
		}
		return (float)num * 1.5f + (float)num3 * 1.5f + (float)num9 * 0.5f + (float)num10 * 1.5f + (float)(num2 * 2) + (float)(num4 * 3) + (float)(num5 * 4) + (float)(num6 * 5) + (float)(num7 * 7) + (float)(num8 * 8);
	}

	// Token: 0x06000ECD RID: 3789 RVA: 0x00095560 File Offset: 0x00093960
	public float GetCompletionPercentageDLC()
	{
		if (!DLCManager.DLCEnabled())
		{
			return 0f;
		}
		List<Levels> list = new List<Levels>();
		list.AddRange(Level.worldDLCBossLevels);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		foreach (Levels levelID in list)
		{
			PlayerData.PlayerLevelDataObject levelData = this.GetLevelData(levelID);
			if (levelData.completed)
			{
				num++;
				Level.Mode difficultyBeaten = levelData.difficultyBeaten;
				if (difficultyBeaten != Level.Mode.Normal)
				{
					if (difficultyBeaten == Level.Mode.Hard)
					{
						num2++;
						num3++;
					}
				}
				else
				{
					num2++;
				}
			}
		}
		int num9 = this.coinManager.NumCoinsCollected(true);
		PlayerData.PlayerLevelDataObject levelData2 = this.GetLevelData(Levels.Saltbaker);
		if (levelData2.completed)
		{
			num4++;
			if (levelData2.difficultyBeaten == Level.Mode.Hard)
			{
				num5++;
			}
		}
		if (this.curseCharmPuzzleComplete)
		{
			num6++;
		}
		if (this.GetLevelData(Levels.Graveyard).completed)
		{
			num7++;
		}
		if (this.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerOne) || this.completionPercentageOnly_CurseCharmIsMaxLevel(PlayerId.PlayerTwo))
		{
			num8++;
		}
		return (float)num * 3.5f + (float)num2 * 5f + (float)num3 * 4.5f + (float)num4 * 6f + (float)num5 * 6f + (float)num9 * 1f + (float)num6 * 1f + (float)num7 * 3f + (float)num8 * 3f;
	}

	// Token: 0x06000ECE RID: 3790 RVA: 0x0009571C File Offset: 0x00093B1C
	public int DeathCount(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount();
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount();
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return 0;
		}
		return this.statictics.GetPlayer(PlayerId.PlayerOne).DeathCount() + this.statictics.GetPlayer(PlayerId.PlayerTwo).DeathCount();
	}

	// Token: 0x06000ECF RID: 3791 RVA: 0x0009579C File Offset: 0x00093B9C
	public void Die(PlayerId player)
	{
		if (player != PlayerId.PlayerOne)
		{
			if (player != PlayerId.PlayerTwo)
			{
				if (player != PlayerId.Any && player != PlayerId.None)
				{
				}
			}
			else
			{
				this.statictics.GetPlayer(PlayerId.PlayerTwo).Die();
			}
		}
		else
		{
			this.statictics.GetPlayer(PlayerId.PlayerOne).Die();
		}
	}

	// Token: 0x06000ED0 RID: 3792 RVA: 0x00095804 File Offset: 0x00093C04
	public int GetNumParriesInRow(PlayerId player)
	{
		if (player == PlayerId.PlayerOne)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow;
		}
		if (player == PlayerId.PlayerTwo)
		{
			return this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow;
		}
		if (player != PlayerId.Any)
		{
			if (player != PlayerId.None)
			{
			}
			return 0;
		}
		return Mathf.Max(this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow, this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow);
	}

	// Token: 0x06000ED1 RID: 3793 RVA: 0x00095888 File Offset: 0x00093C88
	public void SetNumParriesInRow(PlayerId player, int numParriesInRow)
	{
		if (player != PlayerId.PlayerOne)
		{
			if (player != PlayerId.PlayerTwo)
			{
				if (player != PlayerId.Any && player != PlayerId.None)
				{
				}
			}
			else
			{
				this.statictics.GetPlayer(PlayerId.PlayerTwo).numParriesInRow = numParriesInRow;
			}
		}
		else
		{
			this.statictics.GetPlayer(PlayerId.PlayerOne).numParriesInRow = numParriesInRow;
		}
	}

	// Token: 0x06000ED2 RID: 3794 RVA: 0x000958F0 File Offset: 0x00093CF0
	public void IncrementKingOfGamesCounter()
	{
		if (this.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length)
		{
			return;
		}
		this.chessBossAttemptCounter++;
	}

	// Token: 0x06000ED3 RID: 3795 RVA: 0x00095918 File Offset: 0x00093D18
	public void ResetKingOfGamesCounter()
	{
		this.chessBossAttemptCounter = 0;
	}

	// Token: 0x06000ED4 RID: 3796 RVA: 0x00095924 File Offset: 0x00093D24
	public bool TryActivateDjimmi()
	{
		if (this.DjimmiFreedCurrentRegion())
		{
			if (this.DjimmiActivatedCurrentRegion())
			{
				if (this.CurrentMap == Scenes.scene_map_world_DLC)
				{
					this.djimmiActivatedInfiniteWishDLC = false;
				}
				else
				{
					this.djimmiActivatedInfiniteWishBaseGame = false;
				}
				AudioManager.Play("sfx_worldmap_djimmi_deactivate");
			}
			else
			{
				if (this.CurrentMap == Scenes.scene_map_world_DLC)
				{
					this.djimmiActivatedInfiniteWishDLC = true;
				}
				else
				{
					this.djimmiActivatedInfiniteWishBaseGame = true;
				}
				MapEventNotification.Current.ShowEvent(MapEventNotification.Type.DjimmiFreed);
			}
			PlayerData.SaveCurrentFile();
			return true;
		}
		if (this.djimmiActivatedCountedWish)
		{
			this.djimmiActivatedCountedWish = false;
			this.djimmiWishes++;
			PlayerData.SaveCurrentFile();
			AudioManager.Play("sfx_worldmap_djimmi_deactivate");
			return true;
		}
		if (!this.djimmiActivatedCountedWish && this.djimmiWishes > 0)
		{
			this.djimmiActivatedCountedWish = true;
			this.djimmiWishes--;
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Djimmi);
			return true;
		}
		return false;
	}

	// Token: 0x06000ED5 RID: 3797 RVA: 0x00095A1A File Offset: 0x00093E1A
	public bool DjimmiActivatedCurrentRegion()
	{
		return (this.CurrentMap != Scenes.scene_map_world_DLC) ? this.DjimmiActivatedBaseGame() : this.DjimmiActivatedDLC();
	}

	// Token: 0x06000ED6 RID: 3798 RVA: 0x00095A3A File Offset: 0x00093E3A
	public bool DjimmiActivatedBaseGame()
	{
		return this.djimmiActivatedCountedWish || this.djimmiActivatedInfiniteWishBaseGame;
	}

	// Token: 0x06000ED7 RID: 3799 RVA: 0x00095A50 File Offset: 0x00093E50
	public bool DjimmiActivatedDLC()
	{
		return this.djimmiActivatedCountedWish || this.djimmiActivatedInfiniteWishDLC;
	}

	// Token: 0x06000ED8 RID: 3800 RVA: 0x00095A66 File Offset: 0x00093E66
	public bool DjimmiFreedCurrentRegion()
	{
		return (this.CurrentMap != Scenes.scene_map_world_DLC) ? this.DjimmiFreedBaseGame() : this.DjimmiFreedDLC();
	}

	// Token: 0x06000ED9 RID: 3801 RVA: 0x00095A88 File Offset: 0x00093E88
	public bool DjimmiFreedBaseGame()
	{
		return this.CheckLevelsCompleted(Level.world1BossLevels) && this.CheckLevelsCompleted(Level.world2BossLevels) && this.CheckLevelsCompleted(Level.world3BossLevels) && this.CheckLevelsCompleted(Level.world4BossLevels) && this.CheckLevelsCompleted(Level.platformingLevels);
	}

	// Token: 0x06000EDA RID: 3802 RVA: 0x00095AE3 File Offset: 0x00093EE3
	public bool DjimmiFreedDLC()
	{
		return this.CheckLevelsCompleted(Level.worldDLCBossLevelsWithSaltbaker) && this.CheckLevelsCompleted(Level.kingOfGamesLevels);
	}

	// Token: 0x06000EDB RID: 3803 RVA: 0x00095B03 File Offset: 0x00093F03
	public void DeactivateDjimmi()
	{
		if (this.DjimmiFreedCurrentRegion())
		{
			if (this.CurrentMap == Scenes.scene_map_world_DLC)
			{
				this.djimmiActivatedInfiniteWishDLC = false;
			}
			else
			{
				this.djimmiActivatedInfiniteWishBaseGame = false;
			}
		}
		else
		{
			this.djimmiActivatedCountedWish = false;
		}
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x040017B5 RID: 6069
	public static readonly PlayerData.LevelCoinIds[] platformingCoinIDs = new PlayerData.LevelCoinIds[]
	{
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_1_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 5fd52d1b-a7f2-43a6-80e2-cb170cbc7d4d"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 63c021bf-52f0-41de-bedf-c77117d244cc"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 245037a6-1fa2-4167-a631-0723abff8138"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: eaefb009-c117-4b9a-96c1-7abc5558d213"
			},
			new string[]
			{
				"scene_level_platforming_1_1F::Level_Coin :: 5526f7bc-a902-4c13-9e7a-1632a5abe378"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_1_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 323989de-349e-4740-a764-dbc12217a27c"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 55a46261-b14c-4065-9ada-18524eaed9f3"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: da0983f6-62d4-4ace-81f2-cad7181d5fe9"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: 7088ec51-4792-49c0-ab2c-c45ec9deb9f0"
			},
			new string[]
			{
				"scene_level_platforming_1_2F::Level_Coin :: e02954c1-ff76-4ba4-849f-90aae53a7787"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_2_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: 24ef654a-a65b-4a1c-b5e5-c3c64e250646"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: b8d96f03-d264-4a61-9ab9-07de34f660aa"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: 383d9b3b-c280-4825-a6b3-1a21fe42d0ac"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: f1b99bcd-0fa8-4aac-9a54-f310e173ddf9"
			},
			new string[]
			{
				"scene_level_platforming_2_1F::Level_Coin :: c763ef21-2ee7-491c-a143-b906856fed6c"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_2_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 9025a0e9-fff1-4f14-93d1-1930eef27405",
				"scene_level_platforming_2_2F::Level_Coin :: abbfb110-69d1-4948-9c70-223c6425c6f5",
				"scene_level_platforming_2_2F::Level_Coin :: 159497a2-3ded-4c0e-8852-4f6c41046df7",
				"scene_level_platforming_2_2F::Level_Coin :: 22bd722b-bf79-438b-92b0-56c638ae7114",
				"scene_level_platforming_2_2F::Level_Coin :: 84c8547b-b9b8-4fe9-9b0f-75980a3f5454",
				"scene_level_platforming_2_2F::Level_Coin :: d8d1b996-c4ef-4586-9c69-a3f18ebaeece",
				"scene_level_platforming_2_2F::Level_Coin :: 79695e06-f5c3-4826-96e8-5318399cdaf0",
				"scene_level_platforming_2_2F::Level_Coin :: 3aa60c71-a8c9-4b44-b53e-f954c9c70b29"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 284ea6f9-5db4-4f80-b0e5-1d9513a8acb7"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: 43a8fc82-b8b8-4a92-b56f-c3e718b46b2c"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: bf86d025-4524-4ce8-ba07-540ef3f61ed8"
			},
			new string[]
			{
				"scene_level_platforming_2_2F::Level_Coin :: a7c0e2b9-9560-4ed7-a3a4-428365222cb9"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_3_1, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 26ba2e1d-4b0a-4964-ba4d-f58655ef47db",
				"scene_level_platforming_3_1F::Level_Coin :: 8d1cd543-fa2f-41d6-9e50-d8ea356c9d26",
				"scene_level_platforming_3_1F::Level_Coin :: 90912b91-c396-429a-b061-0af90b666a0f",
				"scene_level_platforming_3_1F::Level_Coin :: 7a4de11e-fed9-479a-8ace-57bb7a00baa7",
				"scene_level_platforming_3_1F::Level_Coin :: eabb3294-336c-4615-8975-210343a039b5",
				"scene_level_platforming_3_1F::Level_Coin :: 6c032ae2-7bb9-4236-abc4-c27177201615",
				"scene_level_platforming_3_1F::Level_Coin :: 6fcd5ca7-9953-4343-a7f4-55d3fbc7d287",
				"scene_level_platforming_3_1F::Level_Coin :: e280e5f3-9fa1-4587-9139-84c127413e7a"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0f13fbe6-1041-445f-97ed-1bbe2cb0339e",
				"scene_level_platforming_3_1F::Level_Coin :: 9aa051bf-5ec9-47b2-93f5-09f1495e78f2",
				"scene_level_platforming_3_1F::Level_Coin :: 4f4c2a23-244a-484b-84c9-ca5c6fc4e6bb",
				"scene_level_platforming_3_1F::Level_Coin :: 5c1e1ce4-055a-4ed6-8f5a-c667dbcac5af",
				"scene_level_platforming_3_1F::Level_Coin :: c1b74075-ae62-45ab-8d60-08286a35936f",
				"scene_level_platforming_3_1F::Level_Coin :: 5e1c290f-e2a4-4410-a52c-ba41ce7e56c5",
				"scene_level_platforming_3_1F::Level_Coin :: b18f581d-67b3-4020-b031-3a5bb62a9fa1",
				"scene_level_platforming_3_1F::Level_Coin :: 7b9e2b26-9132-4558-922b-ea400d4fdb0f"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0086a9b3-87b8-4406-b97b-b94a1fd60bb0",
				"scene_level_platforming_3_1F::Level_Coin :: 273f231f-11d1-42db-888d-7d78696b934b",
				"scene_level_platforming_3_1F::Level_Coin :: cab629f0-54fa-43d3-8573-5d82db28e5c9",
				"scene_level_platforming_3_1F::Level_Coin :: b9ffa14a-984d-426b-8a96-7e71c58d8542",
				"scene_level_platforming_3_1F::Level_Coin :: b0f7e7a4-16a8-4a58-9abc-51f2aac1aac3",
				"scene_level_platforming_3_1F::Level_Coin :: 2b7cac59-e975-47f2-bf74-e49cb612266a",
				"scene_level_platforming_3_1F::Level_Coin :: e74bfad7-8657-4d6b-b853-9fef027e8600",
				"scene_level_platforming_3_1F::Level_Coin :: 6153c3cb-493f-465e-b6b2-dcddd5c0c50e"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: 0a6fbbe4-5c13-4b17-9b58-91e7bbdacde4",
				"scene_level_platforming_3_1F::Level_Coin :: f72bdba8-cc0b-4d17-a83f-9892c3507b1c",
				"scene_level_platforming_3_1F::Level_Coin :: 5eaabcfd-0101-4ff5-92f4-a65d885be960",
				"scene_level_platforming_3_1F::Level_Coin :: 036a2830-7c80-443b-b9f6-1576dbf5cb33",
				"scene_level_platforming_3_1F::Level_Coin :: 1c782442-7e15-4a48-a66b-19c5a862e61e",
				"scene_level_platforming_3_1F::Level_Coin :: 01b6dc66-dd9a-4a6f-ac4d-e93a173395ef",
				"scene_level_platforming_3_1F::Level_Coin :: 4ca8faee-fb21-4f5a-b521-4deb89d853c3",
				"scene_level_platforming_3_1F::Level_Coin :: 5bfd4fdf-546c-4751-b2a7-eb99c7cdd2f4"
			},
			new string[]
			{
				"scene_level_platforming_3_1F::Level_Coin :: beb664ad-5577-4055-9164-b1b2f77430f3",
				"scene_level_platforming_3_1F::Level_Coin :: 2ffc0eef-d922-4825-bfb4-7377c16e197d",
				"scene_level_platforming_3_1F::Level_Coin :: 0c636a66-f96c-4046-9ccd-12897ab77649",
				"scene_level_platforming_3_1F::Level_Coin :: 267b5e81-84e6-4297-848c-bea5549b1690",
				"scene_level_platforming_3_1F::Level_Coin :: 76e64c16-b4d3-472f-85ae-d1dbd5c055e3",
				"scene_level_platforming_3_1F::Level_Coin :: 05b62218-8f30-4d74-bab4-7d27f4e0ab90",
				"scene_level_platforming_3_1F::Level_Coin :: 6222ae58-b0c8-44e8-81f6-417f00cc1be1",
				"scene_level_platforming_3_1F::Level_Coin :: 54c21221-4a03-4437-a2bd-a5972c3e2bfc"
			}
		}),
		new PlayerData.LevelCoinIds(Levels.Platforming_Level_3_2, new string[][]
		{
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 5da68904-6505-4841-9684-71d2931c1bd6"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 999c9b0d-d554-471d-ad96-ee6d57ccfd19"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: cf0a7cae-d8d9-4be0-9502-8b8544606e04"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: e671db16-cf6e-421c-937c-2b6f5c7ad0e7"
			},
			new string[]
			{
				"scene_level_platforming_3_2F::Level_Coin :: 084a7b75-e752-452f-8710-687db1e165fe"
			}
		})
	};

	// Token: 0x040017B6 RID: 6070
	private const string KEY = "cuphead_player_data_v1_slot_";

	// Token: 0x040017B7 RID: 6071
	private static readonly string[] SAVE_FILE_KEYS = new string[]
	{
		"cuphead_player_data_v1_slot_0",
		"cuphead_player_data_v1_slot_1",
		"cuphead_player_data_v1_slot_2"
	};

	// Token: 0x040017B8 RID: 6072
	public static readonly Weapon[] WeaponsDLC = new Weapon[]
	{
		Weapon.level_weapon_wide_shot,
		Weapon.level_weapon_upshot,
		Weapon.level_weapon_crackshot
	};

	// Token: 0x040017B9 RID: 6073
	public static readonly Charm[] CharmsDLC = new Charm[]
	{
		Charm.charm_chalice,
		Charm.charm_healer,
		Charm.charm_curse
	};

	// Token: 0x040017BA RID: 6074
	private static string emptyDialoguerState = string.Empty;

	// Token: 0x040017BB RID: 6075
	private static int _CurrentSaveFileIndex = 0;

	// Token: 0x040017BC RID: 6076
	private static bool _initialized = false;

	// Token: 0x040017BD RID: 6077
	public static bool inGame = false;

	// Token: 0x040017BE RID: 6078
	private static PlayerData[] _saveFiles;

	// Token: 0x040017BF RID: 6079
	private static PlayerData.PlayerDataInitHandler _playerDatatInitHandler;

	// Token: 0x040017C0 RID: 6080
	public bool isPlayer1Mugman;

	// Token: 0x040017C1 RID: 6081
	public bool hasMadeFirstPurchase;

	// Token: 0x040017C2 RID: 6082
	public bool hasBeatenAnyBossOnEasy;

	// Token: 0x040017C3 RID: 6083
	public bool hasBeatenAnyDLCBossOnEasy;

	// Token: 0x040017C4 RID: 6084
	public bool hasUnlockedFirstSuper;

	// Token: 0x040017C5 RID: 6085
	public bool shouldShowShopkeepTooltip;

	// Token: 0x040017C6 RID: 6086
	public bool shouldShowTurtleTooltip;

	// Token: 0x040017C7 RID: 6087
	public bool shouldShowCanteenTooltip;

	// Token: 0x040017C8 RID: 6088
	public bool shouldShowForkTooltip;

	// Token: 0x040017C9 RID: 6089
	public bool shouldShowKineDiceTooltip;

	// Token: 0x040017CA RID: 6090
	public bool shouldShowMausoleumTooltip;

	// Token: 0x040017CB RID: 6091
	public bool hasUnlockedBoatman;

	// Token: 0x040017CC RID: 6092
	public bool shouldShowBoatmanTooltip;

	// Token: 0x040017CD RID: 6093
	public bool shouldShowChaliceTooltip;

	// Token: 0x040017CE RID: 6094
	public bool hasTalkedToChaliceFan;

	// Token: 0x040017CF RID: 6095
	public int[] curseCharmPuzzleOrder;

	// Token: 0x040017D0 RID: 6096
	public bool curseCharmPuzzleComplete;

	// Token: 0x040017D1 RID: 6097
	public MapCastleZones.Zone currentChessBossZone;

	// Token: 0x040017D2 RID: 6098
	public List<MapCastleZones.Zone> usedChessBossZones = new List<MapCastleZones.Zone>();

	// Token: 0x040017D3 RID: 6099
	public int chessBossAttemptCounter;

	// Token: 0x040017D4 RID: 6100
	public bool djimmiActivatedCountedWish;

	// Token: 0x040017D5 RID: 6101
	public bool djimmiActivatedInfiniteWishBaseGame;

	// Token: 0x040017D6 RID: 6102
	public bool djimmiActivatedInfiniteWishDLC;

	// Token: 0x040017D7 RID: 6103
	public int djimmiWishes = 3;

	// Token: 0x040017D8 RID: 6104
	public bool djimmiFreed;

	// Token: 0x040017D9 RID: 6105
	public bool djimmiFreedDLC;

	// Token: 0x040017DA RID: 6106
	public int dummy;

	// Token: 0x040017DB RID: 6107
	[SerializeField]
	private PlayerData.PlayerLoadouts loadouts = new PlayerData.PlayerLoadouts();

	// Token: 0x040017DC RID: 6108
	[SerializeField]
	private bool _isHardModeAvailable;

	// Token: 0x040017DD RID: 6109
	[SerializeField]
	private bool _isHardModeAvailableDLC;

	// Token: 0x040017DE RID: 6110
	[SerializeField]
	private bool _isTutorialCompleted;

	// Token: 0x040017DF RID: 6111
	[SerializeField]
	private bool _isFlyingTutorialCompleted;

	// Token: 0x040017E0 RID: 6112
	[SerializeField]
	private bool _isChaliceTutorialCompleted;

	// Token: 0x040017E1 RID: 6113
	[SerializeField]
	private PlayerData.PlayerInventories inventories = new PlayerData.PlayerInventories();

	// Token: 0x040017E2 RID: 6114
	public string dialoguerState;

	// Token: 0x040017E3 RID: 6115
	[SerializeField]
	public PlayerData.PlayerCoinManager coinManager = new PlayerData.PlayerCoinManager();

	// Token: 0x040017E4 RID: 6116
	private PlayerData.PlayerCoinManager levelCoinManager = new PlayerData.PlayerCoinManager();

	// Token: 0x040017E5 RID: 6117
	public bool unlockedBlackAndWhite;

	// Token: 0x040017E6 RID: 6118
	public bool unlocked2Strip;

	// Token: 0x040017E7 RID: 6119
	public bool unlockedChaliceRecolor;

	// Token: 0x040017E8 RID: 6120
	public bool vintageAudioEnabled;

	// Token: 0x040017E9 RID: 6121
	public bool pianoAudioEnabled;

	// Token: 0x040017EA RID: 6122
	public BlurGamma.Filter filter;

	// Token: 0x040017EB RID: 6123
	[SerializeField]
	private PlayerData.MapDataManager mapDataManager = new PlayerData.MapDataManager();

	// Token: 0x040017EC RID: 6124
	[SerializeField]
	private PlayerData.PlayerLevelDataManager levelDataManager = new PlayerData.PlayerLevelDataManager();

	// Token: 0x040017ED RID: 6125
	[SerializeField]
	private PlayerData.PlayerStats statictics = new PlayerData.PlayerStats();

	// Token: 0x040017EE RID: 6126
	[CompilerGenerated]
	private static InitializeCloudStoreHandler <>f__mg$cache0;

	// Token: 0x040017EF RID: 6127
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache1;

	// Token: 0x040017F0 RID: 6128
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache2;

	// Token: 0x040017F1 RID: 6129
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache3;

	// Token: 0x040017F2 RID: 6130
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache4;

	// Token: 0x0200040C RID: 1036
	public struct LevelCoinIds
	{
		// Token: 0x06000EDD RID: 3805 RVA: 0x00095FBD File Offset: 0x000943BD
		public LevelCoinIds(Levels level, string[][] coins)
		{
			this.levelId = level;
			this.coinIds = coins;
		}

		// Token: 0x040017F3 RID: 6131
		public Levels levelId;

		// Token: 0x040017F4 RID: 6132
		public string[][] coinIds;
	}

	// Token: 0x0200040D RID: 1037
	// (Invoke) Token: 0x06000EDF RID: 3807
	public delegate void PlayerDataInitHandler(bool success);

	// Token: 0x0200040E RID: 1038
	[Serializable]
	public class PlayerLoadouts
	{
		// Token: 0x06000EE2 RID: 3810 RVA: 0x00095FCD File Offset: 0x000943CD
		public PlayerLoadouts()
		{
			this.playerOne = new PlayerData.PlayerLoadouts.PlayerLoadout();
			this.playerTwo = new PlayerData.PlayerLoadouts.PlayerLoadout();
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00095FEB File Offset: 0x000943EB
		public PlayerLoadouts(PlayerData.PlayerLoadouts.PlayerLoadout playerOne, PlayerData.PlayerLoadouts.PlayerLoadout playerTwo)
		{
			this.playerOne = playerOne;
			this.playerTwo = playerTwo;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x00096001 File Offset: 0x00094401
		public PlayerData.PlayerLoadouts.PlayerLoadout GetPlayerLoadout(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x040017F5 RID: 6133
		public PlayerData.PlayerLoadouts.PlayerLoadout playerOne;

		// Token: 0x040017F6 RID: 6134
		public PlayerData.PlayerLoadouts.PlayerLoadout playerTwo;

		// Token: 0x0200040F RID: 1039
		[Serializable]
		public class PlayerLoadout
		{
			// Token: 0x06000EE5 RID: 3813 RVA: 0x00096024 File Offset: 0x00094424
			public PlayerLoadout()
			{
				this.primaryWeapon = Weapon.level_weapon_peashot;
				this.secondaryWeapon = Weapon.None;
				this.super = Super.None;
				this.charm = Charm.None;
			}

			// Token: 0x1700025E RID: 606
			// (get) Token: 0x06000EE6 RID: 3814 RVA: 0x00096058 File Offset: 0x00094458
			// (set) Token: 0x06000EE7 RID: 3815 RVA: 0x00096060 File Offset: 0x00094460
			public bool HasEquippedSecondaryRegularWeapon { get; set; }

			// Token: 0x1700025F RID: 607
			// (get) Token: 0x06000EE8 RID: 3816 RVA: 0x00096069 File Offset: 0x00094469
			// (set) Token: 0x06000EE9 RID: 3817 RVA: 0x00096071 File Offset: 0x00094471
			public bool HasEquippedSecondarySHMUPWeapon { get; set; }

			// Token: 0x17000260 RID: 608
			// (get) Token: 0x06000EEA RID: 3818 RVA: 0x0009607A File Offset: 0x0009447A
			// (set) Token: 0x06000EEB RID: 3819 RVA: 0x00096082 File Offset: 0x00094482
			public bool MustNotifySwitchRegularWeapon { get; set; }

			// Token: 0x17000261 RID: 609
			// (get) Token: 0x06000EEC RID: 3820 RVA: 0x0009608B File Offset: 0x0009448B
			// (set) Token: 0x06000EED RID: 3821 RVA: 0x00096093 File Offset: 0x00094493
			public bool MustNotifySwitchSHMUPWeapon { get; set; }

			// Token: 0x040017F7 RID: 6135
			public Weapon primaryWeapon;

			// Token: 0x040017F8 RID: 6136
			public Weapon secondaryWeapon;

			// Token: 0x040017F9 RID: 6137
			public Super super;

			// Token: 0x040017FA RID: 6138
			public Charm charm;
		}
	}

	// Token: 0x02000410 RID: 1040
	[Serializable]
	public class PlayerInventories
	{
		// Token: 0x06000EEF RID: 3823 RVA: 0x000960BA File Offset: 0x000944BA
		public PlayerData.PlayerInventory GetPlayer(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x040017FF RID: 6143
		public int dummy;

		// Token: 0x04001800 RID: 6144
		public PlayerData.PlayerInventory playerOne = new PlayerData.PlayerInventory();

		// Token: 0x04001801 RID: 6145
		public PlayerData.PlayerInventory playerTwo = new PlayerData.PlayerInventory();
	}

	// Token: 0x02000411 RID: 1041
	[Serializable]
	public class PlayerInventory
	{
		// Token: 0x06000EF0 RID: 3824 RVA: 0x000960E0 File Offset: 0x000944E0
		public PlayerInventory()
		{
			this.money = 0;
			this._weapons = new List<Weapon>();
			this._supers = new List<Super>();
			this._charms = new List<Charm>();
			this._weapons.Add(Weapon.level_weapon_peashot);
			this._weapons.Add(Weapon.plane_weapon_peashot);
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x0009613B File Offset: 0x0009453B
		public bool IsUnlocked(Weapon weapon)
		{
			return this._weapons.Contains(weapon);
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x00096149 File Offset: 0x00094549
		public bool IsUnlocked(Super super)
		{
			return this._supers.Contains(super);
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x00096157 File Offset: 0x00094557
		public bool IsUnlocked(Charm charm)
		{
			return this._charms.Contains(charm);
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x00096168 File Offset: 0x00094568
		public bool Buy(Weapon value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._weapons.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x000961C0 File Offset: 0x000945C0
		public bool Buy(Super value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._supers.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00096218 File Offset: 0x00094618
		public bool Buy(Charm value)
		{
			if (this.IsUnlocked(value))
			{
				return false;
			}
			if (this.money < WeaponProperties.GetValue(value))
			{
				return false;
			}
			this.money -= WeaponProperties.GetValue(value);
			this._charms.Add(value);
			this.newPurchase = true;
			return true;
		}

		// Token: 0x04001802 RID: 6146
		public const int STARTING_MONEY = 0;

		// Token: 0x04001803 RID: 6147
		public int money;

		// Token: 0x04001804 RID: 6148
		public bool newPurchase;

		// Token: 0x04001805 RID: 6149
		public List<Weapon> _weapons;

		// Token: 0x04001806 RID: 6150
		public List<Super> _supers;

		// Token: 0x04001807 RID: 6151
		public List<Charm> _charms;
	}

	// Token: 0x02000412 RID: 1042
	[Serializable]
	public class PlayerCoinManager
	{
		// Token: 0x06000EF7 RID: 3831 RVA: 0x00096270 File Offset: 0x00094670
		public PlayerCoinManager()
		{
			this.LevelsAndCoins = new List<PlayerData.PlayerCoinManager.LevelAndCoins>();
			IEnumerator enumerator = Enum.GetValues(typeof(Levels)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Levels level = (Levels)obj;
					PlayerData.PlayerCoinManager.LevelAndCoins levelAndCoins = new PlayerData.PlayerCoinManager.LevelAndCoins();
					levelAndCoins.level = level;
					this.LevelsAndCoins.Add(levelAndCoins);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00096318 File Offset: 0x00094718
		public bool GetCoinCollected(LevelCoin coin)
		{
			return this.GetCoinCollected(coin.GlobalID);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00096326 File Offset: 0x00094726
		public bool GetCoinCollected(string coinID)
		{
			return this.ContainsCoin(coinID) && this.GetCoin(coinID).collected;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00096344 File Offset: 0x00094744
		public int NumCoinsCollected()
		{
			int num = 0;
			foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.coins)
			{
				if (playerCoinProperties.collected)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000EFB RID: 3835 RVA: 0x000963AC File Offset: 0x000947AC
		public int NumCoinsCollected(bool DLC)
		{
			int num = 0;
			foreach (PlayerData.PlayerCoinProperties playerCoinProperties in this.coins)
			{
				if (playerCoinProperties.collected && this.IsDLCCoin(playerCoinProperties.coinID) == DLC)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000EFC RID: 3836 RVA: 0x00096428 File Offset: 0x00094828
		public void SetCoinValue(LevelCoin coin, bool collected, PlayerId player)
		{
			this.SetCoinValue(coin.GlobalID, collected, player);
		}

		// Token: 0x06000EFD RID: 3837 RVA: 0x00096438 File Offset: 0x00094838
		public void SetCoinValue(string coinID, bool collected, PlayerId player)
		{
			if (this.ContainsCoin(coinID))
			{
				PlayerData.PlayerCoinProperties coin = this.GetCoin(coinID);
				coin.collected = collected;
				coin.player = player;
			}
			else
			{
				this.AddCoin(new PlayerData.PlayerCoinProperties(coinID)
				{
					collected = collected
				});
			}
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00096481 File Offset: 0x00094881
		private PlayerData.PlayerCoinProperties GetCoin(LevelCoin coin)
		{
			return this.GetCoin(coin.GlobalID);
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00096490 File Offset: 0x00094890
		private PlayerData.PlayerCoinProperties GetCoin(string coinID)
		{
			for (int i = 0; i < this.coins.Count; i++)
			{
				if (this.coins[i].coinID == coinID)
				{
					return this.coins[i];
				}
			}
			return null;
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x000964E3 File Offset: 0x000948E3
		private void AddCoin(LevelCoin coin)
		{
			this.AddCoin(coin.GlobalID);
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x000964F1 File Offset: 0x000948F1
		private void AddCoin(string coinID)
		{
			if (!this.ContainsCoin(coinID))
			{
				this.coins.Add(new PlayerData.PlayerCoinProperties(coinID));
			}
			this.RegisterCoin(coinID);
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x00096517 File Offset: 0x00094917
		private void AddCoin(PlayerData.PlayerCoinProperties coin)
		{
			if (!this.ContainsCoin(coin.coinID))
			{
				this.coins.Add(coin);
			}
			this.RegisterCoin(coin.coinID);
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00096544 File Offset: 0x00094944
		private void RegisterCoin(string coinID)
		{
			PlatformingLevel platformingLevel = Level.Current as PlatformingLevel;
			if (platformingLevel)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins = this.LevelsAndCoins;
				int num = -1;
				for (int i = 0; i < levelsAndCoins.Count; i++)
				{
					if (levelsAndCoins[i].level == platformingLevel.CurrentLevel)
					{
						num = i;
					}
				}
				if (num >= 0)
				{
					for (int j = 0; j < platformingLevel.LevelCoinsIDs.Count; j++)
					{
						if (platformingLevel.LevelCoinsIDs[j].CoinID == coinID)
						{
							switch (j)
							{
							case 0:
								levelsAndCoins[num].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins[num].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins[num].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins[num].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins[num].Coin5Collected = true;
								break;
							}
							break;
						}
					}
				}
			}
			else if (Map.Current != null)
			{
				List<PlayerData.PlayerCoinManager.LevelAndCoins> levelsAndCoins2 = PlayerData.Data.coinManager.LevelsAndCoins;
				int num2 = -1;
				for (int k = 0; k < levelsAndCoins2.Count; k++)
				{
					if (levelsAndCoins2[k].level == Map.Current.level)
					{
						num2 = k;
					}
				}
				if (num2 >= 0)
				{
					for (int l = 0; l < Map.Current.LevelCoinsIDs.Count; l++)
					{
						if (Map.Current.LevelCoinsIDs[l].CoinID == coinID)
						{
							switch (l)
							{
							case 0:
								levelsAndCoins2[num2].Coin1Collected = true;
								break;
							case 1:
								levelsAndCoins2[num2].Coin2Collected = true;
								break;
							case 2:
								levelsAndCoins2[num2].Coin3Collected = true;
								break;
							case 3:
								levelsAndCoins2[num2].Coin4Collected = true;
								break;
							case 4:
								levelsAndCoins2[num2].Coin5Collected = true;
								break;
							}
							break;
						}
					}
				}
			}
			bool flag = true;
			foreach (Levels level in Level.platformingLevels)
			{
				if (PlayerData.Data.GetNumCoinsCollectedInLevel(level) < 5)
				{
					flag = false;
				}
			}
			if (flag)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "FoundAllLevelMoney");
			}
			if (PlayerData.Data.NumCoinsCollectedMainGame >= 40)
			{
				OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "FoundAllMoney");
			}
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x00096830 File Offset: 0x00094C30
		private bool ContainsCoin(LevelCoin coin)
		{
			return this.ContainsCoin(coin.GlobalID);
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00096840 File Offset: 0x00094C40
		private bool ContainsCoin(string coinID)
		{
			for (int i = 0; i < this.coins.Count; i++)
			{
				if (this.coins[i].coinID == coinID)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00096888 File Offset: 0x00094C88
		private bool IsDLCCoin(LevelCoin coin)
		{
			return this.IsDLCCoin(coin.GlobalID);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00096898 File Offset: 0x00094C98
		private bool IsDLCCoin(string coinID)
		{
			return coinID == "619e92f1-e0fd-4f6e-9c2d-5ce5dbaf393f" || coinID == "scene_level_chalice_tutorial::Level_Coin :: 578c0218-df9e-4cdd-932a-a1277b5b7129" || coinID == "a37b3d37-a32e-4b88-a583-34489496494d" || coinID == "25f15554-d229-4330-96cc-ac8a13c18ea0" || coinID == "eacf4228-e200-4839-9d79-3439cfcc5824" || coinID == "47f7edb1-b5c5-4afb-9acb-a46f5e6df557" || coinID == "3826615a-498b-4158-af7b-0d01acbc18c8" || coinID == "d52b1cc6-414c-4a7c-9f8a-250316566d58" || coinID == "fc2c48cd-5dec-472a-ae18-dccfc94232c6" || coinID == "16732bc8-7230-467a-a9ac-ff9c62ab7657" || coinID == "e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c" || coinID == "19090606-09e8-4e56-92ac-e08200926b94" || coinID == "39bfe6d8-0dbc-4886-9998-52c67b57969e" || coinID == "7f3422f5-6650-497f-9c35-9735b64100d6" || coinID == "9970ad6a-560a-4ae3-9d15-a6b636b67024" || coinID == "3367b9b0-da35-4c81-a895-2720862b5b1b";
		}

		// Token: 0x04001808 RID: 6152
		public int dummy;

		// Token: 0x04001809 RID: 6153
		public List<PlayerData.PlayerCoinProperties> coins = new List<PlayerData.PlayerCoinProperties>();

		// Token: 0x0400180A RID: 6154
		public bool hasMigratedCoins;

		// Token: 0x0400180B RID: 6155
		public List<PlayerData.PlayerCoinManager.LevelAndCoins> LevelsAndCoins = new List<PlayerData.PlayerCoinManager.LevelAndCoins>();

		// Token: 0x02000413 RID: 1043
		[Serializable]
		public class LevelAndCoins
		{
			// Token: 0x0400180C RID: 6156
			public Levels level;

			// Token: 0x0400180D RID: 6157
			public bool Coin1Collected;

			// Token: 0x0400180E RID: 6158
			public bool Coin2Collected;

			// Token: 0x0400180F RID: 6159
			public bool Coin3Collected;

			// Token: 0x04001810 RID: 6160
			public bool Coin4Collected;

			// Token: 0x04001811 RID: 6161
			public bool Coin5Collected;
		}
	}

	// Token: 0x02000414 RID: 1044
	[Serializable]
	public class PlayerCoinProperties
	{
		// Token: 0x06000F09 RID: 3849 RVA: 0x000969AB File Offset: 0x00094DAB
		public PlayerCoinProperties()
		{
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x000969C9 File Offset: 0x00094DC9
		public PlayerCoinProperties(LevelCoin coin)
		{
			this.coinID = coin.GlobalID;
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x000969F3 File Offset: 0x00094DF3
		public PlayerCoinProperties(string coinID)
		{
			this.coinID = coinID;
		}

		// Token: 0x04001812 RID: 6162
		public string coinID = string.Empty;

		// Token: 0x04001813 RID: 6163
		public bool collected;

		// Token: 0x04001814 RID: 6164
		public PlayerId player = PlayerId.None;
	}

	// Token: 0x02000415 RID: 1045
	[Serializable]
	public class MapData
	{
		// Token: 0x04001815 RID: 6165
		public Scenes mapId;

		// Token: 0x04001816 RID: 6166
		public bool sessionStarted;

		// Token: 0x04001817 RID: 6167
		public bool hasVisitedDieHouse;

		// Token: 0x04001818 RID: 6168
		public bool hasKingDiceDisappeared;

		// Token: 0x04001819 RID: 6169
		public Vector3 playerOnePosition = Vector3.zero;

		// Token: 0x0400181A RID: 6170
		public Vector3 playerTwoPosition = Vector3.zero;

		// Token: 0x0400181B RID: 6171
		[NonSerialized]
		public PlayerData.MapData.EntryMethod enteringFrom;

		// Token: 0x02000416 RID: 1046
		public enum EntryMethod
		{
			// Token: 0x0400181D RID: 6173
			None,
			// Token: 0x0400181E RID: 6174
			DiceHouseLeft,
			// Token: 0x0400181F RID: 6175
			DiceHouseRight,
			// Token: 0x04001820 RID: 6176
			Boatman
		}
	}

	// Token: 0x02000417 RID: 1047
	[Serializable]
	public class MapDataManager
	{
		// Token: 0x06000F0D RID: 3853 RVA: 0x00096A36 File Offset: 0x00094E36
		public MapDataManager()
		{
			this.mapData = new List<PlayerData.MapData>();
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x00096A50 File Offset: 0x00094E50
		public PlayerData.MapData GetCurrentMapData()
		{
			return this.GetMapData(this.currentMap);
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00096A60 File Offset: 0x00094E60
		public PlayerData.MapData GetMapData(Scenes map)
		{
			for (int i = 0; i < this.mapData.Count; i++)
			{
				if (this.mapData[i].mapId == map)
				{
					return this.mapData[i];
				}
			}
			PlayerData.MapData mapData = new PlayerData.MapData();
			mapData.mapId = map;
			this.mapData.Add(mapData);
			return mapData;
		}

		// Token: 0x04001821 RID: 6177
		public Scenes currentMap = Scenes.scene_map_world_1;

		// Token: 0x04001822 RID: 6178
		public List<PlayerData.MapData> mapData;
	}

	// Token: 0x02000418 RID: 1048
	[Serializable]
	public class PlayerLevelDataManager
	{
		// Token: 0x06000F10 RID: 3856 RVA: 0x00096AC8 File Offset: 0x00094EC8
		public PlayerLevelDataManager()
		{
			this.levelObjects = new List<PlayerData.PlayerLevelDataObject>();
			foreach (Levels levels in EnumUtils.GetValues<Levels>())
			{
				PlayerData.PlayerLevelDataObject playerLevelDataObject = new PlayerData.PlayerLevelDataObject(levels);
				playerLevelDataObject.levelID = levels;
				this.levelObjects.Add(playerLevelDataObject);
			}
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x00096B20 File Offset: 0x00094F20
		public PlayerData.PlayerLevelDataObject GetLevelData(Levels levelID)
		{
			for (int i = 0; i < this.levelObjects.Count; i++)
			{
				if (this.levelObjects[i].levelID == levelID)
				{
					return this.levelObjects[i];
				}
			}
			PlayerData.PlayerLevelDataObject playerLevelDataObject = new PlayerData.PlayerLevelDataObject(levelID);
			this.levelObjects.Add(playerLevelDataObject);
			return playerLevelDataObject;
		}

		// Token: 0x04001823 RID: 6179
		public int dummy;

		// Token: 0x04001824 RID: 6180
		public List<PlayerData.PlayerLevelDataObject> levelObjects;
	}

	// Token: 0x02000419 RID: 1049
	[Serializable]
	public class PlayerLevelDataObject
	{
		// Token: 0x06000F12 RID: 3858 RVA: 0x00096B81 File Offset: 0x00094F81
		public PlayerLevelDataObject(Levels id)
		{
			this.levelID = id;
		}

		// Token: 0x04001825 RID: 6181
		public Levels levelID;

		// Token: 0x04001826 RID: 6182
		public bool completed;

		// Token: 0x04001827 RID: 6183
		public bool completedAsChaliceP1;

		// Token: 0x04001828 RID: 6184
		public bool completedAsChaliceP2;

		// Token: 0x04001829 RID: 6185
		public bool played;

		// Token: 0x0400182A RID: 6186
		public LevelScoringData.Grade grade;

		// Token: 0x0400182B RID: 6187
		public Level.Mode difficultyBeaten;

		// Token: 0x0400182C RID: 6188
		public float bestTime = float.MaxValue;

		// Token: 0x0400182D RID: 6189
		public bool curseCharmP1;

		// Token: 0x0400182E RID: 6190
		public bool curseCharmP2;

		// Token: 0x0400182F RID: 6191
		public int bgmPlayListCurrent;
	}

	// Token: 0x0200041A RID: 1050
	[Serializable]
	public class PlayerStats
	{
		// Token: 0x06000F14 RID: 3860 RVA: 0x00096BB9 File Offset: 0x00094FB9
		public PlayerData.PlayerStat GetPlayer(PlayerId player)
		{
			if (player == PlayerId.PlayerOne)
			{
				return this.playerOne;
			}
			if (player != PlayerId.PlayerTwo)
			{
				return null;
			}
			return this.playerTwo;
		}

		// Token: 0x04001830 RID: 6192
		public int dummy;

		// Token: 0x04001831 RID: 6193
		public PlayerData.PlayerStat playerOne = new PlayerData.PlayerStat();

		// Token: 0x04001832 RID: 6194
		public PlayerData.PlayerStat playerTwo = new PlayerData.PlayerStat();
	}

	// Token: 0x0200041B RID: 1051
	[Serializable]
	public class PlayerStat
	{
		// Token: 0x06000F15 RID: 3861 RVA: 0x00096BDC File Offset: 0x00094FDC
		public PlayerStat()
		{
			this.numDeaths = 0;
			this.numParriesInRow = 0;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x00096BF2 File Offset: 0x00094FF2
		public int DeathCount()
		{
			return this.numDeaths;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x00096BFA File Offset: 0x00094FFA
		public void Die()
		{
			this.numDeaths++;
		}

		// Token: 0x04001833 RID: 6195
		public int numDeaths;

		// Token: 0x04001834 RID: 6196
		public int numParriesInRow;
	}
}
