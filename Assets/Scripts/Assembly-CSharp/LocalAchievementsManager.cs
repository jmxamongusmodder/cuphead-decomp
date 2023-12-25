using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200039A RID: 922
public static class LocalAchievementsManager
{
	// Token: 0x1400000C RID: 12
	// (add) Token: 0x06000B39 RID: 2873 RVA: 0x000825C8 File Offset: 0x000809C8
	// (remove) Token: 0x06000B3A RID: 2874 RVA: 0x000825FC File Offset: 0x000809FC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action<LocalAchievementsManager.Achievement> AchievementUnlockedEvent;

	// Token: 0x06000B3B RID: 2875 RVA: 0x00082630 File Offset: 0x00080A30
	public static void Initialize()
	{
		if (LocalAchievementsManager.initialized)
		{
			return;
		}
		LocalAchievementsManager.initialized = true;
		LocalAchievementsManager.loadFromCloud();
	}

	// Token: 0x06000B3C RID: 2876 RVA: 0x00082648 File Offset: 0x00080A48
	public static void UnlockAchievement(PlayerId playerId, string achievementName)
	{
		LocalAchievementsManager.Achievement achievement = (LocalAchievementsManager.Achievement)Enum.Parse(typeof(LocalAchievementsManager.Achievement), achievementName);
		if (LocalAchievementsManager.IsAchievementUnlocked(achievement))
		{
			return;
		}
		LocalAchievementsManager.achievementData.unlockedAchievements.Add(achievement);
		LocalAchievementsManager.saveToCloud();
		if (LocalAchievementsManager.AchievementUnlockedEvent != null)
		{
			LocalAchievementsManager.AchievementUnlockedEvent(achievement);
		}
	}

	// Token: 0x06000B3D RID: 2877 RVA: 0x000826A4 File Offset: 0x00080AA4
	public static void IncrementStat(PlayerId player, string id, int value)
	{
		if (id == "Parries")
		{
			if (LocalAchievementsManager.achievementData.parryCount >= 100)
			{
				return;
			}
			LocalAchievementsManager.achievementData.parryCount += value;
			bool flag = true;
			if (LocalAchievementsManager.achievementData.parryCount >= 20)
			{
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryApprentice");
				flag = false;
			}
			if (LocalAchievementsManager.achievementData.parryCount >= 100)
			{
				LocalAchievementsManager.UnlockAchievement(PlayerId.Any, "ParryMaster");
				flag = false;
			}
			if (flag)
			{
				LocalAchievementsManager.saveToCloud();
			}
		}
	}

	// Token: 0x06000B3E RID: 2878 RVA: 0x00082736 File Offset: 0x00080B36
	public static IList<LocalAchievementsManager.Achievement> GetUnlockedAchievements()
	{
		return LocalAchievementsManager.achievementData.unlockedAchievements;
	}

	// Token: 0x06000B3F RID: 2879 RVA: 0x00082742 File Offset: 0x00080B42
	public static bool IsAchievementUnlocked(LocalAchievementsManager.Achievement achievement)
	{
		return LocalAchievementsManager.achievementData.unlockedAchievements.Contains(achievement);
	}

	// Token: 0x06000B40 RID: 2880 RVA: 0x00082754 File Offset: 0x00080B54
	public static bool IsHiddenAchievement(LocalAchievementsManager.Achievement achievement)
	{
		return achievement == LocalAchievementsManager.Achievement.FoundSecretPassage || achievement == LocalAchievementsManager.Achievement.SmallPlaneOnlyWin || achievement == LocalAchievementsManager.Achievement.FoundAllMoney || achievement == LocalAchievementsManager.Achievement.PacifistRun || achievement == LocalAchievementsManager.Achievement.NoHitsTakenDicePalace || achievement == LocalAchievementsManager.Achievement.BadEnding || achievement == LocalAchievementsManager.Achievement.CompleteDevil || achievement == LocalAchievementsManager.Achievement.DefeatDevilPhase2 || achievement == LocalAchievementsManager.Achievement.Paladin;
	}

