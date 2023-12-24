using UnityEngine;

public class RumRunnersLevelMobBoss : AbstractCollidableObject
{
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Effect projectileMuzzleFX;
	[SerializeField]
	private Vector2[] projectileRoots;
	[SerializeField]
	private Vector2 positionOffset;
	[SerializeField]
	private Vector2 flippedOffset;
}
