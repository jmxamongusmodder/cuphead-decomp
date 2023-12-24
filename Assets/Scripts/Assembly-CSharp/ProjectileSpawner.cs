using UnityEngine;

public class ProjectileSpawner : AbstractPausableComponent
{
	public enum Type
	{
		Straight = 0,
		Aimed = 1,
	}

	public Type type;
	public float delay;
	public float speed;
	public bool parryable;
	public float stoneTime;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	public float angle;
}
