using UnityEngine;

public class WeaponPeashotExProjectile : AbstractProjectile
{
	[SerializeField]
	private Effect hitFXPrefab;
	[SerializeField]
	private Transform hitFxRoot;
	public float moveSpeed;
	public float hitFreezeTime;
	public float maxDamage;
}
