using UnityEngine;

public class MountainPlatformingLevelElevatorHandler : AbstractPausableComponent
{
	[SerializeField]
	private MountainPlatformingLevelMudmanSpawner mudmanSpawner;
	[SerializeField]
	private Transform topBackground;
	[SerializeField]
	private Transform bottomBackground;
	[SerializeField]
	private float speed;
	[SerializeField]
	private GameObject scrollingObject;
	[SerializeField]
	private Transform triggerPoint;
	[SerializeField]
	private Transform triggerPoint2;
	[SerializeField]
	private Transform pointA;
	[SerializeField]
	private GameObject invisibleWall;
	[SerializeField]
	private ScrollingBackgroundElevator cloudSprite;
	[SerializeField]
	private ScrollingBackgroundElevator foregroundSprite;
	[SerializeField]
	private ScrollingBackgroundElevator backgroundSprite;
	[SerializeField]
	private ScrollingBackgroundElevator[] midgroundSprites;
	[SerializeField]
	private float time;
}
