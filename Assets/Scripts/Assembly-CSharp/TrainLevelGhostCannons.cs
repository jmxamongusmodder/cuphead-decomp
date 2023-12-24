using UnityEngine;

public class TrainLevelGhostCannons : LevelProperties.Train.Entity
{
	[SerializeField]
	private Effect cannonSmoke;
	[SerializeField]
	private Transform[] cannonRoots;
	[SerializeField]
	private TrainLevelGhostCannonGhost ghostPrefab;
}
