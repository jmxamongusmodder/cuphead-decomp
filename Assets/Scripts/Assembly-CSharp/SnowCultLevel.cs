using UnityEngine;

public class SnowCultLevel : Level
{
	[SerializeField]
	private SnowCultLevelWizard wizard;
	[SerializeField]
	private SnowCultLevelYeti yeti;
	[SerializeField]
	private SnowCultLevelJackFrost jackFrost;
	[SerializeField]
	private Animator cultists;
	[SerializeField]
	private GameObject pit;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuotePhaseTwo;
	[SerializeField]
	private string _bossQuotePhaseThree;
}
