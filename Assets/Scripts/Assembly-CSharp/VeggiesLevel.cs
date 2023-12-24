using System;
using UnityEngine;

public class VeggiesLevel : Level
{
	[Serializable]
	public class Prefabs
	{
		public VeggiesLevelPotato potato;
		public VeggiesLevelOnion onion;
		public VeggiesLevelBeet beet;
		public VeggiesLevelPeas peas;
		public VeggiesLevelCarrot carrot;
	}

	[SerializeField]
	private Prefabs prefabs;
	[SerializeField]
	private Sprite _bossPortraitPotato;
	[SerializeField]
	private Sprite _bossPortraitOnion;
	[SerializeField]
	private Sprite _bossPortraitCarrot;
	[SerializeField]
	private string _bossQuotePotato;
	[SerializeField]
	private string _bossQuoteOnion;
	[SerializeField]
	private string _bossQuoteCarrot;
}
