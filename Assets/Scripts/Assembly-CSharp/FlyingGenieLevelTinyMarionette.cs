using UnityEngine;

public class FlyingGenieLevelTinyMarionette : AbstractCollidableObject
{
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private BasicProjectile pinkProjectile;
	[SerializeField]
	private Effect shootFX;
	[SerializeField]
	private Transform shootRoot;
}
