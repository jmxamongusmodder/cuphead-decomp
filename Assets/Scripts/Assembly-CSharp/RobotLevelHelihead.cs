using UnityEngine;

public class RobotLevelHelihead : AbstractCollidableObject
{
	[SerializeField]
	private float verticalMovementStrength;
	[SerializeField]
	private float horizontalMovementStrength;
	[SerializeField]
	private Transform spawnPoint;
	[SerializeField]
	private GameObject bombBotPrefab;
	[SerializeField]
	private RobotLevelBlockade blockadeSegement;
	[SerializeField]
	private RobotLevelGem gem;
}
