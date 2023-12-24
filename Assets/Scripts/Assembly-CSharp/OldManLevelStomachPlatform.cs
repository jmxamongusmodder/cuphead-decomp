using UnityEngine;

public class OldManLevelStomachPlatform : LevelPlatform
{
	[SerializeField]
	private SpriteRenderer[] rend;
	[SerializeField]
	private BoxCollider2D boxColl;
	public bool isTargeted;
	public Animator sparkAnimator;
	public OldManLevelGnomeLeader main;
	[SerializeField]
	private Animator splashAnimator;
	[SerializeField]
	private Animator mouthBubble;
	[SerializeField]
	private Animator noseBubble;
	[SerializeField]
	private SpriteRenderer mouthBubbleRend;
	[SerializeField]
	private SpriteRenderer noseBubbleRend;
	[SerializeField]
	private MinMax noseBubbleRange;
	[SerializeField]
	private MinMax mouthBubbleRange;
}
