using UnityEngine;

public class PlatformingLevelGroundMovementEnemy : AbstractPlatformingLevelEnemy
{
	public enum Direction
	{
		Right = 1,
		Left = -1,
	}

	public float startPosition;
	[SerializeField]
	protected Direction _direction;
	[SerializeField]
	private bool hasJumpAnimation;
	[SerializeField]
	private bool hasTurnAnimation;
	[SerializeField]
	private bool canSpawnOnPlatforms;
	[SerializeField]
	private float turnaroundDistance;
	[SerializeField]
	private Transform shadow;
	[SerializeField]
	private float maxShadowDistance;
	[SerializeField]
	private Effect jumpLandEffectPrefab;
	[SerializeField]
	protected bool noTurn;
	[SerializeField]
	protected bool lockDirectionWhenLanding;
	[SerializeField]
	protected bool gravityReversed;
}
