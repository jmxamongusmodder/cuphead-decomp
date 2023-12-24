using UnityEngine;

public class PlaneWeaponPeashotExProjectile : AbstractProjectile
{
	public float MaxSpeed;
	public float Acceleration;
	public float FreezeTime;
	[SerializeField]
	private Effect chompFxPrefab;
	[SerializeField]
	private Transform chompFxRoot;
	[SerializeField]
	private SpriteRenderer Cuphead;
	[SerializeField]
	private SpriteRenderer Mugman;
	public float speed;
}
