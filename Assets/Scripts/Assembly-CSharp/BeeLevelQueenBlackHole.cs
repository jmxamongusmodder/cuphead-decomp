using UnityEngine;

public class BeeLevelQueenBlackHole : AbstractProjectile
{
	public float health;
	public float speed;
	public float childDelay;
	public float childSpeed;
	[SerializeField]
	private BasicProjectile childPrefab;
}
