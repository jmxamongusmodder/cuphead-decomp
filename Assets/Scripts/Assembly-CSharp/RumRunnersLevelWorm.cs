using UnityEngine;

public class RumRunnersLevelWorm : LevelProperties.RumRunners.Entity
{
	[SerializeField]
	private Sprite[] dropShadowSprites;
	[SerializeField]
	private SpriteRenderer fakePhonographShadowRenderer;
	[SerializeField]
	private SpriteRenderer realPhonographShadowRenderer;
	[SerializeField]
	private Effect dropDustEffect;
	[SerializeField]
	private RumRunnersLevelLaser laserGroup1;
	[SerializeField]
	private RumRunnersLevelLaser laserGroup2;
	[SerializeField]
	private RumRunnersLevelDiamond diamond;
	[SerializeField]
	private RumRunnersLevelBarrel barrelPrefab;
	[SerializeField]
	private Transform runnerSpawnPointTop;
	[SerializeField]
	private Transform runnerSpawnPointBottom;
	[SerializeField]
	private AudioWarble audioWarble;
}
