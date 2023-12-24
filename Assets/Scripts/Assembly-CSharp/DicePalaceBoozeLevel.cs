using UnityEngine;

public class DicePalaceBoozeLevel : AbstractDicePalaceLevel
{
	[SerializeField]
	private Transform[] lamps;
	[SerializeField]
	private DicePalaceBoozeLevelDecanter decanter;
	[SerializeField]
	private DicePalaceBoozeLevelMartini martini;
	[SerializeField]
	private DicePalaceBoozeLevelTumbler tumbler;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private string _bossQuote;
}
