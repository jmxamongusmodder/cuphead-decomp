using UnityEngine;

public class PirateLevelPirate : LevelProperties.Pirate.Entity
{
	[SerializeField]
	private Transform gunRoot;
	[SerializeField]
	private BasicProjectile gunProjectile;
	[SerializeField]
	private BasicProjectile gunProjectileRegular;
	[SerializeField]
	private Effect muzzleFlash;
	[SerializeField]
	private Transform whistleRoot;
	[SerializeField]
	private Effect whistleEffect;
}
