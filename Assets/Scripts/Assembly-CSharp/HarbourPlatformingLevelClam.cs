using UnityEngine;

public class HarbourPlatformingLevelClam : PlatformingLevelShootingEnemy
{
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private Transform main;
	[SerializeField]
	private Transform onTrigger;
	[SerializeField]
	private Transform offTrigger;
	[SerializeField]
	private HarbourPlatformingLevelOctopus octopus;
}
