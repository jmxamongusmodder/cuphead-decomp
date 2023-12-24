using UnityEngine;

public class PlatformingLevelGenericExplosion : Effect
{
	[SerializeField]
	private Effect lightningPrefab;
	[SerializeField]
	private Effect starsPrefab;
	[SerializeField]
	private float lightningOnlyChance;
	[SerializeField]
	private float starOnlyChance;
	[SerializeField]
	private float starsPlusLightningChance;
}
