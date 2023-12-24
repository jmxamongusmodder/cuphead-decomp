using UnityEngine;

public class RobotLevelHatchShotbot : AbstractCollidableObject
{
	[SerializeField]
	private Effect smokeEffect;
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private GameObject projectile;
	[SerializeField]
	private Sprite spriteSpecial;
	[SerializeField]
	private Vector2 time;
}
