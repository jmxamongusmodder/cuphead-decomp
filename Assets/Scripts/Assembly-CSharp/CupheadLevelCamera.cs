public class CupheadLevelCamera : AbstractCupheadGameCamera
{
	public enum Mode
	{
		Lerp = 0,
		TrapBox = 1,
		Relative = 2,
		Platforming = 3,
		Path = 4,
		RelativeRook = 5,
		RelativeRumRunners = 6,
		Static = 10000,
	}

	public bool enablePathScrubbing;
	public float scrub;
	public float LERP_SPEED;
}
