using UnityEngine;

public class ChessRookLevelRegularCannonball : AbstractProjectile
{
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private SpriteRenderer shadow;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private float maxShadowDistance;
}
