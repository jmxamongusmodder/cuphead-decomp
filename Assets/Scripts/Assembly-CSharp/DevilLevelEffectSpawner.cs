using UnityEngine;

public class DevilLevelEffectSpawner : AbstractPausableComponent
{
	[SerializeField]
	private bool isSmoke3;
	public MinMax waitTime;
	public Effect effectPrefab;
}
