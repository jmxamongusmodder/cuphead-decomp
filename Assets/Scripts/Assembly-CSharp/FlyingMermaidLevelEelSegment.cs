using UnityEngine;

public class FlyingMermaidLevelEelSegment : AbstractPausableComponent
{
	[SerializeField]
	private float gravity;
	[SerializeField]
	private MinMax angleRange;
	[SerializeField]
	private float launchSpeed;
	[SerializeField]
	private float despawnY;
}
