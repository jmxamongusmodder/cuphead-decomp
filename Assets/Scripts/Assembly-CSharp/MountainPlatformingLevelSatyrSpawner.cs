using UnityEngine;

public class MountainPlatformingLevelSatyrSpawner : AbstractPausableComponent
{
	[SerializeField]
	private string directionString;
	[SerializeField]
	private string spawnString;
	[SerializeField]
	private float xRange;
	[SerializeField]
	private MountainPlatformingLevelSatyr satyrPrefab;
	[SerializeField]
	private MinMax spawnDelayRange;
}
