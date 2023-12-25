using System;
using UnityEngine;

// Token: 0x02000945 RID: 2373
public class MapNPCAppletraveller : MonoBehaviour
{
	// Token: 0x06003772 RID: 14194 RVA: 0x001FDF87 File Offset: 0x001FC387
	private void Start()
	{
		this.squareRadiusStartWaving = this.radiusStartWaving * this.radiusStartWaving;
		this.squareRadiusStopWaving = this.radiusStopWaving * this.radiusStopWaving;
		this.AddDialoguerEvents();
	}

	// Token: 0x06003773 RID: 14195 RVA: 0x001FDFB5 File Offset: 0x001FC3B5
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003774 RID: 14196 RVA: 0x001FDFC0 File Offset: 0x001FC3C0
	private void Update()
	{
		if ((this.state == MapNPCAppletraveller.AppletravellerState.idle && (base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude < this.squareRadiusStartWaving) || (PlayerManager.Multiplayer && (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude < this.squareRadiusStartWaving))
		{
			this.state = MapNPCAppletraveller.AppletravellerState.wave;
			this.animator.SetTrigger("wave");
		}
		else if (this.state == MapNPCAppletraveller.AppletravellerState.wave)
		{
			if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude > this.squareRadiusStartWaving && (!PlayerManager.Multiplayer || (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude > this.squareRadiusStartWaving))
			{
				this.state = MapNPCAppletraveller.AppletravellerState.idle;
				this.animator.SetTrigger("next");
			}
			if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude < this.squareRadiusStopWaving || (PlayerManager.Multiplayer && (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude < this.squareRadiusStopWaving))
			{
				this.state = MapNPCAppletraveller.AppletravellerState.wait;
				this.animator.SetTrigger("next");
			}
		}
		else if ((base.transform.position - Map.Current.players[0].transform.position).sqrMagnitude > this.squareRadiusStartWaving && (!PlayerManager.Multiplayer || (base.transform.position - Map.Current.players[1].transform.position).sqrMagnitude > this.squareRadiusStartWaving))
		{
			this.state = MapNPCAppletraveller.AppletravellerState.idle;
		}
	}

	// Token: 0x06003775 RID: 14197 RVA: 0x001FE230 File Offset: 0x001FC630
	protected void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(base.transform.position, this.radiusStartWaving);
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(base.transform.position, this.radiusStopWaving);
	}

	// Token: 0x06003776 RID: 14198 RVA: 0x001FE27D File Offset: 0x001FC67D
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003777 RID: 14199 RVA: 0x001FE295 File Offset: 0x001FC695
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x06003778 RID: 14200 RVA: 0x001FE2B0 File Offset: 0x001FC6B0
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "MacCoin" && !PlayerData.Data.coinManager.GetCoinCollected(this.coinID1))
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID1, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID2, true, PlayerId.Any);
			PlayerData.Data.coinManager.SetCoinValue(this.coinID3, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 3);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 3);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.ThreeCoins);
		}
	}

	// Token: 0x04003F80 RID: 16256
	[SerializeField]
	private Animator animator;

	// Token: 0x04003F81 RID: 16257
	[SerializeField]
	private int dialoguerVariableID = 6;

	// Token: 0x04003F82 RID: 16258
	[SerializeField]
	private string coinID1 = Guid.NewGuid().ToString();

	// Token: 0x04003F83 RID: 16259
	[SerializeField]
	private string coinID2 = Guid.NewGuid().ToString();

	// Token: 0x04003F84 RID: 16260
	[SerializeField]
	private string coinID3 = Guid.NewGuid().ToString();

	// Token: 0x04003F85 RID: 16261
	[SerializeField]
	private float radiusStartWaving = 50f;

	// Token: 0x04003F86 RID: 16262
	[SerializeField]
	private float radiusStopWaving = 10f;

	// Token: 0x04003F87 RID: 16263
	private float squareRadiusStartWaving;

	// Token: 0x04003F88 RID: 16264
	private float squareRadiusStopWaving;

	// Token: 0x04003F89 RID: 16265
	private MapNPCAppletraveller.AppletravellerState state;

	// Token: 0x04003F8A RID: 16266
	[HideInInspector]
	public bool SkipDialogueEvent;

	// Token: 0x02000946 RID: 2374
	private enum AppletravellerState
	{
		// Token: 0x04003F8C RID: 16268
		idle,
		// Token: 0x04003F8D RID: 16269
		wave,
		// Token: 0x04003F8E RID: 16270
		wait
	}
}
