using System;
using UnityEngine;

public class FlyingBirdLevel : Level
{
	[Serializable]
	public class Prefabs
	{
		public FlyingBirdLevelEnemy formationBird;
		public FlyingBirdLevelTurret turretBird;
	}

	[SerializeField]
	private FlyingBirdLevelBird bird;
	[SerializeField]
	private FlyingBirdLevelSmallBird smallBird;
	[SerializeField]
	private Transform enemyRoot;
	[SerializeField]
	private Transform turretRootTop;
	[SerializeField]
	private Transform turretRootBottom;
	[SerializeField]
	private Prefabs prefabs;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitHouseDeath;
	[SerializeField]
	private Sprite _bossPortraitBirdRevival;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteHouseDeath;
	[SerializeField]
	private string _bossQuoteBirdRevival;
}
