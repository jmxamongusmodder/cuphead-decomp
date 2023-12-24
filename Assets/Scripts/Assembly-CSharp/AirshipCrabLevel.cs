using UnityEngine;

public class AirshipCrabLevel : Level
{
	[SerializeField]
	private AirshipCrabLevelCrab crab;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
