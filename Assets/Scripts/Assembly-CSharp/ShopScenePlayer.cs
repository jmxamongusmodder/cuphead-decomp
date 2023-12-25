using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rewired;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B0A RID: 2826
public class ShopScenePlayer : AbstractMonoBehaviour
{
	// Token: 0x17000625 RID: 1573
	// (get) Token: 0x0600448B RID: 17547 RVA: 0x002440C5 File Offset: 0x002424C5
	private ShopSceneItem CurrentItem
	{
		get
		{
			this.index = Mathf.Clamp(this.index, 0, this.items.Count - 1);
			return this.items[this.index];
		}
	}

	// Token: 0x140000C6 RID: 198
	// (add) Token: 0x0600448C RID: 17548 RVA: 0x002440F8 File Offset: 0x002424F8
	// (remove) Token: 0x0600448D RID: 17549 RVA: 0x00244130 File Offset: 0x00242530
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPurchaseEvent;

	// Token: 0x140000C7 RID: 199
	// (add) Token: 0x0600448E RID: 17550 RVA: 0x00244168 File Offset: 0x00242568
	// (remove) Token: 0x0600448F RID: 17551 RVA: 0x002441A0 File Offset: 0x002425A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnExitEvent;

	// Token: 0x06004490 RID: 17552 RVA: 0x002441D8 File Offset: 0x002425D8
	protected override void Awake()
	{
		base.Awake();
		this.doorPositionOpen = this.door.transform.localPosition.x;
		this.door.transform.SetLocalPosition(new float?(0f), null, null);
		this.doorPositionClosed = this.door.transform.localPosition.x;
		this.currencyCanvasOriginalScale = this.currencyCanvas.localScale.x;
		if (!PlayerManager.Multiplayer && this.player == PlayerId.PlayerTwo)
		{
			this.items[0].gameObject.SetActive(false);
			return;
		}
		this.weaponIndex = 0;
		this.charmIndex = 0;
		for (int i = 0; i < this.items.Count; i++)
		{
			ShopSceneItem shopSceneItem = null;
			ItemType itemType = this.items[i].itemType;
			if (itemType != ItemType.Weapon)
			{
				if (itemType == ItemType.Charm)
				{
					while (this.charmIndex < this.charmItemPrefabs.Length && (PlayerData.Data.IsUnlocked(this.player, this.charmItemPrefabs[this.charmIndex].charm) || !this.charmItemPrefabs[this.charmIndex].IsAvailable))
					{
						this.charmIndex++;
					}
					if (this.charmIndex < this.charmItemPrefabs.Length)
					{
						shopSceneItem = this.charmItemPrefabs[this.charmIndex];
						this.charmIndex++;
					}
				}
			}
			else
			{
				while (this.weaponIndex < this.weaponItemPrefabs.Length && (PlayerData.Data.IsUnlocked(this.player, this.weaponItemPrefabs[this.weaponIndex].weapon) || !this.weaponItemPrefabs[this.weaponIndex].IsAvailable))
				{
					this.weaponIndex++;
				}
				if (this.weaponIndex < this.weaponItemPrefabs.Length)
				{
					shopSceneItem = this.weaponItemPrefabs[this.weaponIndex];
					this.weaponIndex++;
				}
			}
			if (shopSceneItem == null)
			{
				this.items[i].gameObject.SetActive(false);
				this.items.RemoveAt(i);
				i--;
			}
			else
			{
				ShopSceneItem shopSceneItem2 = this.items[i];
				shopSceneItem2.gameObject.SetActive(false);
				this.items[i] = UnityEngine.Object.Instantiate<ShopSceneItem>(shopSceneItem);
				this.items[i].transform.position = shopSceneItem2.transform.position;
				this.items[i].spriteShadowObject.transform.SetParent(null);
			}
		}
		foreach (ShopSceneItem shopSceneItem3 in this.items)
		{
			shopSceneItem3.Init(this.player);
		}
	}

