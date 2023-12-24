using UnityEngine;

public class AirplaneLevelBulldogCatAttack : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private AirplaneLevelBulldogPlane main;
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Transform root;
}
