using UnityEngine;

public class DicePalaceMainLevelGameManager : LevelProperties.DicePalaceMain.Entity
{
	public enum BoardSpaces
	{
		Booze = 0,
		Chips = 1,
		Cigar = 2,
		Domino = 3,
		EightBall = 4,
		FlyingHorse = 5,
		FlyingMemory = 6,
		Pachinko = 7,
		Rabbit = 8,
		Roulette = 9,
		FreeSpace = 10,
		StartOver = 11,
	}

	[SerializeField]
	private BoardSpaces[] allBoardSpaces;
	[SerializeField]
	private DicePalaceMainLevelBoardSpace[] boardSpacesObj;
	[SerializeField]
	private DicePalaceMainLevelBoardSpace startSpaceObj;
	[SerializeField]
	private DicePalaceMainLevelBoardSpace endSpaceObj;
	[SerializeField]
	private DicePalaceMainLevelKingDice kingDice;
	[SerializeField]
	private DicePalaceMainLevelDice dicePrefab;
	[SerializeField]
	private Transform pivotPoint1;
	[SerializeField]
	private Transform marker;
	[SerializeField]
	private Animator markerAnimator;
	[SerializeField]
	private float endSpaceFlashRate;
	[SerializeField]
	private GameObject heart;
}
