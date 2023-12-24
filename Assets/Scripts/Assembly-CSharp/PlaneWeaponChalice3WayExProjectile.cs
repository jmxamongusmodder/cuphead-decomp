using UnityEngine;

public class PlaneWeaponChalice3WayExProjectile : AbstractProjectile
{
	public float FreezeTime;
	public float arcSpeed;
	public float arcX;
	public float arcY;
	public float damageAfterLaunch;
	public float speedAfterLaunch;
	public float accelAfterLaunch;
	public float minXDistance;
	public float xDistanceNoTarget;
	public int ID;
	public PlaneWeaponChalice3WayExProjectile partner;
	public float pauseTime;
	public float vDirection;
	[SerializeField]
	private SpriteRenderer magnet;
	[SerializeField]
	private SpriteRenderer deathSpark;
	[SerializeField]
	private Effect shootFX;
	[SerializeField]
	private Effect smokeFX;
	[SerializeField]
	private Effect sparkleFX;
	[SerializeField]
	private float firstSmokeDelay;
	[SerializeField]
	private float smokeDelay;
	[SerializeField]
	private float sparkleDelay;
	[SerializeField]
	private float sparkleRadius;
}
