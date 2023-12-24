using UnityEngine;

public class SaltbakerLevelPhaseOneProjectile : BasicProjectile
{
	[SerializeField]
	protected SpriteRenderer shadow;
	[SerializeField]
	protected MinMax shadowScaleHeightRange;
	[SerializeField]
	protected Effect sparkEffect;
	[SerializeField]
	private float sparkSpawnDelay;
	[SerializeField]
	private MinMax sparkAngleShiftRange;
	[SerializeField]
	private MinMax sparkDistanceRange;
}
