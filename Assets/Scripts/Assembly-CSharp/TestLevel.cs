using UnityEngine;

public class TestLevel : Level
{
	[SerializeField]
	private TestLevelFlyingJared jared;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
