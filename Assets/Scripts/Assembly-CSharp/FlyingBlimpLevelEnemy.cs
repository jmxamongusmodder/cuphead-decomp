using UnityEngine;

public class FlyingBlimpLevelEnemy : AbstractCollidableObject
{
	[SerializeField]
	private Effect bulletEffect;
	[SerializeField]
	private FlyingBlimpLevelEnemyDeathPart[] deathPieces;
	[SerializeField]
	private FlyingBlimpLevelEnemyProjectile projectilePrefab;
	[SerializeField]
	private FlyingBlimpLevelEnemyProjectile parryablePrefab;
	[SerializeField]
	private Transform projectileRoot;
}
