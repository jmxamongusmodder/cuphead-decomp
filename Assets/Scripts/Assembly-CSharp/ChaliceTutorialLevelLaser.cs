using UnityEngine;

public class ChaliceTutorialLevelLaser : AbstractCollidableObject
{
	[SerializeField]
	private ChaliceTutorialLevel level;
	[SerializeField]
	private ChaliceTutorialLevelParryable parryable;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private Animator hitAnimator;
}
