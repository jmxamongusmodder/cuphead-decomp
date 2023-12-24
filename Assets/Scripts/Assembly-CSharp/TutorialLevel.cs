using UnityEngine;

public class TutorialLevel : Level
{
	[SerializeField]
	private Transform background;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private PlayerDeathEffect[] playerGoBackToHouseEffects;
}
