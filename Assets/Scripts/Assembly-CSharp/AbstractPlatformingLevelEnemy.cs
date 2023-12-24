using UnityEngine;

public class AbstractPlatformingLevelEnemy : AbstractLevelEntity
{
	public enum StartCondition
	{
		LevelStart = 0,
		TriggerVolume = 1,
		Instant = 2,
		Custom = 3,
	}

	[SerializeField]
	private EnemyID _id;
	[SerializeField]
	protected StartCondition _startCondition;
	[SerializeField]
	private float _startDelay;
	[SerializeField]
	protected Vector2 _triggerPosition;
	[SerializeField]
	protected Vector2 _triggerSize;
	[SerializeField]
	private PlatformingLevelGenericExplosion[] explosionPrefabs;
	[SerializeField]
	private Effect parryEffectPrefab;
}
