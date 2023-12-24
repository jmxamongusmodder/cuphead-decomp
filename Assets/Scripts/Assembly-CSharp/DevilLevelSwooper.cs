using UnityEngine;

public class DevilLevelSwooper : AbstractCollidableObject
{
	public enum State
	{
		Intro = 0,
		Idle = 1,
		Swooping = 2,
		Returning = 3,
		Dying = 4,
	}

	[SerializeField]
	private Effect[] explosions;
	public State state;
	public bool finalSwooping;
}
