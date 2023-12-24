using UnityEngine;

public class CircusPlatformingLevelBallRunnerBall : AbstractCollidableObject
{
	public bool isMoving;
	[SerializeField]
	private float Speed;
	[SerializeField]
	private CircusPlatformingLevelBallRunner runner;
	public Vector3 direction;
}
