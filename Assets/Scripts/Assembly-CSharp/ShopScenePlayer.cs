using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ShopScenePlayer : AbstractMonoBehaviour
{
	public enum State
	{
		Init = 0,
		Selecting = 1,
		Viewing = 2,
		Purchasing = 3,
		Exiting = 4,
		Exited = 5,
	}

	[SerializeField]
	private PlayerId player;
	[SerializeField]
	private Transform door;
	[SerializeField]
	private List<ShopSceneItem> items;
	[SerializeField]
	private ShopSceneItem[] weaponItemPrefabs;
	[SerializeField]
	private ShopSceneItem[] charmItemPrefabs;
	[SerializeField]
	private TMP_Text currencyText;
	[SerializeField]
	private TextMeshProUGUI displayNameText;
	[SerializeField]
	private TextMeshProUGUI subText;
	[SerializeField]
	private TextMeshProUGUI descriptionText;
	[SerializeField]
	private List<Sprite> coinSprites;
	[SerializeField]
	private Image currencyNbImage;
	[SerializeField]
	private Image coinImage;
	[SerializeField]
	private Transform doubleDigitCoinPosition;
	[SerializeField]
	private Transform currencyCanvas;
	[SerializeField]
	private float currencyCanvasScaleValue;
	[SerializeField]
	private float currencyCanvasMultiplier;
	[SerializeField]
	private SpriteRenderer poofPrefab;
	[SerializeField]
	private Sprite[] priceSprites;
	[SerializeField]
	private SpriteRenderer priceSpriteRenderer;
	[SerializeField]
	private SpriteRenderer chalkCoinSpriteRenderer;
	public State state;
}
