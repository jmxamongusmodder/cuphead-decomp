using UnityEngine;

public class MouseLevelGhostMouse : AbstractCollidableObject
{
	[SerializeField]
	private MouseLevelGhostMouseBall blueBallPrefab;
	[SerializeField]
	private MouseLevelGhostMouseBall pinkBallPrefab;
	[SerializeField]
	private Transform projectileRoot;
}
