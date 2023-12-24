using UnityEngine;

public class PirateLevelDogFish : AbstractProjectile
{
	[SerializeField]
	private Collider2D secretHitBox;
	[SerializeField]
	private Collider2D normalHitBox;
	[SerializeField]
	private Effect splashEffect;
	[SerializeField]
	private Transform splashRoot;
	[SerializeField]
	private Effect deathEffect;
}
