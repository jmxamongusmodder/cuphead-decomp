using UnityEngine;

public class MountainPlatformingLevelCyclopsBG : AbstractPausableComponent
{
	[SerializeField]
	private SpriteRenderer eye;
	[SerializeField]
	private float rockDelay;
	[SerializeField]
	private float rockSpeed;
	[SerializeField]
	private int rockCount;
	[SerializeField]
	private MountainPlatformingLevelRock projectile;
	public Vector3 start;
	public bool isDead;
}
