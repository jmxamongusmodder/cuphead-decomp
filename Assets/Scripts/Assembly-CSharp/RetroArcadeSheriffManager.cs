using UnityEngine;

public class RetroArcadeSheriffManager : LevelProperties.RetroArcade.Entity
{
	[SerializeField]
	private Transform bottom;
	[SerializeField]
	private RetroArcadeSheriff sheriffGreenPrefab;
	[SerializeField]
	private RetroArcadeSheriff sheriffYellowPrefab;
	[SerializeField]
	private RetroArcadeSheriff sheriffOrangePrefab;
}
