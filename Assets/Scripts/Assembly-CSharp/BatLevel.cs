using UnityEngine;

public class BatLevel : Level
{
	[SerializeField]
	private BatLevelBat bat;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
