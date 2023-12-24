using UnityEngine;

public class DevilLevelPitchforkBouncingProjectile : AbstractProjectile
{
	public enum State
	{
		Idle = 0,
		Attacking = 1,
	}

	public State state;
	[SerializeField]
	private Effect blueSparkle;
	[SerializeField]
	private Effect pinkSparkle;
	[SerializeField]
	private Effect bounceEffect;
	[SerializeField]
	private Effect bounceEffectPink;
}
