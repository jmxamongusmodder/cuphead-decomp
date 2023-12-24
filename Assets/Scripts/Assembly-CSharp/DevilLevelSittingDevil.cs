using UnityEngine;

public class DevilLevelSittingDevil : LevelProperties.Devil.Entity
{
	public enum State
	{
		Intro = 0,
		Idle = 1,
		Clap = 2,
		Head = 3,
		Pitchfork = 4,
		EndPhase1 = 5,
	}

	public State state;
	[SerializeField]
	private GameObject middleGround;
	[SerializeField]
	private DevilLevelGiantHead giantHead;
	[SerializeField]
	private DevilLevelDemon demonPrefab;
	[SerializeField]
	private Transform leftDemonPeek;
	[SerializeField]
	private Transform leftDemonJumpRoot;
	[SerializeField]
	private Transform leftDemonRunRoot;
	[SerializeField]
	private Transform leftDemonPillar;
	[SerializeField]
	private Transform leftDemonFront;
	[SerializeField]
	private Transform rightDemonPeek;
	[SerializeField]
	private Transform rightDemonJumpRoot;
	[SerializeField]
	private Transform rightDemonRunRoot;
	[SerializeField]
	private Transform rightDemonPillar;
	[SerializeField]
	private Transform rightDemonFront;
	[SerializeField]
	private DevilLevelDevilArm[] arms;
	[SerializeField]
	private DevilLevelSpiderHead spiderHead;
	[SerializeField]
	private DevilLevelDragonHead dragonHead;
	[SerializeField]
	private Transform leftWall;
	[SerializeField]
	private Transform rightWall;
	[SerializeField]
	private DevilLevelPitchforkWheelProjectile wheelProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkOrbitingProjectile wheelOrbitingProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkJumpingProjectile jumpingProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkBouncingProjectile bouncingProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkSpinnerProjectile spinnerProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkOrbitingProjectile spinnerOrbitingProjectilePrefab;
	[SerializeField]
	private DevilLevelPitchforkRingProjectile ringProjectilePrefab;
	[SerializeField]
	private GameObject holeSign;
}
