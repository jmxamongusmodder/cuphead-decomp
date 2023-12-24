using UnityEngine;

public class MountainPlatformingLevelDragonProjectile : BasicProjectile
{
	[SerializeField]
	private Effect trailPrefab;
	[SerializeField]
	private MinMax trailPeriod;
	[SerializeField]
	private float trailMaxOffset;
	[SerializeField]
	private Transform trailRoot;
}
