using UnityEngine;

public class PlaneWeaponChaliceBombProjectile : AbstractProjectile
{
	[SerializeField]
	private PlaneWeaponBombExplosion explosion;
	public float explosionSize;
	public float gravity;
	public float damageExplosion;
	public float size;
	public Vector2 velocity;
}
