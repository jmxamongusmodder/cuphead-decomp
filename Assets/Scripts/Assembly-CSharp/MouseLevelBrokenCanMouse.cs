using UnityEngine;

public class MouseLevelBrokenCanMouse : LevelProperties.Mouse.Entity
{
	[SerializeField]
	private MouseLevelFlame leftFlame;
	[SerializeField]
	private MouseLevelFlame rightFlame;
	[SerializeField]
	private SpriteRenderer leftFlameSprite;
	[SerializeField]
	private SpriteRenderer rightFlameSprite;
	[SerializeField]
	private Vector2 finalFlameScale;
	[SerializeField]
	private Transform leftBulletRoot;
	[SerializeField]
	private Transform rightBulletRoot;
	[SerializeField]
	private Transform mouse;
	[SerializeField]
	private Transform platform;
	[SerializeField]
	private BasicProjectile bulletPrefab;
	[SerializeField]
	private MouseLevelSawBladeManager sawBlades;
	[SerializeField]
	private MouseLevelCat cat;
	[SerializeField]
	private MouseLevelCatPeeking catPeeking;
}
