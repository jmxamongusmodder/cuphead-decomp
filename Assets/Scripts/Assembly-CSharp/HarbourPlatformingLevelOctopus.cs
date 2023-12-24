using UnityEngine;

public class HarbourPlatformingLevelOctopus : PlatformingLevelAutoscrollObject
{
	[SerializeField]
	private Effect puff;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Transform tentacleFront;
	[SerializeField]
	private Transform tentacleBack;
	[SerializeField]
	private ParrySwitch anchor;
	[SerializeField]
	private HarbourPlatformingLevelOctoProjectile projectile;
	[SerializeField]
	private MinMax scrollMinMax;
	[SerializeField]
	private GameObject pinkGem;
	[SerializeField]
	private CollisionChild collisionChild;
	[SerializeField]
	private float accelerationTime;
	[SerializeField]
	private float holdSpeedTime;
	[SerializeField]
	private float deccelerationTime;
	[SerializeField]
	private float speedupMultiplier;
	[SerializeField]
	private float tuckDownDelay;
	[SerializeField]
	private float gemOffTime;
}
