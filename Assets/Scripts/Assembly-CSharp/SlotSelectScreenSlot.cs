using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SlotSelectScreenSlot : AbstractMonoBehaviour
{
	[SerializeField]
	private RectTransform emptyChild;
	[SerializeField]
	private RectTransform mainChild;
	[SerializeField]
	private RectTransform mainDLCChild;
	[SerializeField]
	private TMP_Text worldMapText;
	[SerializeField]
	private TMP_Text worldMapTextDLC;
	[SerializeField]
	private Image boxImage;
	[SerializeField]
	private Image starImage;
	[SerializeField]
	private Image starImageDLC;
	[SerializeField]
	private Image starImageSelectedBase;
	[SerializeField]
	private Image starImageSelectedDLC;
	[SerializeField]
	private Image noiseImage;
	[SerializeField]
	private Sprite unselectedBoxSprite;
	[SerializeField]
	private Sprite unselectedBoxSpriteExpert;
	[SerializeField]
	private Sprite unselectedBoxSpriteComplete;
	[SerializeField]
	private Sprite unselectedBoxSpriteExpertDLC;
	[SerializeField]
	private Sprite unselectedBoxSpriteCompleteDLC;
	[SerializeField]
	private Sprite unselectedNoise;
	[SerializeField]
	private Sprite selectedBoxSpriteMugman;
	[SerializeField]
	private Sprite selectedBoxSprite;
	[SerializeField]
	private Sprite selectedBoxSpriteExpert;
	[SerializeField]
	private Sprite selectedBoxSpriteComplete;
	[SerializeField]
	private Sprite selectedBoxSpriteExpertDLC;
	[SerializeField]
	private Sprite selectedBoxSpriteCompleteDLC;
	[SerializeField]
	private Sprite selectedNoiseMugman;
	[SerializeField]
	private Sprite selectedNoise;
	[SerializeField]
	private GameObject cuphead;
	[SerializeField]
	private Animator cupheadSelect;
	[SerializeField]
	private Animator cupheadAnimator;
	[SerializeField]
	private GameObject mugman;
	[SerializeField]
	private Animator mugmanSelect;
	[SerializeField]
	private Animator mugmanAnimator;
	[SerializeField]
	private TMP_Text slotTitle;
	[SerializeField]
	private TMP_Text slotSeparator;
	[SerializeField]
	private TMP_Text slotPercentage;
	[SerializeField]
	private TMP_Text slotPercentageSelectedBase;
	[SerializeField]
	private TMP_Text slotPercentageSelectedDLC;
	[SerializeField]
	private Text emptyText;
	[SerializeField]
	private Color selectedTextColor;
	[SerializeField]
	private Color unselectedTextColor;
}
