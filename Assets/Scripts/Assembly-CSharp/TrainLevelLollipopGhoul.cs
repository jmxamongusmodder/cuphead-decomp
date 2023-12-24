using UnityEngine;

public class TrainLevelLollipopGhoul : LevelProperties.Train.Entity
{
	[SerializeField]
	private Transform head;
	[SerializeField]
	private Transform lightningRoot;
	[SerializeField]
	private TrainLevelLollipopGhoulLightning lightningPrefab;
}
