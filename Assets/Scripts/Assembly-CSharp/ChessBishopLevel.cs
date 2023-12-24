using UnityEngine;

public class ChessBishopLevel : ChessLevel
{
	[SerializeField]
	private ChessBishopLevelBishop bishop;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
