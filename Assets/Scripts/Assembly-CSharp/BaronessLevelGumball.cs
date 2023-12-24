using UnityEngine;

public class BaronessLevelGumball : BaronessLevelMiniBossBase
{
	[SerializeField]
	private BaronessLevelGumballProjectile[] projectilePrefabs;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private SpriteRenderer lid;
	[SerializeField]
	private SpriteRenderer legs;
	[SerializeField]
	private CollisionChild headCollider;
	[SerializeField]
	private GameObject headSpark;
	[SerializeField]
	private Transform feetDust;
}
