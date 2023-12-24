public class DamageReceiver : AbstractPausableComponent
{
	public enum Type
	{
		Enemy = 0,
		Player = 1,
		Other = 2,
	}

	public Type type;
	public AnimationHelper[] animatorsEffectedByPause;
}
