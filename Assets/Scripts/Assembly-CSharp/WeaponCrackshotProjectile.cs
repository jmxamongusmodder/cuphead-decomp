using UnityEngine;

public class WeaponCrackshotProjectile : BasicProjectile
{
	public float maxAngleRange;
	public int variant;
	public bool useBComet;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private Effect crackFX;
}
