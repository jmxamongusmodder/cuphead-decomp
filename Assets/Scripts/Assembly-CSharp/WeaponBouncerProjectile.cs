using UnityEngine;

public class WeaponBouncerProjectile : AbstractProjectile
{
	[SerializeField]
	private bool isEx;
	[SerializeField]
	private WeaponArcProjectileExplosion exExplosion;
	[SerializeField]
	private Effect trailFxPrefab;
	[SerializeField]
	private float trailFxMaxOffset;
	[SerializeField]
	private float trailDelay;
	public float gravity;
	public Vector2 velocity;
	public WeaponBouncer weapon;
	public float bounceRatio;
	public float bounceSpeedDampening;
}
