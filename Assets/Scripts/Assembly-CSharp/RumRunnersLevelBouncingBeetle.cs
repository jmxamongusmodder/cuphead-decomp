using UnityEngine;

public class RumRunnersLevelBouncingBeetle : AbstractProjectile
{
	[SerializeField]
	private Effect wallPoofEffect;
	[SerializeField]
	private Transform visualTransform;
	[SerializeField]
	private float squashAmount;
	[SerializeField]
	private float squashAmountPerpendicular;
	[SerializeField]
	private Effect explosionPrefab;
	[SerializeField]
	private SpriteDeathPartsDLC shrapnelPrefab;
}
