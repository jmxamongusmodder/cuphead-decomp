using UnityEngine;

public class RumRunnersLevelPoliceman : AbstractCollidableObject
{
	[SerializeField]
	private RumRunnersLevelPoliceBullet regularBullet;
	[SerializeField]
	private Transform bulletOriginStraight;
	[SerializeField]
	private Transform bulletOriginUp;
	[SerializeField]
	private Transform bulletOriginDown;
	[SerializeField]
	private Vector2 spawnPositionOffset;
	[SerializeField]
	private SpriteRenderer gunSmokeRenderer;
	[SerializeField]
	private SpriteRenderer gunSmokeParryRenderer;
}
