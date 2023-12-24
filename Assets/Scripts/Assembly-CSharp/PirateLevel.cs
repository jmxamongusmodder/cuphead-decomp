using System;
using UnityEngine;

public class PirateLevel : Level
{
	[Serializable]
	public class Prefabs
	{
		public PirateLevelSquid squid;
		public PirateLevelShark shark;
		public PirateLevelDogFish dogFish;
		public PirateLevelSquidInkOverlay inkOverlay;
	}

	public PirateLevelPirate pirate;
	public PirateLevelPirateDead deadPirate;
	public PirateLevelBoat boat;
	public PirateLevelBarrel barrel;
	public PirateLevelDogFishScope scope;
	public Transform[] boatParts;
	[SerializeField]
	private Prefabs prefabs;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitBoat;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteBoat;
}
