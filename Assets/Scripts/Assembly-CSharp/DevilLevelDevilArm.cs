using UnityEngine;

public class DevilLevelDevilArm : AbstractCollidableObject
{
	public enum State
	{
		Idle = 0,
		Attacking = 1,
	}

	public State state;
	[SerializeField]
	private Transform endPos;
	public bool isRight;
}
