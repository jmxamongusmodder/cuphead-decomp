using UnityEngine;

public class ChessKnightLevelKnightDeathArmor : AbstractPausableComponent
{
	public enum Type
	{
		Helmet = 0,
		Shield = 1,
		Sword = 2,
	}

	[SerializeField]
	private Type type;
	[SerializeField]
	private float growthSpeed;
}
