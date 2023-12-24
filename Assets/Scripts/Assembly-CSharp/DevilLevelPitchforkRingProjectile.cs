public class DevilLevelPitchforkRingProjectile : AbstractProjectile
{
	public enum State
	{
		Idle = 0,
		Attacking = 1,
		OnGround = 2,
	}

	public State state;
}
