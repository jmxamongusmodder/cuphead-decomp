using UnityEngine;

public class FlyingCowboyLevelBirdProjectile : BasicProjectile
{
	[SerializeField]
	private Transform shadowTransform;
	[SerializeField]
	private Transform spawnPoint;
	[SerializeField]
	private Transform smokeTransform;
	[SerializeField]
	private BasicProjectile shrapnelPrefab;
}
