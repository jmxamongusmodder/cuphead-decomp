using UnityEngine;

public class RetroArcadeEnemy : AbstractProjectile
{
	public enum Type
	{
		A = 0,
		B = 1,
		C = 2,
		None = 3,
		IsBoss = 4,
	}

	[SerializeField]
	public Type type;
}
