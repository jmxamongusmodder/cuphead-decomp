using UnityEngine;

public class ChessBOldBLevel : Level
{
	[SerializeField]
	private ChessBOldBLevelBoss boss;
	[SerializeField]
	private ChessBOldBLevelGameManager gameManager;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
