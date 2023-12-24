using UnityEngine;

public class FunhousePlatformingLevelWall : PlatformingLevelBigEnemy
{
	[SerializeField]
	private Effect hornEffect;
	[SerializeField]
	private Effect honkEffect;
	[SerializeField]
	private bool isTongue;
	[SerializeField]
	private FunhousePlatformingLevelCar carPrefab;
	[SerializeField]
	private BasicProjectile shootProjectile;
	[SerializeField]
	private GameObject mouthBlockageTop;
	[SerializeField]
	private GameObject mouthBlockageBottom;
	[SerializeField]
	private GameObject middleBlockage;
	[SerializeField]
	private GameObject deadBlockage;
	[SerializeField]
	private Transform tongue;
	[SerializeField]
	private Transform topTransform;
	[SerializeField]
	private Transform bottomTransform;
	[SerializeField]
	private Transform topProjectileRoot;
	[SerializeField]
	private Transform bottomProjectileRoot;
	[SerializeField]
	private LevelBossDeathExploder explosion;
}
