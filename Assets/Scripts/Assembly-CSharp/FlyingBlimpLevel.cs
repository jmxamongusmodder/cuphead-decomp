using UnityEngine;

public class FlyingBlimpLevel : Level
{
	[SerializeField]
	private FlyingBlimpLevelBlimpLady blimpLady;
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;
	public bool changingStates;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitMagical;
	[SerializeField]
	private Sprite _bossPortraitMoon;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteMagical;
	[SerializeField]
	private string _bossQuoteMoon;
}
