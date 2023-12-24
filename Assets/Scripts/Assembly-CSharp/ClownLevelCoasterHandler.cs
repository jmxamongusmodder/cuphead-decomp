using UnityEngine;

public class ClownLevelCoasterHandler : LevelProperties.Clown.Entity
{
	public bool finalRun;
	public bool isRunning;
	[SerializeField]
	private ClownLevelClownSwing swing;
	[SerializeField]
	private ClownLevelLights warningLight;
	[SerializeField]
	private Transform frontTrackStart;
	[SerializeField]
	private Transform backTrackStart;
	[SerializeField]
	private ClownLevelCoasterPiece redCoaster;
	[SerializeField]
	private ClownLevelCoasterPiece blueCoaster;
	[SerializeField]
	private ClownLevelRiders ridersPrefab;
	[SerializeField]
	private GameObject tailPrefab;
	[SerializeField]
	private ClownLevelCoaster coasterPrefab;
}
