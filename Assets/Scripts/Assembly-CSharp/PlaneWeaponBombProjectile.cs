using UnityEngine;

public class PlaneWeaponBombProjectile : AbstractProjectile
{
	[SerializeField]
	private PlaneWeaponBombExplosion explosion;
	public bool shootsUp;
	public float explosionSize;
	public float bulletSize;
	public float gravity;
	public Vector2 velocity;
}
