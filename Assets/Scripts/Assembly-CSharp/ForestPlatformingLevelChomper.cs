using UnityEngine;

public class ForestPlatformingLevelChomper : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private float speed;
	[SerializeField]
	private float gravityUp;
	[SerializeField]
	private float gravityDown;
	[SerializeField]
	private MinMax initialDelay;
	[SerializeField]
	private MinMax mainDelay;
}
