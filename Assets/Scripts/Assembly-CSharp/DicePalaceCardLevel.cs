using UnityEngine;

public class DicePalaceCardLevel : AbstractDicePalaceLevel
{
	[SerializeField]
	private DicePalaceCardGameManager gameManager;
	[SerializeField]
	private DicePalaceCardLevelCard card;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private string _bossQuote;
}
