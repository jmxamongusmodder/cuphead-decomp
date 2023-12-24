using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotSelectScreen : AbstractMonoBehaviour
{
	[SerializeField]
	private RectTransform LoadingChild;
	[SerializeField]
	private RectTransform mainMenuChild;
	[SerializeField]
	private RectTransform slotSelectChild;
	[SerializeField]
	private RectTransform confirmDeleteChild;
	[SerializeField]
	private Text[] mainMenuItems;
	[SerializeField]
	private SlotSelectScreenSlot[] slots;
	[SerializeField]
	private Text[] confirmDeleteItems;
	[SerializeField]
	private RectTransform playerProfiles;
	[SerializeField]
	private RectTransform confirmPrompt;
	[SerializeField]
	private RectTransform confirmGlyph;
	[SerializeField]
	private RectTransform confirmSpacer;
	[SerializeField]
	private RectTransform backPrompt;
	[SerializeField]
	private RectTransform backGlyph;
	[SerializeField]
	private RectTransform backSpacer;
	[SerializeField]
	private RectTransform storePrompt;
	[SerializeField]
	private RectTransform storeGlyph;
	[SerializeField]
	private RectTransform storeSpacer;
	[SerializeField]
	private RectTransform deletePrompt;
	[SerializeField]
	private RectTransform deleteGlyph;
	[SerializeField]
	private RectTransform deleteSpacer;
	[SerializeField]
	private RectTransform prompts;
	[SerializeField]
	private Color mainMenuSelectedColor;
	[SerializeField]
	private Color mainMenuUnselectedColor;
	[SerializeField]
	private Color confirmDeleteSelectedColor;
	[SerializeField]
	private Color confirmDeleteUnselectedColor;
	[SerializeField]
	private OptionsGUI optionsPrefab;
	[SerializeField]
	private RectTransform optionsRoot;
	[SerializeField]
	private AchievementsGUI achievementsPrefab;
	[SerializeField]
	private RectTransform achievementsRoot;
	[SerializeField]
	private DLCGUI dlcMenuPrefab;
	[SerializeField]
	private RectTransform dlcMenuRoot;
	[SerializeField]
	private TMP_Text confirmDeleteSlotTitle;
	[SerializeField]
	private TMP_Text confirmDeleteSlotSeparator;
	[SerializeField]
	private TMP_Text confirmDeleteSlotPercentage;
}
