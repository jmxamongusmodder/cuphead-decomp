using UnityEngine;

public class MountainPlatformingLevelScale : AbstractPausableComponent
{
	public enum ScaleState
	{
		rightDown = 0,
		leftDown = 1,
		still = 2,
	}

	[SerializeField]
	private float scaleSpeed;
	[SerializeField]
	private float scaleChangeAmount;
	[SerializeField]
	private MountainPlatformingLevelScalePart ScaleLeft;
	[SerializeField]
	private MountainPlatformingLevelScalePart ScaleRight;
	public ScaleState scaleState;
}
