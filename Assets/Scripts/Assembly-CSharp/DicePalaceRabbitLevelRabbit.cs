using UnityEngine;

public class DicePalaceRabbitLevelRabbit : LevelProperties.DicePalaceRabbit.Entity
{
	[SerializeField]
	private AbstractProjectile orbPrefab;
	[SerializeField]
	private DicePalaceRabbitLevelMagic magicPrefab;
	[SerializeField]
	private FlowerLevelPlatform platform1;
	[SerializeField]
	private FlowerLevelPlatform platform2;
	[SerializeField]
	private Effect explosionPrefab;
}
