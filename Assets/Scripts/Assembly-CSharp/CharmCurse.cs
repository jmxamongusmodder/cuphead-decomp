using System;
using System.Collections.Generic;

// Token: 0x02000A43 RID: 2627
public static class CharmCurse
{
	// Token: 0x06003EA2 RID: 16034 RVA: 0x00225B98 File Offset: 0x00223F98
	public static int CalculateLevel(PlayerId playerId)
	{
		if (!PlayerData.Data.GetLevelData(Levels.Graveyard).completed)
		{
			return -1;
		}
		int num = PlayerData.Data.CalculateCurseCharmAccumulatedValue(playerId, CharmCurse.CountableLevels);
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

	// Token: 0x06003EA3 RID: 16035 RVA: 0x00225C00 File Offset: 0x00224000
	public static bool IsMaxLevel(PlayerId playerId)
	{
		int[] levelThreshold = WeaponProperties.CharmCurse.levelThreshold;
		return CharmCurse.CalculateLevel(playerId) == levelThreshold.Length - 1;
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x00225C20 File Offset: 0x00224020
	public static int GetHealthModifier(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.healthModifierValues[charmLevel];
	}

	// Token: 0x06003EA5 RID: 16037 RVA: 0x00225C32 File Offset: 0x00224032
	public static float GetSuperMeterAmount(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0f;
		}
		return WeaponProperties.CharmCurse.superMeterAmount[charmLevel];
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x00225C48 File Offset: 0x00224048
	public static int GetSmokeDashInterval(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.smokeDashInterval[charmLevel];
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x00225C5A File Offset: 0x0022405A
	public static int GetWhetstoneInterval(int charmLevel)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		return WeaponProperties.CharmCurse.whetstoneInterval[charmLevel];
	}

	// Token: 0x06003EA8 RID: 16040 RVA: 0x00225C6C File Offset: 0x0022406C
	public static int GetHealerInterval(int charmLevel, int hpReceived)
	{
		if (charmLevel < 0)
		{
			return 0;
		}
		if (CharmCurse.healerCharmIntervals == null)
		{
			string[] healerInterval = WeaponProperties.CharmCurse.healerInterval;
			CharmCurse.healerCharmIntervals = new List<int[]>(healerInterval.Length);
			foreach (string text in healerInterval)
			{
				string[] array2 = text.Split(new char[]
				{
					','
				});
				if (array2.Length != 3)
				{
					throw new Exception("Invalid healer intervals");
				}
				int[] array3 = new int[array2.Length];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = Parser.IntParse(array2[j]);
				}
				CharmCurse.healerCharmIntervals.Add(array3);
			}
		}
		return CharmCurse.healerCharmIntervals[charmLevel][hpReceived];
	}

	// Token: 0x040045B1 RID: 17841
	public static readonly Levels[] CountableLevels = new Levels[]
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

	// Token: 0x040045B2 RID: 17842
	private static List<int[]> healerCharmIntervals;
}
