using System;
using UnityEngine;

public class ArcadePlayerWeaponManager : AbstractArcadePlayerComponent
{
	[Serializable]
	public class WeaponPrefabs
	{
		[SerializeField]
		private ArcadeWeaponPeashot peashot;
		[SerializeField]
		private ArcadeWeaponRocketPeashot rocketPeashot;
	}

	[Serializable]
	public class SuperPrefabs
	{
	}

	[SerializeField]
	private WeaponPrefabs weaponPrefabs;
	[SerializeField]
	private SuperPrefabs superPrefabs;
	[SerializeField]
	private Effect exDustEffect;
	[SerializeField]
	private Effect exChargeEffect;
	[SerializeField]
	private Transform exRoot;
}
