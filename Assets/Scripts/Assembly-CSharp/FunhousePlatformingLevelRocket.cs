using UnityEngine;

public class FunhousePlatformingLevelRocket : PlatformingLevelGroundMovementEnemy
{
	[SerializeField]
	private Transform sprite;
	[SerializeField]
	private FunhousePlatformingLevelExplosionFX explosion;
	[SerializeField]
	private float distToLaunch;
	[SerializeField]
	private float launchSpeed;
	[SerializeField]
	private DamageReceiver collisionDamageReceiver;
}
