using UnityEngine;

public class FlowerLevelPlatform : LevelPlatform
{
	public enum State
	{
		Up = 0,
		Down = 1,
		PlayerOn = 2,
	}

	public float YPositionUp;
	[SerializeField]
	private State state;
	[SerializeField]
	private Transform shadow;
}
