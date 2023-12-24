using UnityEngine;

public class BeeLevelQueenTriangle : AbstractProjectile
{
	[SerializeField]
	private bool isInvincible;
	[SerializeField]
	private Transform[] roots;
	[SerializeField]
	private BasicDamagableProjectile childPrefab;
	[SerializeField]
	private BasicProjectile childPrefabInvincible;
}
