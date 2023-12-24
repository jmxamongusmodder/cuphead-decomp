using UnityEngine;

public class SaltbakerLevelSaltbaker : LevelProperties.Saltbaker.Entity
{
	public enum State
	{
		Idle = 0,
		Strawberries = 1,
		Sugarcubes = 2,
		Dough = 3,
		Limes = 4,
	}

	public State currentAttack;
	[SerializeField]
	private SpriteRenderer sugarTextReversed;
	[SerializeField]
	private Transform[] playerDefrostPositions;
	[SerializeField]
	private GameObject shadow;
	[SerializeField]
	private GameObject table;
	[SerializeField]
	private SaltbakerLevelStrawberry strawberryPrefab;
	[SerializeField]
	private SaltbakerLevelSugarcube sugarcubePrefab;
	[SerializeField]
	private SaltbakerLevelDough doughPrefab;
	[SerializeField]
	private SaltbakerLevelLime limePrefab;
	[SerializeField]
	private SaltbakerLevelStrawberryBasket strawberryBasket;
	[SerializeField]
	private SaltbakerLevelFeistTurret feistTurretPrefab;
	[SerializeField]
	private SaltBakerLevelLeaf leafFallPrefab;
	[SerializeField]
	private SaltbakerLevelBGMint bgMintPrefab;
	[SerializeField]
	private Transform[] turretRoots;
	[SerializeField]
	private Animator handAnimator;
	[SerializeField]
	private GameObject transitionCamera;
	[SerializeField]
	private float transitionDelayAfterHandsClose;
	[SerializeField]
	private float transitionDuration;
	[SerializeField]
	private SpriteRenderer transitionFader;
	[SerializeField]
	private float endPhaseTwoShakeAmount;
	[SerializeField]
	private float startPhaseThreeShakeHoldover;
	public bool phaseTwoStarted;
	public bool preventAdditionalTurretLaunch;
	[SerializeField]
	private Animator doughFXAnimator;
	[SerializeField]
	private GameObject BG;
	[SerializeField]
	private Collider2D phaseOneCollider;
	[SerializeField]
	private SpriteRenderer phaseOneRenderer;
	[SerializeField]
	private float[] timeToNextAttack;
	[SerializeField]
	private Animator mintHandAnimator;
	[SerializeField]
	private GameObject reflectionCamera;
	[SerializeField]
	private Material reflectionMaterial;
	[SerializeField]
	private GameObject reflectionTexture;
}
