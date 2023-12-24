using UnityEngine;

public class FlyingCowboyLevelRicochetDebris : BasicUprightProjectile
{
	[SerializeField]
	private SpriteRenderer[] deathBits;
	[SerializeField]
	private BasicProjectile[] regularProjectiles;
	[SerializeField]
	private BasicProjectile[] parryableProjectiles;
	[SerializeField]
	private Transform shadowTransform;
	[SerializeField]
	private Effect deathEffect;
}
