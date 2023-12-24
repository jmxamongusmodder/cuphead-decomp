using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MapEventNotification : AbstractMonoBehaviour
{
	[SerializeField]
	private Image background;
	[SerializeField]
	private TextMeshProUGUI text;
	[SerializeField]
	private LocalizationHelper localizationHelper;
	[SerializeField]
	private LocalizationHelper notificationLocalizationHelper;
	[SerializeField]
	private RectTransform sparkleTransformContract;
	[SerializeField]
	private RectTransform sparkleTransformCoin1;
	[SerializeField]
	private RectTransform sparkleTransformCoin2;
	[SerializeField]
	private RectTransform sparkleTransformCoin3;
	[SerializeField]
	private GameObject sparklePrefab;
	[SerializeField]
	private CanvasGroup glyphCanvasGroup;
	[SerializeField]
	private GameObject coin2;
	[SerializeField]
	private GameObject coin3;
	[SerializeField]
	private GameObject coinVariable;
	[SerializeField]
	private Text coinVariableText;
	[SerializeField]
	private GameObject super1;
	[SerializeField]
	private GameObject super2;
	[SerializeField]
	private GameObject super3;
	[SerializeField]
	private GameObject curseCharm;
	[SerializeField]
	private GameObject ingredientStarburst;
	[SerializeField]
	private GameObject airplaneIngred;
	[SerializeField]
	private GameObject rumIngred;
	[SerializeField]
	private GameObject oldManIngred;
	[SerializeField]
	private GameObject snowCultIngred;
	[SerializeField]
	private GameObject cowboyIngred;
	[SerializeField]
	private GameObject confirmGlyph;
	[SerializeField]
	private GameObject dlcUIPrefab;
	[SerializeField]
	private Transform dlcUIRoot;
	[SerializeField]
	private CanvasGroup tooltipCanvasGroup;
	[SerializeField]
	private Image tooltipPortrait;
	[SerializeField]
	private LocalizationHelper tooltipLocalizationHelper;
	[SerializeField]
	private GameObject tooltipEquipGlyph;
	[SerializeField]
	private Sprite TurtleSprite;
	[SerializeField]
	private Sprite CanteenSprite;
	[SerializeField]
	private Sprite ShopkeepSprite;
	[SerializeField]
	private Sprite ForkSprite;
	[SerializeField]
	private Sprite KingDiceSprite;
	[SerializeField]
	private Sprite MausoleumSprite;
	[SerializeField]
	private Sprite SaltbakerSpriteA;
	[SerializeField]
	private Sprite SaltbakerSpriteB;
	[SerializeField]
	private Sprite ChaliceSprite;
	[SerializeField]
	private Sprite ChaliceFanSprite;
	[SerializeField]
	private Sprite BoatmanSprite;
	[SerializeField]
	private float timeBetweenSparkle;
}
