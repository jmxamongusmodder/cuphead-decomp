public class BatLevelPinkCore : ParrySwitch
{
	public enum SideHit
	{
		Top = 0,
		Bottom = 1,
		Left = 2,
		Right = 3,
		None = 4,
	}

	public SideHit lastSideHit;
}