	// Token: 0x06004491 RID: 17553 RVA: 0x0024452C File Offset: 0x0024292C
	private void Start()
	{
		if (this.player != PlayerId.PlayerOne && !PlayerManager.Multiplayer)
		{
			base.enabled = false;
			base.gameObject.SetActive(false);
			return;
		}
		this.singleDigitCoinPosition = this.coinImage.transform.position;
		if (PlayerData.Data.GetCurrency(this.player) >= 10)
		{
			this.isMoneyDoubleDigit = true;
			this.coinImage.transform.position = this.doubleDigitCoinPosition.position;
		}
		PlayerManager.OnPlayerLeaveEvent += this.OnPlayerLeft;
		this.displayNameText.font = Localization.Instance.fonts[(int)Localization.language][41].fontAsset;
		this.subText.font = Localization.Instance.fonts[(int)Localization.language][41].fontAsset;
		this.descriptionText.font = Localization.Instance.fonts[(int)Localization.language][11].fontAsset;
	}

	// Token: 0x06004492 RID: 17554 RVA: 0x00244644 File Offset: 0x00242A44
	private void Update()
	{
		if (InterruptingPrompt.IsInterrupting())
		{
			return;
		}
		if (PlayerData.Data.GetCurrency(this.player) >= 10 && !this.isMoneyDoubleDigit)
		{
			this.isMoneyDoubleDigit = true;
			this.coinImage.transform.position = this.doubleDigitCoinPosition.position;
		}
		else if (PlayerData.Data.GetCurrency(this.player) < 10 && this.isMoneyDoubleDigit)
		{
			this.isMoneyDoubleDigit = false;
			this.coinImage.transform.position = this.singleDigitCoinPosition;
		}
		int currency = PlayerData.Data.GetCurrency(this.player);
		Sprite sprite;
		if (currency < 0)
		{
			sprite = this.coinSprites[0];
		}
		else if (currency > this.coinSprites.Count - 1)
		{
			sprite = this.coinSprites[this.coinSprites.Count - 1];
		}
		else
		{
			sprite = this.coinSprites[currency];
		}
		this.currencyNbImage.sprite = sprite;
		switch (this.state)
		{
		case ShopScenePlayer.State.Selecting:
			if (this.items.Count > 0 && this.CurrentItem.state != ShopSceneItem.State.Ready)
			{
				return;
			}
			if (this.items.Count > 1 && this.input.GetButtonDown(18))
			{
				AudioManager.Play("shop_selection_change");
				this.index--;
				this.UpdateSelection();
				return;
			}
			if (this.items.Count > 1 && this.input.GetButtonDown(20))
			{
				AudioManager.Play("shop_selection_change");
				this.index++;
				this.UpdateSelection();
				return;
			}
			if (this.items.Count > 0 && this.input.GetButtonDown(13))
			{
				if (this.CurrentItem.Purchase())
				{
					this.Purchase();
				}
				else
				{
					this.CantPurchase();
				}
			}
			if (this.input.GetButtonDown(14) || this.playerLeft)
			{
				this.Exit();
			}
			break;
		case ShopScenePlayer.State.Purchasing:
			return;
		case ShopScenePlayer.State.Exited:
			if (this.input.GetButtonDown(13) && !this.exitingShop)
			{
				this.StopAllCoroutines();
				this.state = ShopScenePlayer.State.Init;
				this.OnStart();
			}
			break;
		}
	}

	// Token: 0x06004493 RID: 17555 RVA: 0x002448DE File Offset: 0x00242CDE
	public ShopSceneItem[] GetWeaponItemPrefabs()
	{
		return this.weaponItemPrefabs;
	}

	// Token: 0x06004494 RID: 17556 RVA: 0x002448E6 File Offset: 0x00242CE6
	public ShopSceneItem[] GetCharmItemPrefabs()
	{
		return this.charmItemPrefabs;
	}

	// Token: 0x06004495 RID: 17557 RVA: 0x002448EE File Offset: 0x00242CEE
	public void OnStart()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x06004496 RID: 17558 RVA: 0x00244910 File Offset: 0x00242D10
	private void Purchase()
	{
		AudioManager.Play("shop_purchase");
		this.UpdateSelection();
		if (this.scaleCoinCoroutine != null)
		{
			base.StopCoroutine(this.scaleCoinCoroutine);
		}
		this.scaleCoinCoroutine = base.StartCoroutine(this.scaleCoin_cr());
		this.state = ShopScenePlayer.State.Purchasing;
		if (this.OnPurchaseEvent != null)
		{
			this.OnPurchaseEvent();
		}
	}

	// Token: 0x06004497 RID: 17559 RVA: 0x00244973 File Offset: 0x00242D73
	private void CantPurchase()
	{
		AudioManager.Play("shop_cantpurchase");
		if (this.moveItemCantPurchaseCoroutine != null)
		{
			base.StopCoroutine(this.moveItemCantPurchaseCoroutine);
		}
		this.moveItemCantPurchaseCoroutine = base.StartCoroutine(this.cantBuy_cr());
	}

