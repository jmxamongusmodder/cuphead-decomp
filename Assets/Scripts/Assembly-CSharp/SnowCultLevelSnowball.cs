using UnityEngine;

public class SnowCultLevelSnowball : AbstractProjectile
{
	public enum Size
	{
		Small = 0,
		Medium = 1,
		Large = 2,
	}

	[SerializeField]
	private SnowCultLevelSnowball smallSnowballPrefab;
	[SerializeField]
	private SnowCultLevelSnowball mediumSnowballPrefab;
	[SerializeField]
	private SnowCultLevelSnowballExplosion snowballExplosion;
	public Size size;
	[SerializeField]
	private SpriteRenderer[] glares;
}
