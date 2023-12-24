using UnityEngine;

public class AirshipStorkLevel : Level
{
	[SerializeField]
	private AirshipStorkLevelStork stork;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
