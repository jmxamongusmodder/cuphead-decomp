using System.Collections.Generic;
using UnityEngine;

public class PlatformingLevel : Level
{
	public enum Theme
	{
		Forest = 0,
	}

	public List<CoinPositionAndID> LevelCoinsIDs;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	private Sprite _bossPortraitAlt;
	[SerializeField]
	private string _bossQuote;
	[SerializeField]
	private string _bossQuoteAlt;
	[SerializeField]
	private float goalTime;
	public bool useAltQuote;
}
