using System;
using UnityEngine;

// Token: 0x02000934 RID: 2356
public class MapCoin : MonoBehaviour
{
	// Token: 0x06003719 RID: 14105 RVA: 0x001FBCBC File Offset: 0x001FA0BC
	private void Start()
	{
		if (PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600371A RID: 14106 RVA: 0x001FBCE4 File Offset: 0x001FA0E4
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!PlayerData.Data.coinManager.GetCoinCollected(this.coinID))
		{
			PlayerData.Data.coinManager.SetCoinValue(this.coinID, true, PlayerId.Any);
			PlayerData.Data.AddCurrency(PlayerId.PlayerOne, 1);
			PlayerData.Data.AddCurrency(PlayerId.PlayerTwo, 1);
			PlayerData.SaveCurrentFile();
			MapEventNotification.Current.ShowEvent(MapEventNotification.Type.Coin);
			if (this.mapNPCCoinMoneyman != null)
			{
				this.mapNPCCoinMoneyman.UpdateCoins();
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04003F4C RID: 16204
	[SerializeField]
	private MapNPCCoinMoneyman mapNPCCoinMoneyman;

	// Token: 0x04003F4D RID: 16205
	public string coinID = Guid.NewGuid().ToString();
}
