using UnityEngine;

public class WeaponUpshotExProjectile : AbstractProjectile
{
	public float rotateDir;
	[SerializeField]
	private SpriteRenderer trail1;
	[SerializeField]
	private SpriteRenderer trail2;
	[SerializeField]
	private Effect hitFXPrefab;
}
