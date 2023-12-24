using UnityEngine;

public class SlimeLevel : Level
{
	[SerializeField]
	private SlimeLevelSlime smallSlime;
	[SerializeField]
	private SlimeLevelSlime bigSlime;
	[SerializeField]
	private SlimeLevelTombstone tombStone;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitBigSlime;
	[SerializeField]
	private Sprite _bossPortraitTombstone;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteBigSlime;
	[SerializeField]
	private string _bossQuoteTombstone;
}
