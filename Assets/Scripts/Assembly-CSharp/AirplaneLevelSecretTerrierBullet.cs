using UnityEngine;

public class AirplaneLevelSecretTerrierBullet : AbstractProjectile
{
	[SerializeField]
	private CircleCollider2D coll;
	[SerializeField]
	private BasicProjectile splitBulletPrefab;
}