	// Token: 0x06004498 RID: 17560 RVA: 0x002449A8 File Offset: 0x00242DA8
	private void Exit()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.state = ShopScenePlayer.State.Exiting;
		if (this.moveItemCantPurchaseCoroutine != null)
		{
			base.StopCoroutine(this.moveItemCantPurchaseCoroutine);
		}
		base.StartCoroutine(this.out_cr());
		if (this.OnExitEvent != null)
		{
			this.OnExitEvent();
		}
	}

	// Token: 0x06004499 RID: 17561 RVA: 0x00244A07 File Offset: 0x00242E07
	public void OnExit()
	{
		this.exitingShop = true;
	}

	// Token: 0x0600449A RID: 17562 RVA: 0x00244A10 File Offset: 0x00242E10
	private void UpdateSelection()
	{
		if (this.items.Count == 0)
		{
			Localization.Translation translation = Localization.Translate("out_of_stock_name");
			this.displayNameText.text = translation.text;
			this.displayNameText.font = translation.fonts.fontAsset;
			Localization.Translation translation2 = Localization.Translate("out_of_stock_subtext");
			this.subText.text = translation2.text;
			this.subText.font = translation2.fonts.fontAsset;
			Localization.Translation translation3 = Localization.Translate("out_of_stock_description");
			this.descriptionText.text = translation3.text;
			this.descriptionText.font = translation3.fonts.fontAsset;
			this.priceSpriteRenderer.enabled = false;
			this.chalkCoinSpriteRenderer.enabled = false;
			return;
		}
		foreach (ShopSceneItem shopSceneItem in this.items)
		{
			shopSceneItem.Deselect();
		}
		this.CurrentItem.Select();
		if (this.CurrentItem.Purchased)
		{
			this.displayNameText.text = Localization.Translate("item_purchased_name").text;
			this.subText.text = Localization.Translate("item_purchased_subtext").text;
			this.descriptionText.text = Localization.Translate("item_purchased_description").text;
			return;
		}
		this.displayNameText.text = this.CurrentItem.DisplayName.ToUpper();
		this.priceSpriteRenderer.sprite = this.priceSprites[this.CurrentItem.Value - 1];
		this.subText.text = this.CurrentItem.Subtext;
		this.descriptionText.text = this.CurrentItem.Description;
		if (this.CurrentItem.charm == Charm.charm_curse)
		{
			this.displayNameText.text = Localization.Translate("charm_broken_name").text.ToUpper();
			this.subText.text = Localization.Translate("charm_broken_subtext").text;
			this.descriptionText.text = Localization.Translate("charm_broken_description").text;
		}
	}

	// Token: 0x0600449B RID: 17563 RVA: 0x00244C84 File Offset: 0x00243084
	private void OnDoorTweened(float value)
	{
		this.door.SetLocalPosition(new float?(Mathf.Lerp(this.doorPositionClosed, this.doorPositionOpen, value)), null, null);
	}

	// Token: 0x0600449C RID: 17564 RVA: 0x00244CC8 File Offset: 0x002430C8
	private IEnumerator in_cr()
	{
		if (this.firstStart)
		{
			yield return new WaitForSeconds((this.player != PlayerId.PlayerOne) ? 1.4f : 1.1f);
		}
		this.firstStart = false;
		this.input = PlayerManager.GetPlayerInput(this.player);
		this.UpdateSelection();
		if (this.player == PlayerId.PlayerOne)
		{
			AudioManager.Play("shop_slide_open_cuphead");
		}
		else
		{
			AudioManager.Play("shop_slide_open_mugman");
		}
		yield return base.TweenValue(0f, 1f, 1f, EaseUtils.EaseType.easeOutBounce, new AbstractMonoBehaviour.TweenUpdateHandler(this.OnDoorTweened));
		this.state = ShopScenePlayer.State.Selecting;
		yield break;
	}

	// Token: 0x0600449D RID: 17565 RVA: 0x00244CE4 File Offset: 0x002430E4
	private IEnumerator out_cr()
	{
		if (this.player == PlayerId.PlayerOne)
		{
			AudioManager.Play("shop_slide_close_cuphead");
		}
		else
		{
			AudioManager.Play("shop_slide_close_mugman");
		}
		foreach (ShopSceneItem shopSceneItem in this.items)
		{
			shopSceneItem.Deselect();
		}
		yield return base.TweenValue(1f, 0f, 1f, EaseUtils.EaseType.easeOutBounce, new AbstractMonoBehaviour.TweenUpdateHandler(this.OnDoorTweened));
		this.state = ShopScenePlayer.State.Exited;
		yield break;
	}

	// Token: 0x0600449E RID: 17566 RVA: 0x00244D00 File Offset: 0x00243100
	private IEnumerator scaleCoin_cr()
	{
		while (this.currencyCanvas.localScale.x < this.currencyCanvasOriginalScale * this.currencyCanvasMultiplier)
		{
			this.currencyCanvas.localScale = new Vector2(this.currencyCanvas.localScale.x + this.currencyCanvasScaleValue, this.currencyCanvas.localScale.y + this.currencyCanvasScaleValue);
			yield return null;
		}
		while (this.currencyCanvas.localScale.x > this.currencyCanvasOriginalScale)
		{
			this.currencyCanvas.localScale = new Vector2(this.currencyCanvas.localScale.x - this.currencyCanvasScaleValue, this.currencyCanvas.localScale.y - this.currencyCanvasScaleValue);
			yield return null;
		}
		this.scaleCoinCoroutine = null;
		base.StartCoroutine(this.addNewItem_cr());
		yield break;
	}

	// Token: 0x0600449F RID: 17567 RVA: 0x00244D1C File Offset: 0x0024311C
	private IEnumerator cantBuy_cr()
	{
		float startPositionY = this.CurrentItem.endPosition.y;
		while (this.CurrentItem.transform.localPosition.y > startPositionY - this.CurrentItem.cantPurchaseYMovementPosition)
		{
			this.CurrentItem.transform.localPosition = new Vector2(this.CurrentItem.transform.localPosition.x, this.CurrentItem.transform.localPosition.y - this.CurrentItem.cantPurchaseYMovementValue);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		while (this.CurrentItem.transform.position.y < startPositionY)
		{
			this.CurrentItem.transform.localPosition = new Vector2(this.CurrentItem.transform.localPosition.x, this.CurrentItem.transform.localPosition.y + this.CurrentItem.cantPurchaseYMovementValue * 1.5f);
			yield return null;
		}
		this.moveItemCantPurchaseCoroutine = null;
		yield break;
	}

	// Token: 0x060044A0 RID: 17568 RVA: 0x00244D38 File Offset: 0x00243138
	private IEnumerator addNewItem_cr()
	{
		ItemType type = this.CurrentItem.itemType;
		ShopSceneItem originalItem = this.CurrentItem;
		if (type == ItemType.Charm)
		{
			bool foundItem = false;
			for (int i = this.charmIndex; i < this.charmItemPrefabs.Length; i++)
			{
				if (!PlayerData.Data.IsUnlocked(this.player, this.charmItemPrefabs[i].charm) && this.charmItemPrefabs[i].IsAvailable)
				{
					foundItem = true;
					int itemIndex = this.items.IndexOf(this.CurrentItem);
					this.items[itemIndex] = UnityEngine.Object.Instantiate<ShopSceneItem>(this.charmItemPrefabs[i]);
					this.items[itemIndex].player = this.player;
					this.items[itemIndex].startPosition = originalItem.startPosition;
					this.items[itemIndex].endPosition = originalItem.endPosition;
					Vector3 startPosition = this.items[itemIndex].startPosition;
					startPosition.y += 800f;
					this.items[itemIndex].transform.position = this.items[itemIndex].startPosition;
					this.items[itemIndex].spriteShadowObject.transform.SetParent(null);
					this.items[itemIndex].transform.position = startPosition;
					Vector3 originalShadowScale = this.items[itemIndex].spriteShadowObject.transform.localScale;
					this.items[itemIndex].spriteShadowObject.transform.localScale = Vector3.zero;
					this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].startPosition.y, 0.5f, EaseUtils.EaseType.linear);
					float t = 0f;
					float TIME = 0.5f;
					while (t < TIME)
					{
						float val = t / TIME;
						Vector3 newScale = Vector3.Lerp(Vector3.zero, originalShadowScale, EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, val));
						this.items[itemIndex].spriteShadowObject.transform.localScale = newScale;
						t += Time.deltaTime;
						yield return null;
					}
					SpriteRenderer dustPoof = UnityEngine.Object.Instantiate<SpriteRenderer>(this.poofPrefab, this.items[itemIndex].transform.position + this.items[itemIndex].poofOffset, Quaternion.identity);
					AudioManager.Play("item_drop");
					dustPoof.sortingOrder = 501;
					UnityEngine.Object.Destroy(dustPoof.gameObject, 3f);
					yield return this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].transform.position.y + 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return this.items[itemIndex].TweenLocalPositionY(this.items[itemIndex].transform.position.y, this.items[itemIndex].transform.position.y - 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return CupheadTime.WaitForSeconds(this, 0.2f);
					this.charmIndex = i + 1;
					this.UpdateSelection();
					break;
				}
			}
			if (!foundItem)
			{
				this.CurrentItem.spriteShadowObject.gameObject.SetActive(false);
				this.items.Remove(this.CurrentItem);
				this.UpdateSelection();
			}
		}
		else if (type == ItemType.Weapon)
		{
			bool foundItem2 = false;
			for (int j = this.weaponIndex; j < this.weaponItemPrefabs.Length; j++)
			{
				if (!PlayerData.Data.IsUnlocked(this.player, this.weaponItemPrefabs[j].weapon) && this.weaponItemPrefabs[j].IsAvailable)
				{
					foundItem2 = true;
					int itemIndex2 = this.items.IndexOf(this.CurrentItem);
					this.items[itemIndex2] = UnityEngine.Object.Instantiate<ShopSceneItem>(this.weaponItemPrefabs[j]);
					this.items[itemIndex2].player = this.player;
					this.items[itemIndex2].startPosition = originalItem.startPosition;
					this.items[itemIndex2].endPosition = originalItem.endPosition;
					Vector3 startPosition2 = this.items[itemIndex2].startPosition;
					startPosition2.y += 800f;
					this.items[itemIndex2].transform.position = this.items[itemIndex2].startPosition;
					this.items[itemIndex2].spriteShadowObject.transform.SetParent(null);
					this.items[itemIndex2].transform.position = startPosition2;
					Vector3 originalShadowScale2 = this.items[itemIndex2].spriteShadowObject.transform.localScale;
					this.items[itemIndex2].spriteShadowObject.transform.localScale = Vector3.zero;
					this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].startPosition.y, 0.5f, EaseUtils.EaseType.linear);
					float t2 = 0f;
					float TIME2 = 0.5f;
					while (t2 < TIME2)
					{
						float val2 = t2 / TIME2;
						Vector3 newScale2 = Vector3.Lerp(Vector3.zero, originalShadowScale2, EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, val2));
						this.items[itemIndex2].spriteShadowObject.transform.localScale = newScale2;
						t2 += Time.deltaTime;
						yield return null;
					}
					SpriteRenderer dustPoof2 = UnityEngine.Object.Instantiate<SpriteRenderer>(this.poofPrefab, this.items[itemIndex2].transform.position + this.items[itemIndex2].poofOffset, Quaternion.identity);
					AudioManager.Play("item_drop");
					dustPoof2.sortingOrder = 401;
					UnityEngine.Object.Destroy(dustPoof2.gameObject, 3f);
					yield return this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].transform.position.y + 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return this.items[itemIndex2].TweenLocalPositionY(this.items[itemIndex2].transform.position.y, this.items[itemIndex2].transform.position.y - 30f, 0.1f, EaseUtils.EaseType.linear);
					yield return CupheadTime.WaitForSeconds(this, 0.2f);
					this.weaponIndex = j + 1;
					this.UpdateSelection();
					break;
				}
			}
			if (!foundItem2)
			{
				this.CurrentItem.spriteShadowObject.gameObject.SetActive(false);
				this.items.Remove(this.CurrentItem);
				this.UpdateSelection();
			}
		}
		this.state = ShopScenePlayer.State.Selecting;
		yield break;
	}

	// Token: 0x060044A1 RID: 17569 RVA: 0x00244D53 File Offset: 0x00243153
	private void OnPlayerLeft(PlayerId playerId)
	{
		if (playerId == this.player)
		{
			this.playerLeft = true;
		}
	}

	// Token: 0x060044A2 RID: 17570 RVA: 0x00244D68 File Offset: 0x00243168
	private void OnDestroy()
	{
		this.weaponItemPrefabs = null;
		this.charmItemPrefabs = null;
		this.currencyNbImage = null;
		this.coinImage = null;
		this.priceSprites = null;
		this.poofPrefab = null;
		this.items = null;
	}

	// Token: 0x04004A30 RID: 18992
	private const float DOOR_TIME = 1f;

	// Token: 0x04004A31 RID: 18993
	private const float START_DELAY = 1f;

	// Token: 0x04004A32 RID: 18994
	[SerializeField]
	private PlayerId player;

	// Token: 0x04004A33 RID: 18995
	[Header("Visuals")]
	[SerializeField]
	private Transform door;

	// Token: 0x04004A34 RID: 18996
	[Header("Items")]
	[SerializeField]
	private List<ShopSceneItem> items;

	// Token: 0x04004A35 RID: 18997
	[SerializeField]
	private ShopSceneItem[] weaponItemPrefabs;

	// Token: 0x04004A36 RID: 18998
	[SerializeField]
	private ShopSceneItem[] charmItemPrefabs;

	// Token: 0x04004A37 RID: 18999
	[Header("UI Elements")]
	[SerializeField]
	private TMP_Text currencyText;

	// Token: 0x04004A38 RID: 19000
	[Space(10f)]
	[SerializeField]
	private TextMeshProUGUI displayNameText;

	// Token: 0x04004A39 RID: 19001
	[SerializeField]
	private TextMeshProUGUI subText;

	// Token: 0x04004A3A RID: 19002
	[SerializeField]
	private TextMeshProUGUI descriptionText;

	// Token: 0x04004A3B RID: 19003
	[SerializeField]
	private List<Sprite> coinSprites;

	// Token: 0x04004A3C RID: 19004
	[SerializeField]
	private Image currencyNbImage;

	// Token: 0x04004A3D RID: 19005
	[SerializeField]
	private Image coinImage;

	// Token: 0x04004A3E RID: 19006
	[SerializeField]
	private Transform doubleDigitCoinPosition;

	// Token: 0x04004A3F RID: 19007
	private Vector3 singleDigitCoinPosition;

	// Token: 0x04004A40 RID: 19008
	[SerializeField]
	private Transform currencyCanvas;

	// Token: 0x04004A41 RID: 19009
	[SerializeField]
	private float currencyCanvasScaleValue;

	// Token: 0x04004A42 RID: 19010
	[SerializeField]
	private float currencyCanvasMultiplier;

	// Token: 0x04004A43 RID: 19011
	[SerializeField]
	private SpriteRenderer poofPrefab;

	// Token: 0x04004A44 RID: 19012
	[SerializeField]
	private Sprite[] priceSprites;

	// Token: 0x04004A45 RID: 19013
	[SerializeField]
	private SpriteRenderer priceSpriteRenderer;

	// Token: 0x04004A46 RID: 19014
	[SerializeField]
	private SpriteRenderer chalkCoinSpriteRenderer;

	// Token: 0x04004A47 RID: 19015
	private Player input;

	// Token: 0x04004A48 RID: 19016
	private float doorPositionClosed;

	// Token: 0x04004A49 RID: 19017
	private float doorPositionOpen;

	// Token: 0x04004A4A RID: 19018
	public ShopScenePlayer.State state;

	// Token: 0x04004A4B RID: 19019
	private int index;

	// Token: 0x04004A4C RID: 19020
	private float currencyCanvasOriginalScale;

	// Token: 0x04004A4D RID: 19021
	private Coroutine scaleCoinCoroutine;

	// Token: 0x04004A4E RID: 19022
	private Coroutine moveItemCantPurchaseCoroutine;

	// Token: 0x04004A51 RID: 19025
	private bool exitingShop;

	// Token: 0x04004A52 RID: 19026
	private bool firstStart = true;

	// Token: 0x04004A53 RID: 19027
	private bool playerLeft;

	// Token: 0x04004A54 RID: 19028
	private bool isMoneyDoubleDigit;

	// Token: 0x04004A55 RID: 19029
	private int weaponIndex;

	// Token: 0x04004A56 RID: 19030
	private int charmIndex;

	// Token: 0x02000B0B RID: 2827
	public enum State
	{
		// Token: 0x04004A58 RID: 19032
		Init,
		// Token: 0x04004A59 RID: 19033
		Selecting,
		// Token: 0x04004A5A RID: 19034
		Viewing,
		// Token: 0x04004A5B RID: 19035
		Purchasing,
		// Token: 0x04004A5C RID: 19036
		Exiting,
		// Token: 0x04004A5D RID: 19037
		Exited
	}
}
