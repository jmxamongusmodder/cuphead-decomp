using System;
using UnityEngine;

// Token: 0x02000953 RID: 2387
public class MapNPCJuggler : MonoBehaviour
{
	// Token: 0x060037C0 RID: 14272 RVA: 0x001FFECC File Offset: 0x001FE2CC
	private void Start()
	{
		this.AddDialoguerEvents();
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) == 1f)
		{
			this.animator.SetTrigger("three");
			return;
		}
		int numParriesInRow = PlayerData.Data.GetNumParriesInRow(PlayerId.Any);
		if (numParriesInRow <= 1)
		{
			this.animator.SetTrigger("one");
		}
		else if (numParriesInRow == 2)
		{
			this.animator.SetTrigger("two");
		}
		else if (numParriesInRow == 3)
		{
			this.animator.SetTrigger("three");
		}
		else if (numParriesInRow > 3)
		{
			this.animator.SetTrigger("three");
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
		}
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x001FFF95 File Offset: 0x001FE395
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x001FFF9D File Offset: 0x001FE39D
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x001FFFB5 File Offset: 0x001FE3B5
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x001FFFD0 File Offset: 0x001FE3D0
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "JugglerCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
		}
	}

	// Token: 0x04003FC0 RID: 16320
	[SerializeField]
	private Animator animator;

	// Token: 0x04003FC1 RID: 16321
	[SerializeField]
	private int dialoguerVariableID;

	// Token: 0x04003FC2 RID: 16322
	[SerializeField]
	private string coinID = Guid.NewGuid().ToString();

	// Token: 0x04003FC3 RID: 16323
	[HideInInspector]
	public bool SkipDialogueEvent;
}
