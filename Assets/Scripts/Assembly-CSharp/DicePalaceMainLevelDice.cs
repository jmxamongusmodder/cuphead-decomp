public class DicePalaceMainLevelDice : ParrySwitch
{
	public enum Roll
	{
		One = 0,
		Two = 1,
		Three = 2,
	}

	public Roll roll;
	public bool waitingToRoll;
}
