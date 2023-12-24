using UnityEngine;

public class PlaneWeaponChaliceBombExProjectile : AbstractProjectile
{
	public float MaxSpeed;
	public float Acceleration;
	public float FreezeTime;
	[SerializeField]
	private Effect chompFxPrefab;
	[SerializeField]
	private Transform chompFxRoot;
	public Vector3 Velocity;
	public float Gravity;
	public float DamageRateIncrease;
	public float speed;
}
