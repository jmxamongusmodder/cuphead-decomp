using UnityEngine;

public class MapCastleZones : MonoBehaviour
{
	public enum Zone
	{
		None = 0,
		OldMan = 1,
		RumRunners = 2,
		Cowgirl = 3,
		DogFight = 4,
		SnowCult = 5,
		Dock = 6,
	}

	[SerializeField]
	private MapCastleZoneCollider[] zones;
	[SerializeField]
	private MapLevelLoaderLadder ladder;
}
