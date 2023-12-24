using System;
using UnityEngine;

public class BeeLevel : Level
{
	[Serializable]
	public class Prefabs
	{
		public BeeLevelGrunt grunt;
		public BeeLevelHoneyDrip drip;
	}

	[SerializeField]
	private Vector2 p2ChaliceSpawnPoint;
	[SerializeField]
	private BeeLevelAirplane airplane;
	[SerializeField]
	private BeeLevelQueen queen;
	[SerializeField]
	private BeeLevelSecurityGuard guard;
	[SerializeField]
	private Transform[] gruntRoots;
	[SerializeField]
	private BeeLevelBackground background;
	[SerializeField]
	private Prefabs prefabs;
	[SerializeField]
	private Sprite _bossPortraitGuard;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitAirplane;
	[SerializeField]
	private string _bossQuoteGuard;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteAirplane;
}
