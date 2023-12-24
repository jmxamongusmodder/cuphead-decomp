using UnityEngine;

public class FlyingGenieLevelGoop : LevelProperties.FlyingGenie.Entity
{
	[SerializeField]
	private Transform topRoot;
	[SerializeField]
	private Transform bottomRoot;
	[SerializeField]
	private FlyingGenieLevelHelixProjectile projectile;
	[SerializeField]
	private Transform endRoot;
}
