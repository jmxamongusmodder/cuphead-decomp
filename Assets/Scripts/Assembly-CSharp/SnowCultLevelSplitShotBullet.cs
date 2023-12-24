using UnityEngine;

public class SnowCultLevelSplitShotBullet : AbstractProjectile
{
	[SerializeField]
	private SnowCultLevelSplitShotBulletShattered shatteredBullet;
	[SerializeField]
	private Effect shootFX;
	[SerializeField]
	private Animator spawnFX;
	[SerializeField]
	private float wobbleX;
	[SerializeField]
	private float wobbleY;
	[SerializeField]
	private float wobbleSpeed;
	public SnowCultLevelJackFrost main;
}
