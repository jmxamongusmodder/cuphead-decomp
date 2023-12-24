using UnityEngine;

public class CircusPlatformingLevelPoleBot : AbstractPlatformingLevelEnemy
{
	public bool isDying;
	public bool isSliding;
	[SerializeField]
	private float fallDelay;
	[SerializeField]
	private float deadSpin;
	[SerializeField]
	private MinMax minVelocity;
	[SerializeField]
	private MinMax maxVelocity;
}
