using UnityEngine;

public class TrainLevelBlindSpecter : LevelProperties.Train.Entity
{
	[SerializeField]
	private Transform eyeRoot;
	[SerializeField]
	private TrainLevelBlindSpecterEyeProjectile eyePrefab;
}
