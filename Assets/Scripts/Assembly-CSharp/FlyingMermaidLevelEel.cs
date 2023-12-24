using UnityEngine;

public class FlyingMermaidLevelEel : AbstractCollidableObject
{
	[SerializeField]
	private float riseTime;
	[SerializeField]
	private float riseDistance;
	[SerializeField]
	private float leaveTime;
	[SerializeField]
	private MinMax segmentY;
	[SerializeField]
	private int numSegments;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private FlyingMermaidLevelEelSegment headSegmentPrefab;
	[SerializeField]
	private FlyingMermaidLevelEelSegment[] bodySegmentPrefabs;
}
