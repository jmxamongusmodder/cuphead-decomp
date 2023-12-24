using UnityEngine;

public class DevilLevelSpiderHead : AbstractCollidableObject
{
	public enum State
	{
		Idle = 0,
		Attacking = 1,
	}

	public State state;
	[SerializeField]
	private float moveDistanceY;
}
