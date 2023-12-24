using UnityEngine;

public class RumRunnersLevel : Level
{
	[SerializeField]
	private Animator ph2SpiderAnimation;
	[SerializeField]
	private RumRunnersLevelSpider spider;
	[SerializeField]
	private RumRunnersLevelWorm worm;
	[SerializeField]
	private RumRunnersLevelAnteater anteater;
	[SerializeField]
	private RumRunnersLevelMobIntroAnimation mobIntro;
	[SerializeField]
	private Effect fullscreenDirtFX;
	[SerializeField]
	private Animator fakeBannerAnimator;
	[SerializeField]
	private GameObject[] destroyedSpritesMiddleA;
	[SerializeField]
	private GameObject[] destroyedSpritesMiddleB;
	[SerializeField]
	private GameObject[] destroyedSpritesUpperA;
	[SerializeField]
	private GameObject[] destroyedSpritesUpperB;
	[SerializeField]
	private LevelPlatform[] destroyedPlatformsMiddle;
	[SerializeField]
	private LevelPlatform[] destroyedPlatformsUpper;
	[SerializeField]
	private LevelPlatform[] swapPlatformsMappingBefore;
	[SerializeField]
	private LevelPlatform[] swapPlatformsMappingAfter;
	[SerializeField]
	private Rangef middlePlatformEffectRange;
	[SerializeField]
	private Rangef topPlatformEffectRange;
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
}
