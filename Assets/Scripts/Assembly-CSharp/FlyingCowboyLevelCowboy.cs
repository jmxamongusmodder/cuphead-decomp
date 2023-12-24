using UnityEngine;

public class FlyingCowboyLevelCowboy : LevelProperties.FlyingCowboy.Entity
{
	[SerializeField]
	private SpriteRenderer posterRenderer;
	[SerializeField]
	private FlyingCowboyLevelUFO ufoPrefab;
	[SerializeField]
	private FlyingCowboyLevelBird birdPrefab;
	[SerializeField]
	private Transform birdStartPosition;
	[SerializeField]
	private Transform birdEndPosition;
	[SerializeField]
	private TriggerZone birdSafetyZone;
	[SerializeField]
	private FlyingCowboyLevelBackshot backshotPrefab;
	[SerializeField]
	private Transform[] snakeTopRoot;
	[SerializeField]
	private Transform[] snakeBottomRoot;
	[SerializeField]
	private FlyingCowboyLevelOilBlob oilBlobPrefab;
	[SerializeField]
	private Effect snakeOilMuzzleFXPrefab;
	[SerializeField]
	private GameObject cactus;
	[SerializeField]
	private Vector2 wobbleRadius;
	[SerializeField]
	private Vector2 wobbleDuration;
	[SerializeField]
	private GameObject saloonCollidersParent;
	[SerializeField]
	private SpriteRenderer lanternARenderer;
	[SerializeField]
	private SpriteRenderer lanternBRenderer;
	[SerializeField]
	private SpriteRenderer[] saloonTransitionDisableRenderers;
	[SerializeField]
	private SpriteRenderer frontWheelRenderer;
	[SerializeField]
	private SpriteRenderer backWheelRenderer;
	[SerializeField]
	private FlyingCowboyLevelDebris[] smallVacuumDebrisPrefabs;
	[SerializeField]
	private FlyingCowboyLevelDebris[] mediumVacuumDebrisPrefabs;
	[SerializeField]
	private FlyingCowboyLevelDebris[] largeVacuumDebrisPrefabs;
	[SerializeField]
	private FlyingCowboyLevelRicochetDebris ricochetPrefab;
	[SerializeField]
	private AbstractProjectile ricochetUpPrefab;
	[SerializeField]
	private Transform ricochetUpSpawnPoint;
	[SerializeField]
	private BasicProjectile coinProjectile;
	[SerializeField]
	private Transform pistolShootRoot;
	[SerializeField]
	private int debrisSpawnHorizontalSpacing;
	[SerializeField]
	private Transform vacuumDebrisAimTransform;
	[SerializeField]
	private Transform vacuumDebrisDisappearTransform;
	[SerializeField]
	private Transform vacuumSpawnTop;
	[SerializeField]
	private Transform vacuumSpawnBottom;
	[SerializeField]
	private SpriteRenderer bigVacuumRenderer;
	[SerializeField]
	private SpriteRenderer transitionVacuumRenderer;
	[SerializeField]
	private SpriteRenderer regularVacuumRenderer;
	[SerializeField]
	private SpriteRenderer bigHoseRenderer;
	[SerializeField]
	private SpriteRenderer transitionHoseRenderer;
	[SerializeField]
	private SpriteRenderer regularHoseRenderer;
	[SerializeField]
	private SpriteRenderer phase2PuffARenderer;
	[SerializeField]
	private SpriteRenderer phase2PuffBRenderer;
}
