using UnityEngine;

public class ShmupTutorialLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	public Animator canvasAnimator;
	public float waitForAnimationTime;
}
