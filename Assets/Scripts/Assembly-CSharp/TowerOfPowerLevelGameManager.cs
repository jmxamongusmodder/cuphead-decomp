using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000803 RID: 2051
public class TowerOfPowerLevelGameManager : LevelProperties.TowerOfPower.Entity
{
	// Token: 0x06002F70 RID: 12144 RVA: 0x001C1BE4 File Offset: 0x001BFFE4
	public override void LevelInit(LevelProperties.TowerOfPower properties)
	{
		base.LevelInit(properties);
		this.anyInput = new CupheadInput.AnyPlayerInput(false);
		TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER;
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN == 0)
		{
			TowerOfPowerLevelGameInfo.baseDifficulty = Level.Current.mode;
			TowerOfPowerLevelGameInfo.SetDefaultToken(properties.CurrentState.slotMachine.DefaultStartingToken);
			TowerOfPowerLevelGameInfo.MIN_RANK_NEED_TO_GET_TOKEN = properties.CurrentState.slotMachine.MinRankToGainToken;
			this.InitDifficultyBossByIndex();
			this.InitPools();
			this.SetTowerBosses();
			this.InitSlotMachine();
			TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerOne);
			if (PlayerManager.Multiplayer)
			{
				TowerOfPowerLevelGameInfo.InitEquipment(PlayerId.PlayerTwo);
			}
			if (this.debugSkipToLastFight)
			{
				TowerOfPowerLevelGameInfo.TURN_COUNTER = TowerOfPowerLevelGameInfo.allStageSpaces.Count - 1;
				TowerOfPowerLevelGameInfo.CURRENT_TURN = TowerOfPowerLevelGameInfo.TURN_COUNTER;
			}
		}
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002F71 RID: 12145 RVA: 0x001C1CB8 File Offset: 0x001C00B8
	public void ChangePlayersWeapon(PlayerId playerId)
	{
		if (playerId == PlayerId.PlayerTwo && !PlayerManager.Multiplayer)
		{
			return;
		}
		bool flag = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm == Charm.charm_chalice;
		this.bonusHP[(int)playerId] = 0;
		this.bonusToken[(int)playerId] = 0;
		this.SlotMachineWeapon2Attempt[(int)playerId] = 0;
		TowerOfPowerLevelGameManager.Weapon_Slot weaponSlotEnumByName = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotOne.RandomChoice<string>());
		int count = TowerOfPowerLevelGameInfo.SlotTwo.Count;
		int num = UnityEngine.Random.Range(0, count);
		TowerOfPowerLevelGameManager.Weapon_Slot weaponSlotEnumByName2 = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo[num]);
		while (weaponSlotEnumByName2 == weaponSlotEnumByName)
		{
			num++;
			this.SlotMachineWeapon2Attempt[(int)playerId]++;
			if (num >= count)
			{
				num = 0;
			}
			weaponSlotEnumByName2 = TowerOfPowerLevelGameManager.GetWeaponSlotEnumByName(TowerOfPowerLevelGameInfo.SlotTwo[num]);
			if (this.SlotMachineWeapon2Attempt[(int)playerId] == count)
			{
				global::Debug.LogError("The slotTwo list needs at least two kinds of weapon. Modify the Tower of Power in the Level Editor--Slot Two weapon in the SlotMachine section.", null);
				break;
			}
		}
		TowerOfPowerLevelGameManager.Charm_Slot charmSlotEnumByName;
		TowerOfPowerLevelGameManager.Super_Slot superSlotEnumByName;
		if (flag)
		{
			charmSlotEnumByName = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThreeChalice.RandomChoice<string>());
			superSlotEnumByName = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFourChalice.RandomChoice<string>());
		}
		else
		{
			charmSlotEnumByName = TowerOfPowerLevelGameManager.GetCharmSlotEnumByName(TowerOfPowerLevelGameInfo.SlotThree.RandomChoice<string>());
			superSlotEnumByName = TowerOfPowerLevelGameManager.GetSuperSlotEnumByName(TowerOfPowerLevelGameInfo.SlotFour.RandomChoice<string>());
		}
		if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_extra_token)
		{
			this.bonusToken[(int)playerId] = 1;
		}
		if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_1)
		{
			this.bonusHP[(int)playerId] = 1;
		}
		else if (charmSlotEnumByName == TowerOfPowerLevelGameManager.Charm_Slot.charm_health_up_2)
		{
			this.bonusHP[(int)playerId] = 2;
		}
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
		playerLoadout.primaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weaponSlotEnumByName.ToString());
		playerLoadout.secondaryWeapon = TowerOfPowerLevelGameManager.GetWeaponEnumByName(weaponSlotEnumByName2.ToString());
		playerLoadout.charm = TowerOfPowerLevelGameManager.GetCharmEnumByName(charmSlotEnumByName.ToString());
		playerLoadout.super = TowerOfPowerLevelGameManager.GetSuperEnumByName(superSlotEnumByName.ToString());
	}

	// Token: 0x06002F72 RID: 12146 RVA: 0x001C1E9C File Offset: 0x001C029C
	private IEnumerator startMiniBoss_cr(Levels level)
	{
		TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerOne);
		if (PlayerManager.Multiplayer)
		{
			TowerOfPowerLevelGameInfo.SetPlayersStats(PlayerId.PlayerTwo);
		}
		Level.ScoringData.time += Level.Current.LevelTime;
		SceneLoader.LoadLevel(level, SceneLoader.Transition.Fade, SceneLoader.Icon.Hourglass, null);
		yield return null;
		yield break;
	}

	// Token: 0x06002F73 RID: 12147 RVA: 0x001C1EB8 File Offset: 0x001C02B8
	public static TowerOfPowerLevelGameManager.Weapon_Slot GetWeaponSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Weapon_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Weapon_Slot), Name);
	}

	// Token: 0x06002F74 RID: 12148 RVA: 0x001C1EDC File Offset: 0x001C02DC
	public static TowerOfPowerLevelGameManager.Charm_Slot GetCharmSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Charm_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Charm_Slot), Name);
	}

	// Token: 0x06002F75 RID: 12149 RVA: 0x001C1F00 File Offset: 0x001C0300
	public static TowerOfPowerLevelGameManager.Super_Slot GetSuperSlotEnumByName(string Name)
	{
		return (TowerOfPowerLevelGameManager.Super_Slot)Enum.Parse(typeof(TowerOfPowerLevelGameManager.Super_Slot), Name);
	}

	// Token: 0x06002F76 RID: 12150 RVA: 0x001C1F24 File Offset: 0x001C0324
	public static Weapon GetWeaponEnumByName(string Name)
	{
		Weapon result = Weapon.None;
		if (Enum.IsDefined(typeof(Weapon), Name))
		{
			result = (Weapon)Enum.Parse(typeof(Weapon), Name);
		}
		return result;
	}

	// Token: 0x06002F77 RID: 12151 RVA: 0x001C1F64 File Offset: 0x001C0364
	public static Charm GetCharmEnumByName(string Name)
	{
		Charm result = Charm.None;
		if (Enum.IsDefined(typeof(Charm), Name))
		{
			result = (Charm)Enum.Parse(typeof(Charm), Name);
		}
		return result;
	}

	// Token: 0x06002F78 RID: 12152 RVA: 0x001C1FA4 File Offset: 0x001C03A4
	public static Super GetSuperEnumByName(string Name)
	{
		Super result = Super.None;
		if (Enum.IsDefined(typeof(Super), Name))
		{
			result = (Super)Enum.Parse(typeof(Super), Name);
		}
		return result;
	}

	// Token: 0x06002F79 RID: 12153 RVA: 0x001C1FE3 File Offset: 0x001C03E3
	private void InitPools()
	{
		this.InitBossPools();
		this.InitShmupPools();
		this.InitKingDicePools();
	}

	// Token: 0x06002F7A RID: 12154 RVA: 0x001C1FF8 File Offset: 0x001C03F8
	private void InitBossPools()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.PoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.PoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.PoolThreeString.Split(new char[]
		{
			','
		});
		this.BossPools = new List<Levels>[3];
		this.BossPools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.BossPools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.BossPools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.BossPools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.BossPools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.BossPools[2].Add(Level.GetEnumByName(array3[k]));
		}
	}

	// Token: 0x06002F7B RID: 12155 RVA: 0x001C2130 File Offset: 0x001C0530
	private void InitShmupPools()
	{
		this.ShmupPlacement.Clear();
		string[] array = base.properties.CurrentState.bossesPropertises.ShmupPoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.ShmupPoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.ShmupPoolThreeString.Split(new char[]
		{
			','
		});
		List<string> list = base.properties.CurrentState.bossesPropertises.ShmupPlacementString.Split(new char[]
		{
			','
		}).ToList<string>();
		string shmupCountString = base.properties.CurrentState.bossesPropertises.ShmupCountString;
		string[] array4 = shmupCountString.Split(new char[]
		{
			','
		});
		int num = Parser.IntParse(array4[UnityEngine.Random.Range(0, array4.Length)]);
		if (num > 0)
		{
			do
			{
				int placement = Parser.IntParse(list[UnityEngine.Random.Range(0, list.Count)]);
				this.ShmupPlacement.Add(placement);
				list.RemoveAll((string x) => x == placement.ToString());
			}
			while (this.ShmupPlacement.Count < num && list.Count != 0);
		}
		this.ShmupPools = new List<Levels>[3];
		this.ShmupPools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.ShmupPools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.ShmupPools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.ShmupPools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.ShmupPools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.ShmupPools[2].Add(Level.GetEnumByName(array3[k]));
		}
	}

	// Token: 0x06002F7C RID: 12156 RVA: 0x001C2354 File Offset: 0x001C0754
	private void InitKingDicePools()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.KingDicePoolOneString.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.bossesPropertises.KingDicePoolTwoString.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.bossesPropertises.KingDicePoolThreeString.Split(new char[]
		{
			','
		});
		string[] array4 = base.properties.CurrentState.bossesPropertises.KingDicePoolFourString.Split(new char[]
		{
			','
		});
		int kingDiceMiniBossCount = base.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount;
		this.KingDicePools = new List<Levels>[4];
		this.KingDicePools[0] = new List<Levels>();
		for (int i = 0; i < array.Length; i++)
		{
			this.KingDicePools[0].Add(Level.GetEnumByName(array[i]));
		}
		this.KingDicePools[1] = new List<Levels>();
		for (int j = 0; j < array2.Length; j++)
		{
			this.KingDicePools[1].Add(Level.GetEnumByName(array2[j]));
		}
		this.KingDicePools[2] = new List<Levels>();
		for (int k = 0; k < array3.Length; k++)
		{
			this.KingDicePools[2].Add(Level.GetEnumByName(array3[k]));
		}
		this.KingDicePools[3] = new List<Levels>();
		for (int l = 0; l < array4.Length; l++)
		{
			this.KingDicePools[3].Add(Level.GetEnumByName(array4[l]));
		}
	}

	// Token: 0x06002F7D RID: 12157 RVA: 0x001C2508 File Offset: 0x001C0908
	private void SetTowerBosses()
	{
		TowerOfPowerLevelGameInfo.allStageSpaces.Clear();
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				if (i == 2 && j == 2)
				{
					int kingDiceMiniBossCount = base.properties.CurrentState.bossesPropertises.KingDiceMiniBossCount;
					for (int k = 0; k < kingDiceMiniBossCount; k++)
					{
						this.SetKingDiceBosses(k);
					}
					TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.DicePalaceMain);
					if (base.properties.CurrentState.bossesPropertises.DevilFinalBoss)
					{
						TowerOfPowerLevelGameInfo.allStageSpaces.Add(Levels.Devil);
					}
				}
				else
				{
					int count = TowerOfPowerLevelGameInfo.allStageSpaces.Count;
					if (this.ShmupPlacement.Contains(count + 1))
					{
						this.AddShmupInTower(this.ShmupPlacement.IndexOf(count + 1));
					}
					else
					{
						this.AddBossInTower(i);
					}
				}
			}
		}
	}

	// Token: 0x06002F7E RID: 12158 RVA: 0x001C2600 File Offset: 0x001C0A00
	private void AddBossInTower(int tier)
	{
		this.BossPools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.BossPools[tier].Count == 0)
		{
			global::Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.BossPools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			global::Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.BossPools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002F7F RID: 12159 RVA: 0x001C26D0 File Offset: 0x001C0AD0
	private void AddShmupInTower(int tier)
	{
		this.ShmupPools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.ShmupPools[tier].Count == 0)
		{
			global::Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.ShmupPools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			global::Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like experted", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.ShmupPools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002F80 RID: 12160 RVA: 0x001C27A0 File Offset: 0x001C0BA0
	private void SetKingDiceBosses(int tier)
	{
		this.KingDicePools[tier].RemoveAll((Levels x) => TowerOfPowerLevelGameInfo.allStageSpaces.Contains(x));
		if (this.KingDicePools[tier].Count == 0)
		{
			global::Debug.LogError("Number of Boss in the pool " + tier + " is empty.", null);
			return;
		}
		Levels randLv = this.KingDicePools[tier].RandomChoice<Levels>();
		if (TowerOfPowerLevelGameInfo.allStageSpaces.Contains(randLv))
		{
			global::Debug.LogError("RemoveAll(x => allStageSpaces.Contains(x) don't work like expected", null);
		}
		else
		{
			TowerOfPowerLevelGameInfo.allStageSpaces.Add(randLv);
			this.KingDicePools[tier].RemoveAll((Levels x) => x == randLv);
		}
	}

	// Token: 0x06002F81 RID: 12161 RVA: 0x001C2870 File Offset: 0x001C0C70
	private void InitDifficultyBossByIndex()
	{
		string[] array = base.properties.CurrentState.bossesPropertises.MiniBossDifficultyByIndex.Split(new char[]
		{
			','
		});
		TowerOfPowerLevelGameInfo.difficultyByBossIndex = new Level.Mode[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			int num = 0;
			Parser.IntTryParse(array[i], out num);
			TowerOfPowerLevelGameInfo.difficultyByBossIndex[i] = (Level.Mode)num;
		}
	}

	// Token: 0x06002F82 RID: 12162 RVA: 0x001C28DC File Offset: 0x001C0CDC
	private void InitSlotMachine()
	{
		string[] array = base.properties.CurrentState.slotMachine.SlotOneWeapon.Split(new char[]
		{
			','
		});
		string[] array2 = base.properties.CurrentState.slotMachine.SlotTwoWeapon.Split(new char[]
		{
			','
		});
		string[] array3 = base.properties.CurrentState.slotMachine.SlotThreeCharm.Split(new char[]
		{
			','
		});
		string[] array4 = base.properties.CurrentState.slotMachine.SlotThreeChalice.Split(new char[]
		{
			','
		});
		string[] array5 = base.properties.CurrentState.slotMachine.SlotFourSuper.Split(new char[]
		{
			','
		});
		string[] array6 = base.properties.CurrentState.slotMachine.SlotFourChalice.Split(new char[]
		{
			','
		});
		for (int i = 0; i < array.Length; i++)
		{
			string item = (!(array[i] != "None")) ? array[i] : ("level_weapon_" + array[i]);
			TowerOfPowerLevelGameInfo.SlotOne.Add(item);
		}
		for (int j = 0; j < array2.Length; j++)
		{
			string item = (!(array2[j] != "None")) ? array2[j] : ("level_weapon_" + array2[j]);
			TowerOfPowerLevelGameInfo.SlotTwo.Add(item);
		}
		for (int k = 0; k < array3.Length; k++)
		{
			string item = (!(array3[k] != "None")) ? array3[k] : ("charm_" + array3[k]);
			TowerOfPowerLevelGameInfo.SlotThree.Add(item);
		}
		for (int l = 0; l < array4.Length; l++)
		{
			string item = (!(array4[l] != "None")) ? array4[l] : ("charm_" + array4[l]);
			TowerOfPowerLevelGameInfo.SlotThreeChalice.Add(item);
		}
		for (int m = 0; m < array5.Length; m++)
		{
			string item = (!(array5[m] != "None")) ? array5[m] : ("level_super_" + array5[m]);
			TowerOfPowerLevelGameInfo.SlotFour.Add(item);
		}
		for (int n = 0; n < array6.Length; n++)
		{
			string item = (!(array6[n] != "None")) ? array6[n] : ("level_super_chalice_" + array6[n]);
			TowerOfPowerLevelGameInfo.SlotFourChalice.Add(item);
		}
	}

	// Token: 0x06002F83 RID: 12163 RVA: 0x001C2BB8 File Offset: 0x001C0FB8
	private void SetDifficulty()
	{
		int num = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		if (num >= TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length)
		{
			num = TowerOfPowerLevelGameInfo.difficultyByBossIndex.Length - 1;
		}
		Level.SetCurrentMode(TowerOfPowerLevelGameInfo.difficultyByBossIndex[num]);
	}

	// Token: 0x06002F84 RID: 12164 RVA: 0x001C2BF0 File Offset: 0x001C0FF0
	private void RevivePlayer(PlayerId playerId)
	{
		PlayerStatsManager stats = PlayerManager.GetPlayer(playerId).stats;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = 3;
		TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = 3;
		stats.SetHealth(3);
	}

	// Token: 0x06002F85 RID: 12165 RVA: 0x001C2C2C File Offset: 0x001C102C
	private IEnumerator main_cr()
	{
		int turn = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		List<Levels> allStage = TowerOfPowerLevelGameInfo.allStageSpaces;
		if (turn > 0)
		{
			this.showingScorecard = true;
			while (SceneLoader.CurrentlyLoading)
			{
				yield return null;
			}
			this.scorecard.gameObject.SetActive(true);
			while (!this.scorecard.done)
			{
				yield return null;
			}
			this.showingScorecard = false;
			this.scorecard.gameObject.SetActive(false);
			if (PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead && TowerOfPowerLevelGameInfo.IsTokenLeft(0))
			{
				TowerOfPowerLevelGameInfo.ReduceToken(0);
				this.RevivePlayer(PlayerId.PlayerOne);
			}
			if (PlayerManager.Multiplayer && PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead && TowerOfPowerLevelGameInfo.IsTokenLeft(1))
			{
				TowerOfPowerLevelGameInfo.ReduceToken(1);
				this.RevivePlayer(PlayerId.PlayerTwo);
			}
		}
		if ((turn != 0 && turn % 3 == 0 && turn < 8) || allStage[turn] == Levels.Devil || this.debugForceSlotMachineEveryTurn || (turn == 1 && this.debugForceSlotMachineAfterOneFight))
		{
			if (!PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead)
			{
				this.ChangePlayersWeapon(PlayerId.PlayerOne);
				this.slotsDone[0] = false;
				base.StartCoroutine(this.play_slot_machine_cr(PlayerId.PlayerOne));
			}
			else
			{
				this.slotsDone[0] = true;
			}
			if (PlayerManager.Multiplayer && !PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead)
			{
				this.slotsDone[1] = false;
				this.ChangePlayersWeapon(PlayerId.PlayerTwo);
				base.StartCoroutine(this.play_slot_machine_cr(PlayerId.PlayerTwo));
			}
			else
			{
				this.slotsDone[1] = true;
			}
		}
		else
		{
			base.StartCoroutine(this.go_to_next_level_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002F86 RID: 12166 RVA: 0x001C2C48 File Offset: 0x001C1048
	private IEnumerator spin_slot_machine_cr(PlayerId playerId)
	{
		yield return null;
		yield break;
	}

	// Token: 0x06002F87 RID: 12167 RVA: 0x001C2C5C File Offset: 0x001C105C
	private IEnumerator stop_slot_machine_cr(PlayerId playerId)
	{
		for (;;)
		{
			if (PauseManager.state == PauseManager.State.Paused)
			{
				this.waitForButtonRelease = 3;
				yield return null;
			}
			if (PlayerManager.GetPlayer(playerId).input.actions.GetButtonUp(13))
			{
				this.waitForButtonRelease = 0;
			}
			if (this.waitForButtonRelease == 0 && PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(13))
			{
				break;
			}
			if (this.waitForButtonRelease > 0)
			{
				this.waitForButtonRelease--;
			}
			yield return this.slowdown_slots_cr();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002F88 RID: 12168 RVA: 0x001C2C80 File Offset: 0x001C1080
	private IEnumerator slowdown_slots_cr()
	{
		yield return null;
		yield break;
	}

	// Token: 0x06002F89 RID: 12169 RVA: 0x001C2C94 File Offset: 0x001C1094
	private IEnumerator play_slot_machine_cr(PlayerId playerId)
	{
		yield return this.spin_slot_machine_cr(playerId);
		this.slotsConfirm[(int)playerId] = false;
		this.slotsCanSpinAgain[(int)playerId] = false;
		this.slotsAreSpinning[(int)playerId] = true;
		yield return this.stop_slot_machine_cr(playerId);
		this.slotsAreSpinning[(int)playerId] = false;
		this.slotsConfirm[(int)playerId] = true;
		bool playAgain = false;
		while (!PlayerManager.GetPlayerInput(playerId).GetButtonDown(13))
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount > 0)
			{
				this.slotsCanSpinAgain[(int)playerId] = true;
				if (PlayerManager.GetPlayer(playerId).input.actions.GetButtonDown(7))
				{
					playAgain = true;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount--;
					this.ChangePlayersWeapon(playerId);
					IL_1B9:
					this.slotsConfirm[(int)playerId] = false;
					this.slotsCanSpinAgain[(int)playerId] = false;
					yield return null;
					if (playAgain)
					{
						yield return this.play_slot_machine_cr(playerId);
					}
					else
					{
						this.slotsDone[(int)playerId] = true;
						bool chaliceCharmEquipped = TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BaseCharm == Charm.charm_chalice;
						PlayerData.PlayerLoadouts.PlayerLoadout P1loadout = PlayerData.Data.Loadouts.GetPlayerLoadout(playerId);
						if (this.bonusHP[(int)playerId] > 0)
						{
							P1loadout.charm = ((!chaliceCharmEquipped) ? Charm.None : Charm.charm_chalice);
						}
						if (this.bonusToken[(int)playerId] > 0)
						{
							P1loadout.charm = ((!chaliceCharmEquipped) ? Charm.None : Charm.charm_chalice);
						}
						PlayerStatsManager playerStats = PlayerManager.GetPlayer(playerId).stats;
						int hp = playerStats.Health + this.bonusHP[(int)playerId];
						if (hp > 8)
						{
							hp = 8;
						}
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].HP = hp;
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].BonusHP = hp - 3;
						playerStats.SetHealth(hp);
						this.bonusHP[(int)playerId] = 0;
						TowerOfPowerLevelGameInfo.PLAYER_STATS[(int)playerId].tokenCount += this.bonusToken[(int)playerId];
						this.bonusToken[(int)playerId] = 0;
						if (this.slotsDone[0] && this.slotsDone[1])
						{
							yield return this.go_to_next_level_cr();
						}
					}
					yield break;
				}
			}
			yield return null;
		}
		goto IL_1B9;
	}

	// Token: 0x06002F8A RID: 12170 RVA: 0x001C2CB6 File Offset: 0x001C10B6
	public bool SlotsDone()
	{
		if (PlayerManager.Multiplayer)
		{
			return this.slotsDone[0] && this.slotsDone[1];
		}
		return this.slotsDone[0];
	}

	// Token: 0x06002F8B RID: 12171 RVA: 0x001C2CE4 File Offset: 0x001C10E4
	private IEnumerator go_to_next_level_cr()
	{
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		for (;;)
		{
			if (PauseManager.state == PauseManager.State.Paused)
			{
				this.waitForButtonRelease = 3;
				yield return null;
			}
			if (this.anyInput.GetButtonUp(CupheadButton.Accept))
			{
				this.waitForButtonRelease = 0;
			}
			if (this.waitForButtonRelease == 0 && this.anyInput.GetButtonDown(CupheadButton.Accept))
			{
				break;
			}
			if (this.waitForButtonRelease > 0)
			{
				this.waitForButtonRelease--;
			}
			yield return null;
		}
		int currentLevel = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		if (TowerOfPowerLevelGameInfo.CURRENT_TURN < TowerOfPowerLevelGameInfo.allStageSpaces.Count)
		{
			if (TowerOfPowerLevelGameInfo.PLAYER_STATS[0] != null)
			{
				this.SetDifficulty();
				yield return base.StartCoroutine(this.startMiniBoss_cr(TowerOfPowerLevelGameInfo.allStageSpaces[currentLevel]));
			}
		}
		else
		{
			SceneLoader.LoadLastMap();
		}
		yield break;
	}

	// Token: 0x0400383C RID: 14396
	private const string PREWEAPON_NAME = "level_weapon_";

	// Token: 0x0400383D RID: 14397
	private const string PRESUPER_NAME = "level_super_";

	// Token: 0x0400383E RID: 14398
	private const string PRESUPER_CHALICE_NAME = "level_super_chalice_";

	// Token: 0x0400383F RID: 14399
	private const string PRECHARM_NAME = "charm_";

	// Token: 0x04003840 RID: 14400
	private const string PRECHARM_CHALICE_NAME = "charm_chalice_";

	// Token: 0x04003841 RID: 14401
	[SerializeField]
	private float advanceDelay = 10f;

	// Token: 0x04003842 RID: 14402
	private CupheadInput.AnyPlayerInput anyInput;

	// Token: 0x04003843 RID: 14403
	private int[] bonusHP = new int[2];

	// Token: 0x04003844 RID: 14404
	private int[] bonusToken = new int[2];

	// Token: 0x04003845 RID: 14405
	private List<Levels>[] BossPools;

	// Token: 0x04003846 RID: 14406
	private List<Levels>[] ShmupPools;

	// Token: 0x04003847 RID: 14407
	private List<Levels>[] KingDicePools;

	// Token: 0x04003848 RID: 14408
	private List<int> ShmupPlacement = new List<int>();

	// Token: 0x04003849 RID: 14409
	private int[] SlotMachineWeapon2Attempt = new int[2];

	// Token: 0x0400384A RID: 14410
	public bool[] slotsAreSpinning = new bool[2];

	// Token: 0x0400384B RID: 14411
	public bool[] slotsCanSpinAgain = new bool[2];

	// Token: 0x0400384C RID: 14412
	public bool[] slotsConfirm = new bool[2];

	// Token: 0x0400384D RID: 14413
	private bool[] slotsDone = new bool[]
	{
		true,
		true
	};

	// Token: 0x0400384E RID: 14414
	private int waitForButtonRelease;

	// Token: 0x0400384F RID: 14415
	[SerializeField]
	private TowerOfPowerScorecard scorecard;

	// Token: 0x04003850 RID: 14416
	public bool showingScorecard;

	// Token: 0x04003851 RID: 14417
	[SerializeField]
	private bool debugForceSlotMachineEveryTurn;

	// Token: 0x04003852 RID: 14418
	[SerializeField]
	private bool debugForceSlotMachineAfterOneFight;

	// Token: 0x04003853 RID: 14419
	[SerializeField]
	private bool debugSkipToLastFight;

	// Token: 0x02000804 RID: 2052
	public enum Charm_Slot
	{
		// Token: 0x04003858 RID: 14424
		charm_health_up_1,
		// Token: 0x04003859 RID: 14425
		charm_health_up_2,
		// Token: 0x0400385A RID: 14426
		charm_super_builder,
		// Token: 0x0400385B RID: 14427
		charm_smoke_dash,
		// Token: 0x0400385C RID: 14428
		charm_parry_plus,
		// Token: 0x0400385D RID: 14429
		charm_pit_saver,
		// Token: 0x0400385E RID: 14430
		charm_parry_attack,
		// Token: 0x0400385F RID: 14431
		charm_chalice,
		// Token: 0x04003860 RID: 14432
		charm_directional_dash,
		// Token: 0x04003861 RID: 14433
		None,
		// Token: 0x04003862 RID: 14434
		charm_extra_token
	}

	// Token: 0x02000805 RID: 2053
	public enum Weapon_Slot
	{
		// Token: 0x04003864 RID: 14436
		level_weapon_peashot,
		// Token: 0x04003865 RID: 14437
		level_weapon_spreadshot,
		// Token: 0x04003866 RID: 14438
		level_weapon_arc,
		// Token: 0x04003867 RID: 14439
		level_weapon_homing,
		// Token: 0x04003868 RID: 14440
		level_weapon_exploder,
		// Token: 0x04003869 RID: 14441
		level_weapon_boomerang,
		// Token: 0x0400386A RID: 14442
		level_weapon_charge,
		// Token: 0x0400386B RID: 14443
		level_weapon_bouncer,
		// Token: 0x0400386C RID: 14444
		level_weapon_wide_shot,
		// Token: 0x0400386D RID: 14445
		plane_weapon_peashot,
		// Token: 0x0400386E RID: 14446
		plane_weapon_laser,
		// Token: 0x0400386F RID: 14447
		plane_weapon_bomb,
		// Token: 0x04003870 RID: 14448
		plane_chalice_weapon_3way,
		// Token: 0x04003871 RID: 14449
		arcade_weapon_peashot,
		// Token: 0x04003872 RID: 14450
		arcade_weapon_rocket_peashot,
		// Token: 0x04003873 RID: 14451
		None
	}

	// Token: 0x02000806 RID: 2054
	public enum Super_Slot
	{
		// Token: 0x04003875 RID: 14453
		level_super_beam,
		// Token: 0x04003876 RID: 14454
		level_super_ghost,
		// Token: 0x04003877 RID: 14455
		level_super_invincible,
		// Token: 0x04003878 RID: 14456
		level_super_chalice_shmup,
		// Token: 0x04003879 RID: 14457
		level_super_chalice_vert_beam,
		// Token: 0x0400387A RID: 14458
		level_super_chalice_shield,
		// Token: 0x0400387B RID: 14459
		plane_super_bomb,
		// Token: 0x0400387C RID: 14460
		plane_super_chalice_stream,
		// Token: 0x0400387D RID: 14461
		None
	}
}
