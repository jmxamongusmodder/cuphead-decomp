using UnityEngine;

public class FlyingMermaidLevelTurtle : AbstractCollidableObject
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
	private FlyingMermaidLevelTurtleCannonBall cannonBallPrefab;
	[SerializeField]
	private Transform cannonBallRoot;
	[SerializeField]
	private Transform shootEffectRoot;
	[SerializeField]
	private Effect shootEffectPrefab;
}
