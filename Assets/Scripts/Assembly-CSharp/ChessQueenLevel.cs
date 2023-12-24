using UnityEngine;

public class ChessQueenLevel : ChessLevel
{
	[SerializeField]
	private ChessQueenLevelQueen queen;
	[SerializeField]
	private Animator[] mouseAnimator;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private string _bossQuoteMain;
	public bool cannonBlastFXVariant;
}
