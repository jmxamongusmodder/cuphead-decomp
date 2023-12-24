using UnityEngine;

public class FlyingMermaidLevelSeahorse : AbstractCollidableObject
{
	[SerializeField]
	private float spawnY;
	[SerializeField]
	private float riseTime;
	[SerializeField]
	private float riseDistance;
	[SerializeField]
	private float deathStayTime;
	[SerializeField]
	private float deathMoveTime;
	[SerializeField]
	private float deathMoveDistance;
	[SerializeField]
	private FlyingMermaidLevelSeahorseSpray spray;
	[SerializeField]
	private Transform deathFxRoot;
	[SerializeField]
	private Transform deathFxPrefab;
}
