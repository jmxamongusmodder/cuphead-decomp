using System;
using UnityEngine;

// Token: 0x02000957 RID: 2391
public class MapNPCNewsieCat : MonoBehaviour
{
	// Token: 0x060037D5 RID: 14293 RVA: 0x00200528 File Offset: 0x001FE928
	private void Start()
	{
		Dialoguer.SetGlobalFloat(40, 0f);
		if (!PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(39, 0f);
		}
		Levels[] levels = new Levels[]
		{
			Levels.Veggies,
			Levels.Slime,
			Levels.FlyingBlimp,
			Levels.Flower,
			Levels.Frogs
		};
		Levels[] array = new Levels[]
		{
			Levels.OldMan,
			Levels.RumRunners,
			Levels.Airplane,
			Levels.SnowCult,
			Levels.FlyingCowboy,
			Levels.Saltbaker
		};
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (PlayerData.Data.CheckLevelCompleted(array[i]))
			{
				num++;
			}
		}
		if (num > 0)
		{
			Dialoguer.SetGlobalFloat(24, 3f);
		}
		else if (PlayerData.Data.CheckLevelCompleted(Levels.Devil))
		{
			Dialoguer.SetGlobalFloat(24, 2f);
		}
		else if (PlayerData.Data.CheckLevelsCompleted(levels))
		{
			Dialoguer.SetGlobalFloat(24, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(24, 0f);
		}
		this.AddDialoguerEvents();
	}

	// Token: 0x060037D6 RID: 14294 RVA: 0x00200627 File Offset: 0x001FEA27
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037D7 RID: 14295 RVA: 0x0020062F File Offset: 0x001FEA2F
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037D8 RID: 14296 RVA: 0x00200647 File Offset: 0x001FEA47
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037D9 RID: 14297 RVA: 0x00200660 File Offset: 0x001FEA60
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "NewsieCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(39, 1f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID1, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID2, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID3, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins);
		}
	}

	// Token: 0x04003FD0 RID: 16336
	private const int DIALOGUER_VAR_INDEX = 24;

	// Token: 0x04003FD1 RID: 16337
	private const int DIALOGUER_VAR_GOT_COIN = 39;

	// Token: 0x04003FD2 RID: 16338
	private const int DIALOGUER_VAR_INTERACT_COUNTER = 40;

	// Token: 0x04003FD3 RID: 16339
	[SerializeField]
	private string coinID1 = Guid.NewGuid().ToString();

	// Token: 0x04003FD4 RID: 16340
	[SerializeField]
	private string coinID2 = Guid.NewGuid().ToString();

	// Token: 0x04003FD5 RID: 16341
	[SerializeField]
	private string coinID3 = Guid.NewGuid().ToString();
}
