using UnityEngine;

public class MountainPlatformingLevelMudmanSpawner : AbstractPausableComponent
{
	[SerializeField]
	private Transform[] spawnPoints;
	[SerializeField]
	private MountainPlatformingLevelMudman bigMudman;
	[SerializeField]
	private MountainPlatformingLevelMudman smallMudman;
	[SerializeField]
	private MinMax spawnDelayRange;
	[SerializeField]
	private MinMax initialDelayRange;
	[SerializeField]
	private string mudmanSizeString;
	[SerializeField]
	private string mudmanBigSpawnString;
	[SerializeField]
	private string mudmanSmallSpawnString;
}
