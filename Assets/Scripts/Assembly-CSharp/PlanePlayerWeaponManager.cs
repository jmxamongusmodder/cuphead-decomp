using System;
using UnityEngine;

public class PlanePlayerWeaponManager : AbstractPlanePlayerComponent
{
	[Serializable]
	public class Weapons
	{
		public AbstractPlaneWeapon peashot;
		public AbstractPlaneWeapon bomb;
		public AbstractPlaneWeapon chalice3Way;
		public AbstractPlaneWeapon chaliceBomb;
	}

	[SerializeField]
	private Weapons weapons;
	[SerializeField]
	private AbstractPlaneSuper super;
	[SerializeField]
	private AbstractPlaneSuper chaliceSuper;
	[SerializeField]
	private Transform bulletRoot;
}
