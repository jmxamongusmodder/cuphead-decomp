using UnityEngine;

public class FlyingGenieLevelGem : AbstractProjectile
{
	[SerializeField]
	private SpriteRenderer gemRenderer;
	[SerializeField]
	private float outOfChestY;
	[SerializeField]
	private float outOfChestSpeed;
}
