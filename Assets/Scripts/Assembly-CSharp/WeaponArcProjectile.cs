using UnityEngine;

public class WeaponArcProjectile : AbstractProjectile
{
	[SerializeField]
	private bool isEx;
	[SerializeField]
	private WeaponArcProjectileExplosion exExplosion;
	public float chargeTime;
	public float gravity;
	public Vector2 velocity;
	public WeaponArc weapon;
}
