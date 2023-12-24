using UnityEngine;

public class AirshipCrabLevelCrab : LevelProperties.AirshipCrab.Entity
{
	[SerializeField]
	private Transform barncileRoot;
	[SerializeField]
	private Transform bubbleRoot;
	[SerializeField]
	private Collider2D crabHitBox;
	[SerializeField]
	private BasicProjectile barnicleProjectile;
	[SerializeField]
	private AirshipCrabLevelBubbles bubbleProjectile;
	[SerializeField]
	private AirshipCrabLevelGems gemProjectile;
}
