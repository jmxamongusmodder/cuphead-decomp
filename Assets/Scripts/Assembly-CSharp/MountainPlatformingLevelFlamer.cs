using UnityEngine;

public class MountainPlatformingLevelFlamer : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private float loopSize;
	[SerializeField]
	private MinMax startDelayRange;
	[SerializeField]
	private MinMax respawnRange;
}
