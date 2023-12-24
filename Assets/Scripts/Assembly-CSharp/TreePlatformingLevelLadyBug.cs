public class TreePlatformingLevelLadyBug : PlatformingLevelGroundMovementEnemy
{
	public enum Type
	{
		GroundFast = 0,
		GroundSlow = 1,
		BounceFast = 2,
		BounceSlow = 3,
		BouncePink = 4,
	}

	public Type type;
}
