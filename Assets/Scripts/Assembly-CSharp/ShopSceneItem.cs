using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000B06 RID: 2822
public class ShopSceneItem : AbstractMonoBehaviour
{
	// Token: 0x1700061E RID: 1566
	// (get) Token: 0x06004471 RID: 17521 RVA: 0x00243780 File Offset: 0x00241B80
	// (set) Token: 0x06004472 RID: 17522 RVA: 0x00243788 File Offset: 0x00241B88
	public ShopSceneItem.State state { get; private set; }

	// Token: 0x1700061F RID: 1567
	// (get) Token: 0x06004473 RID: 17523 RVA: 0x00243791 File Offset: 0x00241B91
	public bool Purchased
	{
		get
		{
			return this.isPurchased(this.player);
		}
	}

	// Token: 0x06004474 RID: 17524 RVA: 0x002437A0 File Offset: 0x00241BA0
	private bool isPurchased(PlayerId player)
	{
		switch (this.itemType)
		{
		case ItemType.Weapon:
			return PlayerData.Data.IsUnlocked(player, this.weapon);
		case ItemType.Super:
			return PlayerData.Data.IsUnlocked(player, this.super);
		case ItemType.Charm:
			return PlayerData.Data.IsUnlocked(player, this.charm);
		default:
			return false;
		}
	}

	// Token: 0x06004475 RID: 17525 RVA: 0x00243802 File Offset: 0x00241C02
	public bool isPurchasedForBuyAllItemsAchievement(PlayerId player)
	{
		return this.isDLCItem || this.isPurchased(player);
	}

