using UnityEngine;

public class AirplaneLevelDropBullet : AbstractProjectile
{
	[SerializeField]
	private CircleCollider2D circColl;
	[SerializeField]
	private BoxCollider2D boxColl;
	[SerializeField]
	private Effect shootFX;
	[SerializeField]
	private SpriteRenderer speedLines;
	[SerializeField]
	private SpriteRenderer rend;
}
