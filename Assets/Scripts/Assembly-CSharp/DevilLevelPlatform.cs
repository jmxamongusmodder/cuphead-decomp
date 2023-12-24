public class DevilLevelPlatform : AbstractPausableComponent
{
	public enum PlatformType
	{
		A = 0,
		B = 1,
		C = 2,
		D = 3,
		E = 4,
	}

	public enum State
	{
		Idle = 0,
		Raising = 1,
		Lowering = 2,
		Dead = 3,
	}

	public PlatformType type;
	public State state;
}
