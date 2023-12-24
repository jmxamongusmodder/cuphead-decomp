using UnityEngine;

public class ChessKingLevel : Level
{
	[SerializeField]
	private ChessKingLevelKing king;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
