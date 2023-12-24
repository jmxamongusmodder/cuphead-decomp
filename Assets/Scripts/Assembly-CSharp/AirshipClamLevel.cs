using UnityEngine;

public class AirshipClamLevel : Level
{
	[SerializeField]
	private AirshipClamLevelClam clam;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
