using System;
using UnityEngine;

public class LevelPlayerWeaponManager : AbstractLevelPlayerComponent
{
	[Serializable]
	public class WeaponPrefabs
	{
		[SerializeField]
		private WeaponPeashot peashot;
		[SerializeField]
		private WeaponSpread spread;
		[SerializeField]
		private WeaponArc arc;
		[SerializeField]
		private WeaponHoming homing;
		[SerializeField]
		private WeaponExploder exploder;
		[SerializeField]
		private WeaponCharge charge;
		[SerializeField]
		private WeaponBoomerang boomerang;
		[SerializeField]
		private WeaponBouncer bouncer;
		[SerializeField]
		private WeaponWideShot wideShot;
		[SerializeField]
		private WeaponUpshot upShot;
		[SerializeField]
		private WeaponCrackshot crackshot;
	}

	[Serializable]
	public class SuperPrefabs
	{
		[SerializeField]
		private AbstractPlayerSuper beam;
		[SerializeField]
		private AbstractPlayerSuper ghost;
		[SerializeField]
		private AbstractPlayerSuper invincible;
		[SerializeField]
		private AbstractPlayerSuper chaliceIII;
		[SerializeField]
		private AbstractPlayerSuper chaliceVertBeam;
		[SerializeField]
		private AbstractPlayerSuper chaliceShield;
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
	public bool allowInput;
}
