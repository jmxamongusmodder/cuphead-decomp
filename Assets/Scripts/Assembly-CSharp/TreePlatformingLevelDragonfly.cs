using UnityEngine;

public class TreePlatformingLevelDragonfly : PlatformingLevelBigEnemy
{
	[SerializeField]
	private LevelBossDeathExploder explosion;
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Transform projectileRoot;
	public GameObject platforms;
}
