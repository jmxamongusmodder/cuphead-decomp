using UnityEngine;

public class BeeLevelAirplane : LevelProperties.Bee.Entity
{
	[SerializeField]
	private Transform rightShootRoot;
	[SerializeField]
	private Transform leftShootRoot;
	[SerializeField]
	private BeeLevelTurbineBullet bullet;
	[SerializeField]
	private SpriteRenderer topLayer;
	[SerializeField]
	private SpriteRenderer midLayer;
}
