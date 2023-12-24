using UnityEngine;

public class PlatformingLevelMovingPlatform : AbstractPausableComponent
{
	public float loopRepeatDelay;
	public float speed;
	public VectorPath path;
	public bool goingUp;
	public SpriteRenderer sprite;
}
