using UnityEngine;

public class MountainPlatformingLevelPlatformsHandler : AbstractPausableComponent
{
	[SerializeField]
	private Transform platformHolder;
	[SerializeField]
	private ParrySwitch parrySwitch;
	[SerializeField]
	private float platformMoveTime;
	[SerializeField]
	private float platformAppearDelay;
}
