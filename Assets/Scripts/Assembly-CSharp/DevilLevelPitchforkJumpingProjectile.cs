public class DevilLevelPitchforkJumpingProjectile : AbstractProjectile
{
	public enum State
	{
		Idle = 0,
		Jumping = 1,
		OnGround = 2,
	}

	public State state;
}
