using UnityEngine;

public class FlyingCowboyLevel : Level
{
	[SerializeField]
	private FlyingCowboyLevelCowboy cowboy;
	[SerializeField]
	private FlyingCowboyLevelMeat meat;
	[SerializeField]
	private FlyingCowboyLevelBackground background;
	[SerializeField]
	private PlanePlayerDust playerDust;
	[SerializeField]
	private float playerDustSmallTrigger;
	[SerializeField]
	private float playerDustLargeTrigger;
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
