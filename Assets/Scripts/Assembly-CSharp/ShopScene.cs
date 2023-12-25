using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B03 RID: 2819
public class ShopScene : AbstractMonoBehaviour
{
	// Token: 0x1700061D RID: 1565
	// (get) Token: 0x0600445D RID: 17501 RVA: 0x00242FF9 File Offset: 0x002413F9
	// (set) Token: 0x0600445E RID: 17502 RVA: 0x00243000 File Offset: 0x00241400
	public static ShopScene Current { get; private set; }

	// Token: 0x0600445F RID: 17503 RVA: 0x00243008 File Offset: 0x00241408
	protected override void Awake()
	{
		base.Awake();
		Cuphead.Init(false);
		ShopScene.Current = this;
		this.playerOne.OnPurchaseEvent += this.OnPurchase;
		this.playerTwo.OnPurchaseEvent += this.OnPurchase;
		this.playerOne.OnExitEvent += this.OnExit;
		this.playerTwo.OnExitEvent += this.OnExit;
		SceneLoader.OnFadeOutEndEvent += this.OnLoaded;
	}

	// Token: 0x06004460 RID: 17504 RVA: 0x00243094 File Offset: 0x00241494
	private void OnDestroy()
	{
		if (ShopScene.Current == this)
		{
			ShopScene.Current = null;
		}
		SceneLoader.OnFadeOutEndEvent -= this.OnLoaded;
	}

	// Token: 0x06004461 RID: 17505 RVA: 0x002430BD File Offset: 0x002414BD
	private void OnLoaded()
	{
		this.pig.OnStart();
		this.playerOne.OnStart();
		this.playerTwo.OnStart();
		InterruptingPrompt.SetCanInterrupt(true);
	}

	// Token: 0x06004462 RID: 17506 RVA: 0x002430E6 File Offset: 0x002414E6
	private void OnPurchase()
	{
		this.pig.OnPurchase();
	}

	// Token: 0x06004463 RID: 17507 RVA: 0x002430F4 File Offset: 0x002414F4
	private void OnExit()
	{
		if ((!this.playerOne.gameObject.activeInHierarchy || this.playerOne.state == ShopScenePlayer.State.Exiting || this.playerOne.state == ShopScenePlayer.State.Exited) && (!this.playerTwo.gameObject.activeInHierarchy || this.playerTwo.state == ShopScenePlayer.State.Exiting || this.playerTwo.state == ShopScenePlayer.State.Exited))
		{
			base.StartCoroutine(this.exit_cr());
			this.playerOne.OnExit();
			this.playerTwo.OnExit();
		}
	}

	// Token: 0x06004464 RID: 17508 RVA: 0x00243194 File Offset: 0x00241594
	private IEnumerator exit_cr()
	{
		if (this.HasBoughtEverythingForAchievement(PlayerId.PlayerOne))
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerOne, "BoughtAllItems");
		}
		if (this.HasBoughtEverythingForAchievement(PlayerId.PlayerTwo))
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.PlayerTwo, "BoughtAllItems");
		}
		this.pig.OnExit();
		yield return this.pig.animator.WaitForAnimationToEnd(this, "Bye", false, true);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x06004465 RID: 17509 RVA: 0x002431AF File Offset: 0x002415AF
	public ShopSceneItem[] GetCharmItems(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			return this.playerTwo.GetCharmItemPrefabs();
		}
		return this.playerOne.GetCharmItemPrefabs();
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x002431CF File Offset: 0x002415CF
	public ShopSceneItem[] GetWeaponItems(PlayerId player)
	{
		if (player == PlayerId.PlayerTwo)
		{
			return this.playerTwo.GetWeaponItemPrefabs();
		}
		return this.playerOne.GetWeaponItemPrefabs();
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x002431F0 File Offset: 0x002415F0
	public bool HasBoughtEverythingForAchievement(PlayerId player)
	{
		ShopSceneItem[] array = this.GetCharmItems(player);
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].isPurchasedForBuyAllItemsAchievement(player))
			{
				return false;
			}
		}
		array = this.GetWeaponItems(player);
		for (int j = 0; j < array.Length; j++)
		{
			if (!array[j].isPurchasedForBuyAllItemsAchievement(player))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x040049FF RID: 18943
	[SerializeField]
	private ShopScenePlayer playerOne;

	// Token: 0x04004A00 RID: 18944
	[SerializeField]
	private ShopScenePlayer playerTwo;

	// Token: 0x04004A01 RID: 18945
	[Space(10f)]
	[SerializeField]
	private ShopScenePig pig;

	// Token: 0x04004A02 RID: 18946
	public bool isDLCShop;
}
