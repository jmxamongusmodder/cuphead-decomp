using UnityEngine;
using TMPro;
using UnityEngine.PostProcessing;

public class WinScreen : AbstractMonoBehaviour
{
	[SerializeField]
	private float introDelay;
	[SerializeField]
	private float talliesDelay;
	[SerializeField]
	private float gradeDelay;
	[SerializeField]
	private float advanceDelay;
	[SerializeField]
	private WinScreenTicker timeTicker;
	[SerializeField]
	private WinScreenTicker hitsTicker;
	[SerializeField]
	private WinScreenTicker parriesTicker;
	[SerializeField]
	private WinScreenTicker superMeterTicker;
	[SerializeField]
	private WinScreenTicker difficultyTicker;
	[SerializeField]
	private LocalizationHelper spiritStockLabelLocalizationHelper;
	[SerializeField]
	private WinScreenGradeDisplay gradeDisplay;
	[SerializeField]
	private GameObject continuePrompt;
	[SerializeField]
	private GameObject scoring;
	[SerializeField]
	private TextMeshProUGUI gradeLabel;
	[SerializeField]
	private GameObject tryRegular;
	[SerializeField]
	private TMP_Text tryRegularText;
	[SerializeField]
	private SpriteRenderer tryRegularEnglishBackground;
	[SerializeField]
	private GameObject glowingText;
	[SerializeField]
	private GlowText glowScript;
	[SerializeField]
	private PostProcessingBehaviour postProcessingScript;
	[SerializeField]
	public PostProcessingProfile asianProfile;
	[SerializeField]
	private GameObject tryExpert;
	[SerializeField]
	private TMP_Text tryExpertText;
	[SerializeField]
	private Transform Background;
	[SerializeField]
	private GameObject OnePlayerCuphead;
	[SerializeField]
	private GameObject OnePlayerMugman;
	[SerializeField]
	private Transform OnePlayerUIRoot;
	[SerializeField]
	private Animator OnePlayerTitleCuphead;
	[SerializeField]
	private Animator OnePlayerTitleMugman;
	[SerializeField]
	private GameObject TwoPlayerCupheadMugman;
	[SerializeField]
	private GameObject TwoPlayerMugmanCuphead;
	[SerializeField]
	private Transform TwoPlayerCupheadMugmanUIRoot;
	[SerializeField]
	private Transform TwoPlayerMugmanCupheadUIRoot;
	[SerializeField]
	private Animator TwoPlayerTitleCuphead;
	[SerializeField]
	private Animator TwoPlayerTitleMugman;
	[SerializeField]
	private GameObject OnePlayerChalice;
	[SerializeField]
	private GameObject TwoPlayerChaliceCuphead;
	[SerializeField]
	private GameObject TwoPlayerCupheadChalice;
	[SerializeField]
	private GameObject TwoPlayerMugmanChalice;
	[SerializeField]
	private GameObject TwoPlayerChaliceMugman;
	[SerializeField]
	private Transform OnePlayerChaliceUIRoot;
	[SerializeField]
	private Transform TwoPlayerChaliceCupheadUIRoot;
	[SerializeField]
	private Transform TwoPlayerCupheadChaliceUIRoot;
	[SerializeField]
	private Transform TwoPlayerMugmanChaliceUIRoot;
	[SerializeField]
	private Transform TwoPlayerChaliceMugmanUIRoot;
	[SerializeField]
	private Animator OnePlayerTitleChalice;
	[SerializeField]
	private Animator TwoPlayerTitleChaliceCuphead;
	[SerializeField]
	private Animator TwoPlayerTitleCupheadChalice;
	[SerializeField]
	private Animator TwoPlayerTitleMugmanChalice;
	[SerializeField]
	private Animator TwoPlayerTitleChaliceMugman;
	[SerializeField]
	private SpriteRenderer[] studioMHDRSubtitles;
	[SerializeField]
	private Transform[] resultsTitles;
	[SerializeField]
	private Vector3 playerOneOffCenterTitleRoot;
	[SerializeField]
	private Vector3 japaneseTitleRoot;
	[SerializeField]
	private Vector3 koreanTitleRoot;
	[SerializeField]
	private Vector3 chineseTitleRoot;
	[SerializeField]
	private Vector3 chaliceTitleOffset1P;
	[SerializeField]
	private Vector3 chaliceTitleOffset2P;
	[SerializeField]
	private Canvas results;
	[SerializeField]
	private MinMax textWidthRange;
	[SerializeField]
	private MinMax curveScaleRange;
	[SerializeField]
	private float yOffsetDelta;
}
