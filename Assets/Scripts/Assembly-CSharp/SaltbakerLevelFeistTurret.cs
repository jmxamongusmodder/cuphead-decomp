using UnityEngine;

public class SaltbakerLevelFeistTurret : AbstractCollidableObject
{
	[SerializeField]
	private SaltbakerLevelTurretBullet bulletPrefab;
	[SerializeField]
	private SaltbakerLevelTurretBullet pinkBulletPrefab;
	[SerializeField]
	private SpriteRenderer pepperText;
	[SerializeField]
	private SpriteRenderer pepperTextFlip;
	[SerializeField]
	private GameObject fxRend;
	[SerializeField]
	private GameObject sneezeFX;
}
