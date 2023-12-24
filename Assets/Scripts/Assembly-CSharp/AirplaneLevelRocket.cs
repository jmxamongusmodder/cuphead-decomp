using UnityEngine;

public class AirplaneLevelRocket : HomingProjectile
{
	[SerializeField]
	private Transform effectRoot;
	[SerializeField]
	private Effect effectFX;
	[SerializeField]
	private Effect deathFX;
	[SerializeField]
	private Effect deathOnPlaneFX;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private MinMax fxSpawnRate;
}
