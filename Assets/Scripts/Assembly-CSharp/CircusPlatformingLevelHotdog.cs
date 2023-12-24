using UnityEngine;

public class CircusPlatformingLevelHotdog : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private Transform[] projectilesSpawnPoints;
	[SerializeField]
	private string spawnPatternString;
	[SerializeField]
	private string condimentPatternString;
	[SerializeField]
	private string sidePatternString;
	[SerializeField]
	private string shotDelayPatternString;
	[SerializeField]
	private float projectileDistance;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	[SerializeField]
	private LevelBossDeathExploder exploder;
}
