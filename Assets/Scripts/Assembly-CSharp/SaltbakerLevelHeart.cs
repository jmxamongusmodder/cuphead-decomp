using UnityEngine;

public class SaltbakerLevelHeart : AbstractProjectile
{
	public enum LastHit
	{
		None = 0,
		Left = 1,
		Right = 2,
		Up = 3,
		Down = 4,
	}

	[SerializeField]
	private SpriteRenderer pinkSprite;
	[SerializeField]
	private SpriteRenderer regularSprite;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private Animator impactFX;
	[SerializeField]
	private Effect turnFX;
	public LastHit lastHit;
}
