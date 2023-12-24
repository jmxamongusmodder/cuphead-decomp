using System;

public class DamageDealer
{
	[Serializable]
	public class DamageTypesManager
	{
		public bool Player;
		public bool Enemies;
		public bool Other;
	}

	public DamageDealer(AbstractProjectile projectile)
	{
	}

}
