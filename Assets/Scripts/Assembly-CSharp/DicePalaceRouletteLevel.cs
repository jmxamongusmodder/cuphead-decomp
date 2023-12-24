using UnityEngine;

public class DicePalaceRouletteLevel : AbstractDicePalaceLevel
{
	[SerializeField]
	private DicePalaceRouletteLevelRoulette roulette;
	[SerializeField]
	private DicePalaceRouletteLevelPlatform[] platforms;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private string _bossQuote;
}
