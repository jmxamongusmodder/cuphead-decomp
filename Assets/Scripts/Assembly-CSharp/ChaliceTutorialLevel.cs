using UnityEngine;

public class ChaliceTutorialLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private Animator backgroundAnimator;
	[SerializeField]
	private ChaliceTutorialLevelParryable[] parrybles;
	public bool resetParryables;
	[SerializeField]
	private PlayerDeathEffect[] playerExitEffects;
}
