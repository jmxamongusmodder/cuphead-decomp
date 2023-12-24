using UnityEngine;

public class FlyingCowboyLevelBird : AbstractProjectile
{
	[SerializeField]
	private FlyingCowboyLevelBirdProjectile projectilePrefab;
	[SerializeField]
	private SpriteRenderer holdingFeetRenderer;
	[SerializeField]
	private SpriteRenderer emptyFeetRenderer;
	[SerializeField]
	private Transform projectileSpawnPoint;
}
