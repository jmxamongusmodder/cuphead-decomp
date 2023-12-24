using UnityEngine;

public class WeaponCrackshotExProjectile : BasicProjectile
{
	[SerializeField]
	private WeaponCrackshotExProjectileChild childPrefab;
	[SerializeField]
	private Effect shootFXPrefab;
	[SerializeField]
	private Effect launchFXPrefab;
	[SerializeField]
	private Collider2D coll;
}
