using UnityEngine;

public class RumRunnersLevelCopBall : AbstractProjectile
{
	[SerializeField]
	private BasicProjectile copBullet;
	[SerializeField]
	private BasicProjectile copBulletPink;
	[SerializeField]
	private Effect dustEffect;
	[SerializeField]
	private Effect deathEffect;
	[SerializeField]
	private CircleCollider2D circleCollider;
}
