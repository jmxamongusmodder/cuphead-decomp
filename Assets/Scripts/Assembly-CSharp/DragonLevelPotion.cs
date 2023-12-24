using UnityEngine;

public class DragonLevelPotion : AbstractProjectile
{
	public enum PotionType
	{
		Horizontal = 0,
		Vertical = 1,
		Both = 2,
	}

	[SerializeField]
	private BasicProjectile bulletPrefab;
	public PotionType type;
}
