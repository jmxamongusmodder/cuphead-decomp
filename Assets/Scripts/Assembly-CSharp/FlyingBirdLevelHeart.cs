using UnityEngine;

public class FlyingBirdLevelHeart : AbstractCollidableObject
{
	[SerializeField]
	private Effect puffFX;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	[SerializeField]
	private SpriteRenderer[] renderers;
}
