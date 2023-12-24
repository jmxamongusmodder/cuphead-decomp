using UnityEngine;

public class WeaponExploderProjectile : BasicProjectile
{
	[SerializeField]
	private WeaponExploderProjectileExplosion explosionPrefab;
	[SerializeField]
	private BasicProjectile shrapnelPrefab;
	[SerializeField]
	private bool isEx;
	public float explodeRadius;
	public float easeTime;
	public MinMax minMaxSpeed;
	public WeaponExploder weapon;
}
