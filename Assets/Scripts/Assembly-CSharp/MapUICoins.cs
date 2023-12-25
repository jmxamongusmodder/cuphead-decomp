using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009A2 RID: 2466
public class MapUICoins : MonoBehaviour
{
	// Token: 0x060039DF RID: 14815 RVA: 0x0020EDEE File Offset: 0x0020D1EE
	private void Start()
	{
		this.singleDigitCoinPosition = this.coinImage.transform.localPosition;
	}

	// Token: 0x060039E0 RID: 14816 RVA: 0x0020EE08 File Offset: 0x0020D208
	private void Update()
	{
		if (this.playerId == PlayerId.PlayerTwo)
		{
			if (!PlayerManager.Multiplayer)
			{
				this.coinImage.enabled = false;
				this.currencyNbImage.enabled = false;
				return;
			}
			this.coinImage.enabled = true;
			this.currencyNbImage.enabled = true;
		}
		int currency = PlayerData.Data.GetCurrency(this.playerId);
		if (currency != this.previousCurrency)
		{
			this.previousCurrency = currency;
			this.currencyNbImage.sprite = this.coinSprites[currency];
			if (currency > 9)
			{
				this.coinImage.transform.localPosition = this.doubleDigitCoinTransform.localPosition;
			}
			else
			{
				this.coinImage.transform.localPosition = this.singleDigitCoinPosition;
			}
		}
	}

	// Token: 0x040041CF RID: 16847
	[SerializeField]
	private PlayerId playerId;

	// Token: 0x040041D0 RID: 16848
	[SerializeField]
	private Image coinImage;

	// Token: 0x040041D1 RID: 16849
	[SerializeField]
	private Image currencyNbImage;

	// Token: 0x040041D2 RID: 16850
	[SerializeField]
	private Sprite[] coinSprites;

	// Token: 0x040041D3 RID: 16851
	[SerializeField]
	private Transform doubleDigitCoinTransform;

	// Token: 0x040041D4 RID: 16852
	private Vector3 singleDigitCoinPosition;

	// Token: 0x040041D5 RID: 16853
	private int previousCurrency = -1;
}
