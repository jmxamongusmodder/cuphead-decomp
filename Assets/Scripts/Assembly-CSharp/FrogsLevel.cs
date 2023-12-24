using UnityEngine;

public class FrogsLevel : Level
{
	[SerializeField]
	private FrogsLevelTall tall;
	[SerializeField]
	private FrogsLevelShort small;
	[SerializeField]
	private FrogsLevelMorphed morphed;
	[SerializeField]
	private FrogsLevelDemonTrigger demonTrigger;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitRoll;
	[SerializeField]
	private Sprite _bossPortraitMorph;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteRoll;
	[SerializeField]
	private string _bossQuoteMorph;
}
