using UnityEngine;

public class BaronessLevelCastle : LevelProperties.Baroness.Entity
{
	public enum BossPossibility
	{
		Gumball = 1,
		Waffle = 2,
		CandyCorn = 3,
		Cupcake = 4,
		Jawbreaker = 5,
	}

	public bool pauseScrolling;
	[SerializeField]
	private SpriteRenderer teeth;
	[SerializeField]
	private SpriteRenderer blink;
	[SerializeField]
	private BaronessLevelPlatform platform;
	[SerializeField]
	private BaronessLevelBaroness baronessPhase1;
	[SerializeField]
	private Transform baronessPhase2;
	[SerializeField]
	private BaronessLevelPeppermint peppermintPrefab;
	[SerializeField]
	private Transform blackCastleHole;
	[SerializeField]
	private Transform castlePhase2TopLayer;
	[SerializeField]
	private BaronessLevelCupcake cupcakePrefab;
	[SerializeField]
	private BaronessLevelWaffle wafflePrefab;
	[SerializeField]
	private BaronessLevelGumball gumballPrefab;
	[SerializeField]
	private BaronessLevelJawbreaker jawBreakerPrefab;
	[SerializeField]
	private BaronessLevelCandyCorn candyCornPrefab;
	[SerializeField]
	private BaronessLevelJellybeans greenJellyPrefab;
	[SerializeField]
	private BaronessLevelJellybeans pinkJellyPrefab;
	[SerializeField]
	private Transform emergePoint;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private GameObject castleCollidePhase2;
	[SerializeField]
	private SpriteRenderer castleWallFix;
}
