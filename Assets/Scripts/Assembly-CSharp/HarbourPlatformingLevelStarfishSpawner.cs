using UnityEngine;

public class HarbourPlatformingLevelStarfishSpawner : AbstractPausableComponent
{
	[SerializeField]
	private MinMax initialSpawnTime;
	[SerializeField]
	private MinMax spawnTime;
	[SerializeField]
	private HarbourPlatformingLevelStarfish starfishPrefab;
	[SerializeField]
	private string speedXString;
	[SerializeField]
	private string typeString;
	[SerializeField]
	private MinMax speedYRange;
	[SerializeField]
	private float xRange;
	[SerializeField]
	private float loopSize;
}
