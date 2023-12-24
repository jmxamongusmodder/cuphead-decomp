using UnityEngine;

public class FlyingBirdLevelNursePill : AbstractProjectile
{
	[SerializeField]
	private FlyingBirdLevelNursePillProjectile topHalf;
	[SerializeField]
	private FlyingBirdLevelNursePillProjectile bottomHalf;
	[SerializeField]
	private GameObject normalPill;
	[SerializeField]
	private GameObject parryPill;
}
