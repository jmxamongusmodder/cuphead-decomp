using UnityEngine;

public class FlyingGenieLevelSword : AbstractProjectile
{
	[SerializeField]
	private float outOfChestY;
	[SerializeField]
	private float outOfChestSpeed;
	[SerializeField]
	private float swordRotationSpeed;
	[SerializeField]
	private float fastSpinTime;
	[SerializeField]
	private SpriteRenderer swordRenderer;
}
