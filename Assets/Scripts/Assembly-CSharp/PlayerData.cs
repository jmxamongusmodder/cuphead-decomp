using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
	[Serializable]
	public class PlayerLoadouts
	{
		[Serializable]
		public class PlayerLoadout
		{
			public Weapon primaryWeapon;
			public Weapon secondaryWeapon;
			public Super super;
			public Charm charm;
		}

		public PlayerLoadout playerOne;
		public PlayerLoadout playerTwo;
	}

	[Serializable]
	public class PlayerInventory
	{
		public int money;
		public bool newPurchase;
		public List<Weapon> _weapons;
		public List<Super> _supers;
		public List<Charm> _charms;
	}

	[Serializable]
	public class PlayerInventories
	{
		public int dummy;
		public PlayerData.PlayerInventory playerOne;
		public PlayerData.PlayerInventory playerTwo;
	}

	[Serializable]
	public class PlayerCoinProperties
	{
		public string coinID;
		public bool collected;
		public PlayerId player;
	}

	[Serializable]
	public class PlayerCoinManager
	{
		[Serializable]
		public class LevelAndCoins
		{
			public Levels level;
			public bool Coin1Collected;
			public bool Coin2Collected;
			public bool Coin3Collected;
			public bool Coin4Collected;
			public bool Coin5Collected;
		}

		public int dummy;
		public List<PlayerData.PlayerCoinProperties> coins;
		public bool hasMigratedCoins;
		public List<PlayerData.PlayerCoinManager.LevelAndCoins> LevelsAndCoins;
	}

	[Serializable]
	public class MapData
	{
		public Scenes mapId;
		public bool sessionStarted;
		public bool hasVisitedDieHouse;
		public bool hasKingDiceDisappeared;
		public Vector3 playerOnePosition;
		public Vector3 playerTwoPosition;
	}

	[Serializable]
	public class MapDataManager
	{
		public Scenes currentMap;
		public List<PlayerData.MapData> mapData;
	}

	[Serializable]
	public class PlayerLevelDataObject
	{
		public PlayerLevelDataObject(Levels id)
		{
		}

		public Levels levelID;
		public bool completed;
		public bool completedAsChaliceP1;
		public bool completedAsChaliceP2;
		public bool played;
		public LevelScoringData.Grade grade;
		public Level.Mode difficultyBeaten;
		public float bestTime;
		public bool curseCharmP1;
		public bool curseCharmP2;
		public int bgmPlayListCurrent;
	}

	[Serializable]
	public class PlayerLevelDataManager
	{
		public int dummy;
		public List<PlayerData.PlayerLevelDataObject> levelObjects;
	}

	[Serializable]
	public class PlayerStat
	{
		public int numDeaths;
		public int numParriesInRow;
	}

	[Serializable]
	public class PlayerStats
	{
		public int dummy;
		public PlayerData.PlayerStat playerOne;
		public PlayerData.PlayerStat playerTwo;
	}

	public bool isPlayer1Mugman;
	public bool hasMadeFirstPurchase;
	public bool hasBeatenAnyBossOnEasy;
	public bool hasBeatenAnyDLCBossOnEasy;
	public bool hasUnlockedFirstSuper;
	public bool shouldShowShopkeepTooltip;
	public bool shouldShowTurtleTooltip;
	public bool shouldShowCanteenTooltip;
	public bool shouldShowForkTooltip;
	public bool shouldShowKineDiceTooltip;
	public bool shouldShowMausoleumTooltip;
	public bool hasUnlockedBoatman;
	public bool shouldShowBoatmanTooltip;
	public bool shouldShowChaliceTooltip;
	public bool hasTalkedToChaliceFan;
	public int[] curseCharmPuzzleOrder;
	public bool curseCharmPuzzleComplete;
	public MapCastleZones.Zone currentChessBossZone;
	public List<MapCastleZones.Zone> usedChessBossZones;
	public int chessBossAttemptCounter;
	public bool djimmiActivatedCountedWish;
	public bool djimmiActivatedInfiniteWishBaseGame;
	public bool djimmiActivatedInfiniteWishDLC;
	public int djimmiWishes;
	public bool djimmiFreed;
	public bool djimmiFreedDLC;
	public int dummy;
	[SerializeField]
	private PlayerLoadouts loadouts;
	[SerializeField]
	private bool _isHardModeAvailable;
	[SerializeField]
	private bool _isHardModeAvailableDLC;
	[SerializeField]
	private bool _isTutorialCompleted;
	[SerializeField]
	private bool _isFlyingTutorialCompleted;
	[SerializeField]
	private bool _isChaliceTutorialCompleted;
	[SerializeField]
	private PlayerInventories inventories;
	public string dialoguerState;
	[SerializeField]
	public PlayerCoinManager coinManager;
	public bool unlockedBlackAndWhite;
	public bool unlocked2Strip;
	public bool unlockedChaliceRecolor;
	public bool vintageAudioEnabled;
	public bool pianoAudioEnabled;
	public BlurGamma.Filter filter;
	[SerializeField]
	private MapDataManager mapDataManager;
	[SerializeField]
	private PlayerLevelDataManager levelDataManager;
	[SerializeField]
	private PlayerStats statictics;
}
