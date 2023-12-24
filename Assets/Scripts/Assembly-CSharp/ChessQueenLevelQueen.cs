using UnityEngine;

public class ChessQueenLevelQueen : LevelProperties.ChessQueen.Entity
{
	[SerializeField]
	private SpriteRenderer[] dressRenderers;
	[SerializeField]
	private float easeCoefficient;
	[SerializeField]
	private float accel;
	[SerializeField]
	private float attackDecel;
	[SerializeField]
	private bool useSineEasing;
	[SerializeField]
	private float minMoveDistanceAfterLightning;
	[SerializeField]
	private HitFlash hitFlash;
	[SerializeField]
	private ChessQueenLevelCannon cannonLeft;
	[SerializeField]
	private ChessQueenLevelCannon cannonMiddle;
	[SerializeField]
	private ChessQueenLevelCannon cannonRight;
	[SerializeField]
	private Transform head;
	[SerializeField]
	private ChessQueenLevelLooseMouse mouse;
	[SerializeField]
	private float headWobbleSpeed;
	[SerializeField]
	private float headWobbleAmplitude;
	[SerializeField]
	private float headWobbleDecay;
	[SerializeField]
	private ChessQueenLevelEgg eggPrefab;
	[SerializeField]
	private Transform eggRootRight;
	[SerializeField]
	private Transform eggRootLeft;
	[SerializeField]
	private float maxTimeToHoldForTwoEggAttacks;
	[SerializeField]
	private float maxTimeToStayOpenForTwoEggAttacks;
	[SerializeField]
	private ChessQueenLevelLightning lightningPrefab;
	[SerializeField]
	public float lightningDisableRange;
	public float speedThresholdForFastAnimation;
}
