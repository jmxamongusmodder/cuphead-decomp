using UnityEngine;

public class TreePlatformingLevelMosquito : AbstractCollidableObject
{
	public enum Type
	{
		AA = 0,
		AB = 1,
		AC = 2,
		BA = 3,
		BB = 4,
		BC = 5,
		CA = 6,
		CB = 7,
		CC = 8,
	}

	public enum State
	{
		Up = 0,
		Down = 1,
		PlayerOn = 2,
	}

	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private float projectileSpeed;
	[SerializeField]
	private bool projectileShootsUP;
	[SerializeField]
	private float projectileShootUpTime;
	[SerializeField]
	private LevelPlatform platform;
	[SerializeField]
	private float reappearDelay;
	[SerializeField]
	private PlatformingLevelGenericExplosion explosion;
	public float returnTime;
	public Type type;
	public float YPositionUp;
	[SerializeField]
	private State state;
}
