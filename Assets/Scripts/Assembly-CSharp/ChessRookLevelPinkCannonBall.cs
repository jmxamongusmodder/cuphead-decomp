using UnityEngine;

public class ChessRookLevelPinkCannonBall : AbstractProjectile
{
	[SerializeField]
	private Collider2D parryColl;
	[SerializeField]
	private SpriteRenderer shadow;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private SpriteRenderer topExplosion;
	[SerializeField]
	private SpriteRenderer rotatingExplosion;
	[SerializeField]
	private SpriteRenderer bigExplosion;
	[SerializeField]
	private Effect sinkFX;
	[SerializeField]
	private float maxShadowDistance;
}
