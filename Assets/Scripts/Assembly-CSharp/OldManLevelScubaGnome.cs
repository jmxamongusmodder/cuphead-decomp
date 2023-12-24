using UnityEngine;

public class OldManLevelScubaGnome : AbstractProjectile
{
	[SerializeField]
	private Effect deathPuff;
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Transform shootRoot;
	[SerializeField]
	private SpriteRenderer underwaterSprite;
}
