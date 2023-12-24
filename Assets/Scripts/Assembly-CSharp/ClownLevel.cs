using UnityEngine;

public class ClownLevel : Level
{
	[SerializeField]
	private ClownLevelClown clown;
	[SerializeField]
	private ClownLevelClownHelium clownHelium;
	[SerializeField]
	private ClownLevelClownHorse clownHorse;
	[SerializeField]
	private ClownLevelClownSwing clownSwing;
	[SerializeField]
	private ClownLevelCoasterHandler coasterHandler;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitHeliumTank;
	[SerializeField]
	private Sprite _bossPortraitCarouselHorse;
	[SerializeField]
	private Sprite _bossPortraitSwing;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteHeliumTank;
	[SerializeField]
	private string _bossQuoteCarouselHorse;
	[SerializeField]
	private string _bossQuoteSwing;
}
