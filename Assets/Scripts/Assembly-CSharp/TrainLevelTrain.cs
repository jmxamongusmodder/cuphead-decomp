using UnityEngine;

public class TrainLevelTrain : LevelProperties.Train.Entity
{
	[SerializeField]
	private TrainLevelSkeleton skeleton;
	[SerializeField]
	private TrainLevelPassengerCar[] skeletonCars;
	[SerializeField]
	private TrainLevelLollipopGhoulsManager ghouls;
	[SerializeField]
	private TrainLevelEngineCar engineCar;
	[SerializeField]
	private TrainLevelEngineBoss engineBoss;
}
