using UnityEngine;

public class TowerOfPowerLevel : Level
{
	[SerializeField]
	private TowerOfPowerLevelGameManager gameManager;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
