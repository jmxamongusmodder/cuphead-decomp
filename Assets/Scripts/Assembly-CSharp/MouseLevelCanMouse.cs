using UnityEngine;

public class MouseLevelCanMouse : LevelProperties.Mouse.Entity
{
	[SerializeField]
	private Transform cherryBombRoot;
	[SerializeField]
	private MouseLevelCherryBombProjectile cherryBombPrefab;
	[SerializeField]
	private Transform catapult;
	[SerializeField]
	private MouseLevelCanCatapultProjectile catapultProjectilePrefab;
	[SerializeField]
	private Transform catapultRoot;
	[SerializeField]
	private Transform romanCandleRoot;
	[SerializeField]
	private MouseLevelRomanCandleProjectile romanCandlePrefab;
	[SerializeField]
	private SpriteRenderer wheelRenderer;
	[SerializeField]
	private Sprite[] wheelSprites;
	[SerializeField]
	private MouseLevelBrokenCanMouse brokenCan;
	[SerializeField]
	private MouseLevelSawBladeManager sawBlades;
	[SerializeField]
	private MouseLevelCatPeeking catPeeking;
	[SerializeField]
	private MouseLevelSpring[] springs;
}
