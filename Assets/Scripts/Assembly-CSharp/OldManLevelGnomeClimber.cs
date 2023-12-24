using UnityEngine;

public class OldManLevelGnomeClimber : AbstractProjectile
{
	[SerializeField]
	private Effect deathPuff;
	[SerializeField]
	private Effect[] deathParts;
	[SerializeField]
	private SpriteDeathPartsDLC hat;
	[SerializeField]
	private Effect smashEffect;
	[SerializeField]
	private Transform smashRoot;
	[SerializeField]
	private DamageReceiver damageReceiver;
	[SerializeField]
	private new Rigidbody2D rigidbody;
}