	// Token: 0x06000B41 RID: 2881 RVA: 0x000827AC File Offset: 0x00080BAC
	private static void saveToCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			string value = JsonUtility.ToJson(LocalAchievementsManager.achievementData);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary[LocalAchievementsManager.CloudKey] = value;
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			IDictionary<string, string> data = dictionary;
			if (LocalAchievementsManager.<>f__mg$cache0 == null)
			{
				LocalAchievementsManager.<>f__mg$cache0 = new SaveCloudDataHandler(LocalAchievementsManager.onSavedCloudData);
			}
			@interface.SaveCloudData(data, LocalAchievementsManager.<>f__mg$cache0);
		}
	}

	// Token: 0x06000B42 RID: 2882 RVA: 0x00082817 File Offset: 0x00080C17
	private static void onSavedCloudData(bool success)
	{
	}

	// Token: 0x06000B43 RID: 2883 RVA: 0x0008281C File Offset: 0x00080C1C
	private static void loadFromCloud()
	{
		if (OnlineManager.Instance.Interface.CloudStorageInitialized)
		{
			OnlineInterface @interface = OnlineManager.Instance.Interface;
			string[] keys = new string[]
			{
				LocalAchievementsManager.CloudKey
			};
			if (LocalAchievementsManager.<>f__mg$cache1 == null)
			{
				LocalAchievementsManager.<>f__mg$cache1 = new LoadCloudDataHandler(LocalAchievementsManager.onLoadedCloudData);
			}
			@interface.LoadCloudData(keys, LocalAchievementsManager.<>f__mg$cache1);
		}
	}

	// Token: 0x06000B44 RID: 2884 RVA: 0x00082878 File Offset: 0x00080C78
	private static void onLoadedCloudData(string[] data, CloudLoadResult result)
	{
		if (result == CloudLoadResult.Failed)
		{
			LocalAchievementsManager.loadFromCloud();
			return;
		}
		try
		{
			if (result == CloudLoadResult.NoData)
			{
				LocalAchievementsManager.achievementData = new LocalAchievementsManager.AchievementData();
				LocalAchievementsManager.saveToCloud();
			}
			else
			{
				LocalAchievementsManager.achievementData = JsonUtility.FromJson<LocalAchievementsManager.AchievementData>(data[0]);
			}
		}
		catch (ArgumentException)
		{
			LocalAchievementsManager.achievementData = new LocalAchievementsManager.AchievementData();
		}
	}

	// Token: 0x040014BF RID: 5311
	private static readonly string CloudKey = "cuphead_ach";

	// Token: 0x040014C0 RID: 5312
	public static readonly LocalAchievementsManager.Achievement[] DLCAchievements = new LocalAchievementsManager.Achievement[]
	{
		LocalAchievementsManager.Achievement.CompleteWorldDLC,
		LocalAchievementsManager.Achievement.ARankWorldDLC,
		LocalAchievementsManager.Achievement.DefeatBossAsChalice,
		LocalAchievementsManager.Achievement.DefeatXBossesAsChalice,
		LocalAchievementsManager.Achievement.ChaliceSuperWin,
		LocalAchievementsManager.Achievement.DefeatBossDLCWeapon,
		LocalAchievementsManager.Achievement.DefeatAllKOG,
		LocalAchievementsManager.Achievement.DefeatKOGGauntlet,
		LocalAchievementsManager.Achievement.DefeatSaltbaker,
		LocalAchievementsManager.Achievement.SRankAnyDLC,
		LocalAchievementsManager.Achievement.DefeatBossNoMinions,
		LocalAchievementsManager.Achievement.HP9,
		LocalAchievementsManager.Achievement.DefeatDevilPhase2,
		LocalAchievementsManager.Achievement.Paladin
	};

	// Token: 0x040014C2 RID: 5314
	private static bool initialized;

	// Token: 0x040014C3 RID: 5315
	private static LocalAchievementsManager.AchievementData achievementData;

	// Token: 0x040014C4 RID: 5316
	[CompilerGenerated]
	private static SaveCloudDataHandler <>f__mg$cache0;

	// Token: 0x040014C5 RID: 5317
	[CompilerGenerated]
	private static LoadCloudDataHandler <>f__mg$cache1;

	// Token: 0x0200039B RID: 923
	public enum Achievement
	{
		// Token: 0x040014C7 RID: 5319
		DefeatBoss,
		// Token: 0x040014C8 RID: 5320
		ParryApprentice,
		// Token: 0x040014C9 RID: 5321
		ParryMaster,
		// Token: 0x040014CA RID: 5322
		ExWin,
		// Token: 0x040014CB RID: 5323
		SuperWin,
		// Token: 0x040014CC RID: 5324
		ParryChain,
		// Token: 0x040014CD RID: 5325
		NoHitsTaken,
		// Token: 0x040014CE RID: 5326
		ARankWorld1,
		// Token: 0x040014CF RID: 5327
		ARankWorld2,
		// Token: 0x040014D0 RID: 5328
		ARankWorld3,
		// Token: 0x040014D1 RID: 5329
		CompleteWorld1,
		// Token: 0x040014D2 RID: 5330
		CompleteWorld2,
		// Token: 0x040014D3 RID: 5331
		CompleteWorld3,
		// Token: 0x040014D4 RID: 5332
		UnlockedAllSupers,
		// Token: 0x040014D5 RID: 5333
		FoundAllLevelMoney,
		// Token: 0x040014D6 RID: 5334
		BoughtAllItems,
		// Token: 0x040014D7 RID: 5335
		CompleteDicePalace,
		// Token: 0x040014D8 RID: 5336
		ARankWorld4,
		// Token: 0x040014D9 RID: 5337
		GoodEnding,
		// Token: 0x040014DA RID: 5338
		SRank,
		// Token: 0x040014DB RID: 5339
		NewGamePlus,
		// Token: 0x040014DC RID: 5340
		FoundSecretPassage,
		// Token: 0x040014DD RID: 5341
		SmallPlaneOnlyWin,
		// Token: 0x040014DE RID: 5342
		FoundAllMoney,
		// Token: 0x040014DF RID: 5343
		PacifistRun,
		// Token: 0x040014E0 RID: 5344
		NoHitsTakenDicePalace,
		// Token: 0x040014E1 RID: 5345
		BadEnding,
		// Token: 0x040014E2 RID: 5346
		CompleteDevil,
		// Token: 0x040014E3 RID: 5347
		CompleteWorldDLC,
		// Token: 0x040014E4 RID: 5348
		ARankWorldDLC,
		// Token: 0x040014E5 RID: 5349
		DefeatBossAsChalice,
		// Token: 0x040014E6 RID: 5350
		DefeatXBossesAsChalice,
		// Token: 0x040014E7 RID: 5351
		ChaliceSuperWin,
		// Token: 0x040014E8 RID: 5352
		DefeatBossDLCWeapon,
		// Token: 0x040014E9 RID: 5353
		DefeatAllKOG,
		// Token: 0x040014EA RID: 5354
		DefeatKOGGauntlet,
		// Token: 0x040014EB RID: 5355
		DefeatSaltbaker,
		// Token: 0x040014EC RID: 5356
		SRankAnyDLC,
		// Token: 0x040014ED RID: 5357
		DefeatBossNoMinions,
		// Token: 0x040014EE RID: 5358
		HP9,
		// Token: 0x040014EF RID: 5359
		DefeatDevilPhase2,
		// Token: 0x040014F0 RID: 5360
		Paladin
	}

	// Token: 0x0200039C RID: 924
	[Serializable]
	private class AchievementData
	{
		// Token: 0x040014F1 RID: 5361
		public List<LocalAchievementsManager.Achievement> unlockedAchievements = new List<LocalAchievementsManager.Achievement>();

		// Token: 0x040014F2 RID: 5362
		public int parryCount;
	}
}
