using UnityEngine;

public class AirshipStorkLevelStork : LevelProperties.AirshipStork.Entity
{
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Transform knobSprite;
	[SerializeField]
	private AirshipStorkLevelProjectile projectile;
	[SerializeField]
	private AirshipStorkLevelProjectile projectilePink;
	[SerializeField]
	private AirshipStorkLevelBaby babyPrefab;
}
