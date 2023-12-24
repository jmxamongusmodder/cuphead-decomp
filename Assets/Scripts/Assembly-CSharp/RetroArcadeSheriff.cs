using UnityEngine;

public class RetroArcadeSheriff : RetroArcadeEnemy
{
	public enum Side
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,
	}

	public float speed;
	[SerializeField]
	private BasicProjectile projectile;
	public Side side;
}
