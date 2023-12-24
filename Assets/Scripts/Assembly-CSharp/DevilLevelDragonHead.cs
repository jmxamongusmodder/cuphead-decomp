using UnityEngine;

public class DevilLevelDragonHead : AbstractCollidableObject
{
	public enum State
	{
		Idle = 0,
		Moving = 1,
		Stopped = 2,
	}

	[SerializeField]
	private float speed;
	[SerializeField]
	private Transform leftRoot;
	[SerializeField]
	private Transform rightRoot;
	[SerializeField]
	private Transform children;
	public State state;
}
