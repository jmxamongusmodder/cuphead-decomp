using UnityEngine;

public class FlyingCowboyLevelBackshot : BasicUprightProjectile
{
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Transform projectileSpawnPosition;
	[SerializeField]
	private Transform[] leftWings;
	[SerializeField]
	private Transform[] rightWings;
}
