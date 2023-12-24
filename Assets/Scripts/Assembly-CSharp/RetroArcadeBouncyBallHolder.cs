using UnityEngine;

public class RetroArcadeBouncyBallHolder : RetroArcadeEnemy
{
	[SerializeField]
	private Transform[] ballPositions;
	[SerializeField]
	private RetroArcadeBouncyBall typeABall;
	[SerializeField]
	private RetroArcadeBouncyBall typeBBall;
	[SerializeField]
	private RetroArcadeBouncyBall typeCBall;
}
