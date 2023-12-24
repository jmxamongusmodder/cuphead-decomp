using UnityEngine;

public class VeggiesLevelCarrot : LevelProperties.Veggies.Entity
{
	[SerializeField]
	private AudioSource mindLoopPrefab;
	[SerializeField]
	private Transform homingRoot;
	[SerializeField]
	private Transform straightRoot;
	[SerializeField]
	private VeggiesLevelCarrotHomingProjectile homingPrefab;
	[SerializeField]
	private VeggiesLevelCarrotRegularProjectile straightPrefab;
	[SerializeField]
	private BasicProjectile ringPrefab;
	[SerializeField]
	private Effect ringEffectPrefab;
	[SerializeField]
	private VeggiesLevelCarrotBgCarrot bgPrefab;
	[SerializeField]
	private Effect spark;
}
