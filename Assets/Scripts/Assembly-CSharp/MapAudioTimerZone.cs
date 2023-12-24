using UnityEngine;

public class MapAudioTimerZone : AbstractCollidableObject
{
	[SerializeField]
	private string audioKey;
	[SerializeField]
	private Rangef audioDelayRange;
}
