using UnityEngine;

public class AirplaneLevelSecretTerrier : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private Transform bulletRoot;
	[SerializeField]
	private AirplaneLevelSecretTerrierBullet bulletPrefab;
	[SerializeField]
	private AirplaneLevelSecretTerrierBullet bulletPrefabPink;
	[SerializeField]
	private AirplaneLevel level;
	[SerializeField]
	private int currentHole;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private AirplaneLevelSecretLeader leader;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private SpriteRenderer backerRend;
}
