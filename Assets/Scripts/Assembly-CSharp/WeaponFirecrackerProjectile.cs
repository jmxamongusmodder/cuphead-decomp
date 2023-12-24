using UnityEngine;

public class WeaponFirecrackerProjectile : BasicProjectile
{
	[SerializeField]
	private WeaponFirecrackerProjectile projectile;
	public float bulletLife;
	public float explosionSize;
	public float explosionDuration;
	public float explosionRadiusSize;
	public float explosionAngle;
	public Collider2D collider;
	public new Animator animator;
}
