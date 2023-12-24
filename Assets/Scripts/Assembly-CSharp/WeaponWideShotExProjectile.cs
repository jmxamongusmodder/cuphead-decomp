using UnityEngine;

public class WeaponWideShotExProjectile : AbstractProjectile
{
	public float mainDuration;
	public Vector3 origin;
	[SerializeField]
	private Effect hitsparkPrefab;
}
