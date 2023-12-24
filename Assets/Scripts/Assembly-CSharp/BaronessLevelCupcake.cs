using UnityEngine;

public class BaronessLevelCupcake : BaronessLevelMiniBossBase
{
	[SerializeField]
	private Effect splashPrefab;
	[SerializeField]
	private Transform launchOffset;
	[SerializeField]
	private BasicProjectile cupcakeProjectile;
	[SerializeField]
	private Transform collisionChild;
	[SerializeField]
	private Transform deathRoot;
}
