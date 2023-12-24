using UnityEngine;

public class TrainLevelLollipopGhoulsManager : LevelProperties.Train.Entity
{
	[SerializeField]
	private TrainLevelLollipopGhoul ghoulLeft;
	[SerializeField]
	private TrainLevelLollipopGhoul ghoulRight;
	[SerializeField]
	private TrainLevelGhostCannons cannons;
	[SerializeField]
	private TrainLevelPassengerCar[] cars;
}
