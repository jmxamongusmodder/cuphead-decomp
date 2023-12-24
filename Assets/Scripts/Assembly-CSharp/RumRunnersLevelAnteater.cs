using UnityEngine;

public class RumRunnersLevelAnteater : LevelProperties.RumRunners.Entity
{
	[SerializeField]
	private Transform[] spawnPoints;
	[SerializeField]
	private Vector2[] snoutShadowPositions;
	[SerializeField]
	private RumRunnersLevelSnout snout;
	[SerializeField]
	private RumRunnersLevelMobBoss mobBoss;
	[SerializeField]
	private Transform mobBossHelperTransform;
	[SerializeField]
	private CollisionChild[] collChildren;
	[SerializeField]
	private RumRunnersLevelSnoutTongue tongue;
	[SerializeField]
	private SpriteRenderer[] flipRenderers;
	[SerializeField]
	private float blinkProbability;
	[SerializeField]
	private GameObject eyes;
}
