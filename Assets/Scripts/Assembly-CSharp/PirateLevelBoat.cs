using UnityEngine;

public class PirateLevelBoat : LevelProperties.Pirate.Entity
{
	[SerializeField]
	private DamageReceiver damageReceiver;
	[SerializeField]
	private Transform cannonRoot;
	[SerializeField]
	private BasicProjectile cannonProjectile;
	[SerializeField]
	private Effect cannonSmokePrefab;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Transform beamRoot;
	[SerializeField]
	private PirateLevelBoatProjectile projectilePrefab;
	[SerializeField]
	private PirateLevelBoatBeam beamPrefab;
	[SerializeField]
	private SpriteRenderer ully;
}
