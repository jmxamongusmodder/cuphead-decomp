using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class SaltbakerLevel : Level
{
	[SerializeField]
	private GameObject phase3BG;
	[SerializeField]
	private GameObject[] cracksBG;
	[SerializeField]
	private SaltbakerLevelCutter cutterPrefab;
	[SerializeField]
	private SaltbakerLevelJumper jumperPrefab;
	[SerializeField]
	private SaltbakerLevelSaltbaker saltbaker;
	[SerializeField]
	private SaltbakerLevelBouncer saltbakerBouncer;
	[SerializeField]
	private SaltbakerLevelPillarHandler saltbakerPillarHandler;
	[SerializeField]
	private SpriteRenderer skyFront;
	[SerializeField]
	private SpriteRenderer transitionFader;
	[SerializeField]
	private SaltbakerLevelPhaseThreeToFourTransition phase3to4Transition;
	[SerializeField]
	private string saltSpillageOrderString;
	[SerializeField]
	private string saltSpillageDelayString;
	[SerializeField]
	private Animator groundCrack;
	[SerializeField]
	private Animator tornadoActivator;
	[SerializeField]
	private GameObject phaseFourBlurCamera;
	[SerializeField]
	private MeshRenderer phaseFourBlurTexture;
	[SerializeField]
	private float phaseFourBlurAmount;
	[SerializeField]
	private float phaseFourDimAmount;
	[SerializeField]
	private float phaseFourBlurDimTime;
	[SerializeField]
	private BlurOptimized phaseFourBlurController;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;
	[SerializeField]
	private Sprite _bossPortraitPhaseFour;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuotePhaseTwo;
	[SerializeField]
	private string _bossQuotePhaseThree;
	[SerializeField]
	private string _bossQuotePhaseFour;
	public float yScrollPos;
	[SerializeField]
	private SaltbakerLevelBGTrappedCharacter trappedCharacter;
	[SerializeField]
	private SaltbakerLevelBGTrappedCharacter trappedCharacterPhaseThree;
}
