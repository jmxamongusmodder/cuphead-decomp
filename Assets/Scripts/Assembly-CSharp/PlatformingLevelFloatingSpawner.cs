using UnityEngine;

public class PlatformingLevelFloatingSpawner : AbstractPausableComponent
{
	[SerializeField]
	private PlatformingLevelGroundMovementEnemy enemyPrefab;
	[SerializeField]
	private float xRange;
	[SerializeField]
	private MinMax initialSpawnTime;
	[SerializeField]
	private MinMax spawnTime;
}
