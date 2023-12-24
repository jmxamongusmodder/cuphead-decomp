using UnityEngine;

public class DicePalaceFlyingMemoryLevelCard : ParrySwitch
{
	public enum Card
	{
		Cuphead = 0,
		Chips = 1,
		Flowers = 2,
		Shield = 3,
		Spindle = 4,
		Mugman = 5,
	}

	public bool permanentlyFlipped;
	[SerializeField]
	private Sprite[] flippedUpCards;
	[SerializeField]
	private SpriteRenderer pinkDot;
	public Card card;
}
