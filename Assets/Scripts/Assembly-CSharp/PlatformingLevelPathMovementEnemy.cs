using UnityEngine;

public class PlatformingLevelPathMovementEnemy : AbstractPlatformingLevelEnemy
{
	public enum Direction
	{
		Forward = 1,
		Back = -1,
	}

	public float loopRepeatDelay;
	public float startPosition;
	public VectorPath path;
	[SerializeField]
	protected Direction _direction;
	[SerializeField]
	private bool _hasTurnAnimation;
	[SerializeField]
	private bool _hasFacingDirection;
	[SerializeField]
	private EaseUtils.EaseType _easeType;
}
