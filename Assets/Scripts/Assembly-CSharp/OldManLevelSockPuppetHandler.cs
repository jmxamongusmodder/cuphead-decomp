using UnityEngine;

public class OldManLevelSockPuppetHandler : LevelProperties.OldMan.Entity
{
	public enum TransitionState
	{
		None = 0,
		PlatformDestroyed = 1,
		InStomach = 2,
	}

	[SerializeField]
	private MinMax idleHoldRange;
	[SerializeField]
	private MinMax lookHoldRange;
	[SerializeField]
	private float chanceToSwitchLookSides;
	[SerializeField]
	private float chanceToLaugh;
	public TransitionState transState;
	[SerializeField]
	private Transform oldManAngry;
	[SerializeField]
	private Transform oldManAngryNoseShadow;
	[SerializeField]
	private Collider2D mainPlatformCollider;
	[SerializeField]
	private OldManLevelPuppetBall puppetBallPrefab;
	[SerializeField]
	private OldManLevelSockPuppet sockPuppetLeft;
	[SerializeField]
	private OldManLevelSockPuppet sockPuppetRight;
	[SerializeField]
	private OldManLevelPlatformManager platformManager;
	[SerializeField]
	private Transform[] KDpuppetYPositions;
	[SerializeField]
	private Transform[] DpuppetYPositions;
	[SerializeField]
	private OldManLevelDwarf[] dwarves;
	[SerializeField]
	private GameObject dwarvesObject;
	[SerializeField]
	private GameObject handsParent;
	[SerializeField]
	private GameObject BGParent;
	[SerializeField]
	private GameObject beardObject;
	[SerializeField]
	private GameObject rocksUnderBeardObject;
}
