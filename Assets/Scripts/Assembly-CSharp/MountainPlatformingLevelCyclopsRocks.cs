using UnityEngine;

public class MountainPlatformingLevelCyclopsRocks : AbstractPausableComponent
{
	[SerializeField]
	private float walkSpeed;
	[SerializeField]
	private MinMax attackDelayRange;
	[SerializeField]
	private Transform onTrigger;
	[SerializeField]
	private Transform offTrigger;
	[SerializeField]
	private MountainPlatformingLevelCyclopsBG cyclopsBG;
	[SerializeField]
	private float cyclopsStopOffset;
}
