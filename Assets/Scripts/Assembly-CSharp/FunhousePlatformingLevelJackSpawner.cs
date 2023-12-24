using UnityEngine;

public class FunhousePlatformingLevelJackSpawner : AbstractPausableComponent
{
	[SerializeField]
	private MinMax initialSpawnTime;
	[SerializeField]
	private MinMax spawnTime;
	[SerializeField]
	private float xRange;
	[SerializeField]
	private FunhousePlatformingLevelJack jackPrefab;
	[SerializeField]
	private bool isBottom;
}
