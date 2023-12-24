using UnityEngine;

public class MountainPlatformingLevelMinerSpawner : AbstractPausableComponent
{
	[SerializeField]
	private PlatformingLevelGroundMovementEnemy enemyPrefab;
	[SerializeField]
	private float xRange;
	[SerializeField]
	private MinMax deathDelayTime;
	[SerializeField]
	private MinMax spawnTime;
}
