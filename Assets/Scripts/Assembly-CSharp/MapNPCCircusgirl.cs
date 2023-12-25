using System;
using UnityEngine;

// Token: 0x0200094D RID: 2381
public class MapNPCCircusgirl : MonoBehaviour
{
	// Token: 0x060037A8 RID: 14248 RVA: 0x001FF7E4 File Offset: 0x001FDBE4
	private void Start()
	{
		if (PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
		}
		else
		{
			this.AddDialoguerEvents();
			OnlineManager.Instance.Interface.GetAchievement(PlayerId.PlayerOne, "FoundSecretPassage", delegate(OnlineAchievement achievement)
			{
				if (achievement.IsUnlocked)
				{
					Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				}
			});
		}
	}

	// Token: 0x060037A9 RID: 14249 RVA: 0x001FF847 File Offset: 0x001FDC47
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037AA RID: 14250 RVA: 0x001FF84F File Offset: 0x001FDC4F
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037AB RID: 14251 RVA: 0x001FF867 File Offset: 0x001FDC67
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037AC RID: 14252 RVA: 0x001FF880 File Offset: 0x001FDC80
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "GingerbreadCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
		}
	}

	// Token: 0x04003FAD RID: 16301
	[SerializeField]
	private int dialoguerVariableID = 7;

	// Token: 0x04003FAE RID: 16302
	[SerializeField]
	private string coinID = Guid.NewGuid().ToString();

	// Token: 0x04003FAF RID: 16303
	[HideInInspector]
	public bool SkipDialogueEvent;
}
