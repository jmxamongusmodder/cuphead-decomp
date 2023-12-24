using UnityEngine;

public class RumRunnersLevelSpider : LevelProperties.RumRunners.Entity
{
	[SerializeField]
	private Transform[] spawnPoints;
	[SerializeField]
	private AnimationClip runClip;
	[SerializeField]
	private RumRunnersLevelPoliceman policeman;
	[SerializeField]
	private float deathInvincibilityBuffer;
	[SerializeField]
	private Effect deathExplodeEffect;
	[SerializeField]
	private RumRunnersLevelGrub grubPrefab;
	[SerializeField]
	private RumRunnersLevelGrubPath[] grubPaths;
	[SerializeField]
	private RumRunnersLevelMine minePrefab;
	[SerializeField]
	private RumRunnersLevelBouncingBeetle caterpillarPrefab;
	[SerializeField]
	private Transform caterpillarSpawnPoint;
	[SerializeField]
	private Effect kickFXEffect;
	[SerializeField]
	private Transform kickFXSpawnPoint;
	public bool isSummoning;
}
