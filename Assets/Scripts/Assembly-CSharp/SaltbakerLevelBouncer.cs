using UnityEngine;

public class SaltbakerLevelBouncer : LevelProperties.Saltbaker.Entity
{
	[SerializeField]
	private SaltbakerLevelBGSaltHands saltHands;
	[SerializeField]
	private SpriteRenderer shadow;
	[SerializeField]
	private SpriteRenderer pauseShadow;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private CollisionChild[] collisionKids;
	[SerializeField]
	private Animator landFXAnimator;
	[SerializeField]
	private Collider2D[] colliders;
}
