using UnityEngine;

public class AirshipJellyLevel : Level
{
	[SerializeField]
	private AirshipJellyLevelJelly jelly;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
