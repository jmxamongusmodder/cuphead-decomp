using UnityEngine;

public class CircusPlatformingLevelPretzel : AbstractPlatformingLevelEnemy
{
	public bool goingLeft;
	[SerializeField]
	private float jumpMultiplierX;
	[SerializeField]
	private float jumpMultiplierY;
	[SerializeField]
	private float inverseJumpMultiplierY;
	[SerializeField]
	private Transform transformDustA;
	[SerializeField]
	private Transform transformDustB;
}
