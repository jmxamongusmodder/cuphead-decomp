using UnityEngine;

public class OldManLevelTurretProjectile : BasicProjectile
{
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private float sparkleSpawnDelay;
	[SerializeField]
	private MinMax sparkleAngleShiftRange;
	[SerializeField]
	private MinMax sparkleDistanceRange;
}
