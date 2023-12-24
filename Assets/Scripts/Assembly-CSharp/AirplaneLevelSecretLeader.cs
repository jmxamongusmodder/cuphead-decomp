using UnityEngine;

public class AirplaneLevelSecretLeader : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private BasicProjectile rocketBGPrefab;
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;
	[SerializeField]
	private Effect rocketBGEffect;
	[SerializeField]
	private AirplaneLevel level;
	[SerializeField]
	private AirplaneLevelSecretTerrier[] terriers;
	[SerializeField]
	private int currentHole;
	[SerializeField]
	private BoxCollider2D boxCollider;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private SpriteRenderer backerRend;
}
