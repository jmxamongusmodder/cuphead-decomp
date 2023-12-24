using UnityEngine;

public class DicePalaceCardGameManager : AbstractPausableComponent
{
	[SerializeField]
	private DicePalaceCardLevelColumn columnObject;
	[SerializeField]
	private DicePalaceCardLevelBlock hearts;
	[SerializeField]
	private DicePalaceCardLevelBlock spades;
	[SerializeField]
	private DicePalaceCardLevelBlock clubs;
	[SerializeField]
	private DicePalaceCardLevelBlock diamonds;
	[SerializeField]
	private DicePalaceCardLevelGridBlock gridBlockPrefab;
	public int GridDimX;
	public int GridDimY;
}
