using UnityEngine;

public class VeggiesLevelOnion : LevelProperties.Veggies.Entity
{
	[SerializeField]
	private Transform leftRoot;
	[SerializeField]
	private Transform rightRoot;
	[SerializeField]
	private Transform radishRootRight;
	[SerializeField]
	private Transform radishRootLeft;
	[SerializeField]
	private VeggiesLevelOnionTearsStream tearStreamPrefab;
	[SerializeField]
	private VeggiesLevelOnionTearProjectile projectilePrefab;
	[SerializeField]
	private VeggiesLevelOnionTearProjectile pinkProjectilePrefab;
	[SerializeField]
	private VeggiesLevelOnionHomingHeart homingHeartPrefab;
}
