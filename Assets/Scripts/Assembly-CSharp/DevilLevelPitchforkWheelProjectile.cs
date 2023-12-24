public class DevilLevelPitchforkWheelProjectile : AbstractProjectile
{
	public enum State
	{
		Idle = 0,
		Attacking = 1,
		Returning = 2,
	}

	public State state;
}
