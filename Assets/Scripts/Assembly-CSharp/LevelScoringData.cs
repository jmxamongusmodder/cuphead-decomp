public class LevelScoringData
{
	public enum Grade
	{
		DMinus = 0,
		D = 1,
		DPlus = 2,
		CMinus = 3,
		C = 4,
		CPlus = 5,
		BMinus = 6,
		B = 7,
		BPlus = 8,
		AMinus = 9,
		A = 10,
		APlus = 11,
		S = 12,
		P = 13,
	}

	public float time;
	public float goalTime;
	public int finalHP;
	public int numTimesHit;
	public int numParries;
	public int superMeterUsed;
	public int coinsCollected;
	public Level.Mode difficulty;
	public bool pacifistRun;
	public bool useCoinsInsteadOfSuperMeter;
	public bool usedDjimmi;
	public bool player1IsChalice;
	public bool player2IsChalice;
}
