using UnityEngine;
using TMPro;
using UnityEngine.PostProcessing;

public class TowerOfPowerScorecard : AbstractMonoBehaviour
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
	private Canvas results;
	[SerializeField]
	private MinMax textWidthRange;
	[SerializeField]
	private MinMax curveScaleRange;
	[SerializeField]
	private float yOffsetDelta;
	public bool done;
}
