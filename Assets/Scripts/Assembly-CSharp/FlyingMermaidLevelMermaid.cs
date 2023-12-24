using UnityEngine;

public class FlyingMermaidLevelMermaid : LevelProperties.FlyingMermaid.Entity
{
	[SerializeField]
	private Transform[] walkingPositions;
	[SerializeField]
	private float introRiseTime;
	[SerializeField]
	private float tuckdownMoveTime;
	[SerializeField]
	private float tuckdownWaitTime;
	[SerializeField]
	private float tuckdownRiseTime;
	[SerializeField]
	private float regularY;
	[SerializeField]
	private float startUnderwaterY;
	[SerializeField]
	private float fishUnderwaterY;
	[SerializeField]
	private float transformMoveTime;
	[SerializeField]
	private float transformMoveX;
	[SerializeField]
	private float eelSinkTime;
	[SerializeField]
	private float eelUnderwaterY;
	[SerializeField]
	private FlyingMermaidLevelYellProjectile yellProjectilePrefab;
	[SerializeField]
	private FlyingMermaidLevelSeahorse seahorsePrefab;
	[SerializeField]
	private Effect FishSpitEffectPrefab;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private Transform yellFxRoot;
	[SerializeField]
	private FlyingMermaidLevelPufferfish[] pufferfishPrefabs;
	[SerializeField]
	private FlyingMermaidLevelPufferfish pinkPufferfishPrefab;
	[SerializeField]
	private FlyingMermaidLevelTurtle turtlePrefab;
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;
	[SerializeField]
	private SpriteRenderer spreadshotFishSprite;
	[SerializeField]
	private SpriteRenderer spinnerFishSprite;
	[SerializeField]
	private SpriteRenderer homerFishSprite;
	[SerializeField]
	private SpriteRenderer spreadshotFishOverlaySprite;
	[SerializeField]
	private SpriteRenderer spinnerFishOverlaySprite;
	[SerializeField]
	private SpriteRenderer homerFishOverlaySprite;
	[SerializeField]
	private FlyingMermaidLevelFish spreadshotFishPrefab;
	[SerializeField]
	private FlyingMermaidLevelFish spinnerFishPrefab;
	[SerializeField]
	private FlyingMermaidLevelFish homerFishPrefab;
	[SerializeField]
	private BasicProjectile fishSpreadshotBulletPrefab;
	[SerializeField]
	private FlyingMermaidLevelFishSpinner fishSpinnerBulletPrefab;
	[SerializeField]
	private FlyingMermaidLevelHomingProjectile fishHomerBulletPrefab;
	[SerializeField]
	private Transform fishLaunchRoot;
	[SerializeField]
	private Transform fishProjectileRoot;
	[SerializeField]
	private FlyingMermaidLevelMerdusa merdusa;
	[SerializeField]
	private Transform blockingColliders;
	[SerializeField]
	private Effect splashRight;
	[SerializeField]
	private Effect splashLeft;
	[SerializeField]
	private Transform splashRoot;
	[SerializeField]
	private Effect yellEffect;
}
