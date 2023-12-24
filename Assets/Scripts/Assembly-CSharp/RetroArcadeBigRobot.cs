using UnityEngine;

public class RetroArcadeBigRobot : RetroArcadeEnemy
{
	[SerializeField]
	private RetroArcadeOrbiterRobot[] orbiterPrefabs;
	[SerializeField]
	private RetroArcadeRobotBouncingProjectile projectilePrefab;
	[SerializeField]
	private Transform projectileRoot;
}