	// Token: 0x17000620 RID: 1568
	// (get) Token: 0x06004476 RID: 17526 RVA: 0x00243818 File Offset: 0x00241C18
	public string DisplayName
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetDisplayName(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetDisplayName(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetDisplayName(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x06004477 RID: 17527 RVA: 0x0024386C File Offset: 0x00241C6C
	public string Subtext
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetSubtext(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetSubtext(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetSubtext(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x17000622 RID: 1570
	// (get) Token: 0x06004478 RID: 17528 RVA: 0x002438C0 File Offset: 0x00241CC0
	public string Description
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetDescription(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetDescription(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetDescription(this.charm);
			default:
				return string.Empty;
			}
		}
	}

	// Token: 0x17000623 RID: 1571
	// (get) Token: 0x06004479 RID: 17529 RVA: 0x00243914 File Offset: 0x00241D14
	public int Value
	{
		get
		{
			switch (this.itemType)
			{
			case ItemType.Weapon:
				return WeaponProperties.GetValue(this.weapon);
			case ItemType.Super:
				return WeaponProperties.GetValue(this.super);
			case ItemType.Charm:
				return WeaponProperties.GetValue(this.charm);
			default:
				return 0;
			}
		}
	}

	// Token: 0x17000624 RID: 1572
	// (get) Token: 0x0600447A RID: 17530 RVA: 0x00243964 File Offset: 0x00241D64
	public bool IsAvailable
	{
		get
		{
			return !this.isDLCItem || (this.isDLCItem && DLCManager.DLCEnabled());
		}
	}

	// Token: 0x0600447B RID: 17531 RVA: 0x00243988 File Offset: 0x00241D88
	public void Init(PlayerId player)
	{
		this.startPosition = base.transform.localPosition;
		this.endPosition = this.startPosition;
		this.endPosition.y = this.endPosition.y + 40f;
		this.player = player;
		if (this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Purchased);
		}
		else
		{
			this.SetSprite(ShopSceneItem.SpriteState.Inactive);
		}
	}

	// Token: 0x0600447C RID: 17532 RVA: 0x002439F0 File Offset: 0x00241DF0
	private void SetSprite(ShopSceneItem.SpriteState spriteState)
	{
		this.spriteInactive.enabled = false;
		this.spriteSelected.enabled = false;
		this.spritePurchased.enabled = false;
		switch (spriteState)
		{
		case ShopSceneItem.SpriteState.Inactive:
			this.spriteInactive.enabled = true;
			break;
		case ShopSceneItem.SpriteState.Selected:
			this.spriteSelected.enabled = true;
			break;
		case ShopSceneItem.SpriteState.Purchased:
			this.spritePurchased.enabled = true;
			break;
		}
	}

	// Token: 0x0600447D RID: 17533 RVA: 0x00243A70 File Offset: 0x00241E70
	public void Select()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return;
		}
		if (!this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Selected);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.float_cr(base.transform.localPosition, this.endPosition, this.spriteShadowObject.transform.localScale, this.originalShadowScale * 0.8f));
	}

	// Token: 0x0600447E RID: 17534 RVA: 0x00243AE0 File Offset: 0x00241EE0
	public void Deselect()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return;
		}
		if (!this.Purchased)
		{
			this.SetSprite(ShopSceneItem.SpriteState.Inactive);
		}
		this.StopAllCoroutines();
		base.StartCoroutine(this.float_cr(base.transform.localPosition, this.startPosition, this.spriteShadowObject.transform.localScale, this.originalShadowScale));
	}

	// Token: 0x0600447F RID: 17535 RVA: 0x00243B45 File Offset: 0x00241F45
	private void UpdateFloat(float value)
	{
		base.transform.localPosition = Vector3.Lerp(this.startPosition, this.endPosition, value);
	}

	// Token: 0x06004480 RID: 17536 RVA: 0x00243B64 File Offset: 0x00241F64
	private void UpdatePurchasedColor(float value)
	{
		Color white = Color.white;
		Color black = Color.black;
		this.spritePurchased.color = Color.Lerp(white, black, value);
	}

	// Token: 0x06004481 RID: 17537 RVA: 0x00243B90 File Offset: 0x00241F90
	public bool Purchase()
	{
		if (this.state != ShopSceneItem.State.Ready)
		{
			return false;
		}
		if (this.Purchased)
		{
			return false;
		}
		bool flag = false;
		switch (this.itemType)
		{
		case ItemType.Weapon:
			flag = PlayerData.Data.Buy(this.player, this.weapon);
			break;
		case ItemType.Super:
			flag = PlayerData.Data.Buy(this.player, this.super);
			break;
		case ItemType.Charm:
			flag = PlayerData.Data.Buy(this.player, this.charm);
			break;
		}
		if (flag)
		{
			base.StartCoroutine(this.purchase_cr());
			if (ShopScene.Current.HasBoughtEverythingForAchievement(this.player))
			{
				OnlineManager.Instance.Interface.UnlockAchievement(this.player, "BoughtAllItems");
			}
			if (!PlayerData.Data.hasMadeFirstPurchase)
			{
				PlayerData.Data.shouldShowShopkeepTooltip = true;
				PlayerData.Data.hasMadeFirstPurchase = true;
				PlayerData.SaveCurrentFile();
			}
		}
		return flag;
	}

	// Token: 0x06004482 RID: 17538 RVA: 0x00243CA0 File Offset: 0x002420A0
	private IEnumerator float_cr(Vector3 start, Vector3 end, Vector3 startShadowScale, Vector3 endShadowScale)
	{
		float t = 0f;
		float time = 0.3f * (Vector3.Distance(start, end) / Vector3.Distance(this.startPosition, this.endPosition));
		while (t < time)
		{
			float val = t / time;
			base.transform.localPosition = Vector3.Lerp(start, end, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
			this.spriteShadowObject.transform.localScale = Vector3.Lerp(startShadowScale, endShadowScale, EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, val));
			t += base.LocalDeltaTime;
			yield return null;
		}
		base.transform.localPosition = end;
		yield return null;
		yield break;
	}

	// Token: 0x06004483 RID: 17539 RVA: 0x00243CD8 File Offset: 0x002420D8
	private IEnumerator purchase_cr()
	{
		this.state = ShopSceneItem.State.Busy;
		this.SetSprite(ShopSceneItem.SpriteState.Purchased);
		SpriteRenderer buyAnim = UnityEngine.Object.Instantiate<SpriteRenderer>(this.buyAnimation, base.GetComponentInChildren<SpriteRenderer>().bounds.center, Quaternion.identity);
		buyAnim.sortingOrder = base.GetComponentInChildren<SpriteRenderer>().sortingOrder;
		this.spriteShadowObject.gameObject.SetActive(false);
		yield return base.TweenValue(0f, 1f, 0.0001f, EaseUtils.EaseType.linear, new AbstractMonoBehaviour.TweenUpdateHandler(this.UpdatePurchasedColor));
		this.state = ShopSceneItem.State.Ready;
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06004484 RID: 17540 RVA: 0x00243CF3 File Offset: 0x002420F3
	private void OnDestroy()
	{
		this.buyAnimation = null;
		this.spriteShadow = null;
	}

	// Token: 0x04004A10 RID: 18960
	private const float FLOAT_TIME = 0.3f;

	// Token: 0x04004A11 RID: 18961
	public ItemType itemType;

	// Token: 0x04004A12 RID: 18962
	[Space(5f)]
	public Weapon weapon = Weapon.None;

	// Token: 0x04004A13 RID: 18963
	public Super super = Super.None;

	// Token: 0x04004A14 RID: 18964
	public Charm charm = Charm.None;

	// Token: 0x04004A15 RID: 18965
	[Header("Sprites")]
	public SpriteRenderer spriteInactive;

	// Token: 0x04004A16 RID: 18966
	public SpriteRenderer spriteSelected;

	// Token: 0x04004A17 RID: 18967
	public SpriteRenderer spritePurchased;

	// Token: 0x04004A18 RID: 18968
	public SpriteRenderer spriteShadowObject;

	// Token: 0x04004A19 RID: 18969
	public Sprite spriteShadow;

	// Token: 0x04004A1A RID: 18970
	public float cantPurchaseYMovementPosition;

	// Token: 0x04004A1B RID: 18971
	public float cantPurchaseYMovementValue;

	// Token: 0x04004A1C RID: 18972
	public Vector3 poofOffset;

	// Token: 0x04004A1D RID: 18973
	[HideInInspector]
	public Vector3 endPosition;

	// Token: 0x04004A1E RID: 18974
	public PlayerId player;

	// Token: 0x04004A1F RID: 18975
	[HideInInspector]
	public Vector3 startPosition;

	// Token: 0x04004A20 RID: 18976
	public Vector3 originalShadowScale;

	// Token: 0x04004A21 RID: 18977
	public SpriteRenderer buyAnimation;

	// Token: 0x04004A22 RID: 18978
	private Coroutine selectionCoroutine;

	// Token: 0x04004A24 RID: 18980
	[SerializeField]
	private bool isDLCItem;

	// Token: 0x02000B07 RID: 2823
	public enum State
	{
		// Token: 0x04004A26 RID: 18982
		Ready,
		// Token: 0x04004A27 RID: 18983
		Busy
	}

	// Token: 0x02000B08 RID: 2824
	public enum SpriteState
	{
		// Token: 0x04004A29 RID: 18985
		Inactive,
		// Token: 0x04004A2A RID: 18986
		Selected,
		// Token: 0x04004A2B RID: 18987
		Purchased
	}
}
