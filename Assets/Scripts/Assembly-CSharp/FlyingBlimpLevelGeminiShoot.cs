using UnityEngine;

public class FlyingBlimpLevelGeminiShoot : AbstractCollidableObject
{
	[SerializeField]
	private GameObject smallFX;
	[SerializeField]
	private Transform projectileRootUp;
	[SerializeField]
	private Transform projectileRootDown;
	[SerializeField]
	private BasicProjectile projectilePrefab;
}
