using UnityEngine;

public class DicePalaceMainLevel : AbstractDicePalaceLevel
{
	[SerializeField]
	private DicePalaceMainLevelGameManager gameManager;
	[SerializeField]
	private DicePalaceMainLevelKingDice kingDice;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private string _bossQuote;
}
