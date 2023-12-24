using UnityEngine;

public class RetroArcadeTentacleManager : LevelProperties.RetroArcade.Entity
{
	[SerializeField]
	private GameObject octopusHead;
	[SerializeField]
	private RetroArcadeTentacle tentaclePrefab;
	[SerializeField]
	private Transform bottom;
}
