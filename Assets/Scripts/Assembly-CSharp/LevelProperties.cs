public class LevelProperties
{
	public class DicePalaceDomino : AbstractLevelProperties<LevelProperties.DicePalaceDomino.State, LevelProperties.DicePalaceDomino.Pattern, LevelProperties.DicePalaceDomino.States>
	{
		public class Domino : AbstractLevelPropertyGroup
		{
			public Domino(int dominoHP, float swingSpeed, float swingDistance, float swingPosY, float floorSpeed, string floorColourString, float floorTileScale, float spikesWarningDuration, string mainString)
			{
			}

		}

		public class BouncyBall : AbstractLevelPropertyGroup
		{
			public BouncyBall(float bulletSpeed, string angleString, string upDownString, MinMax attackDelayRange, string projectileTypeString, float initialAttackDelay)
			{
			}

		}

		public class Boomerang : AbstractLevelPropertyGroup
		{
			public Boomerang(float boomerangSpeed, MinMax attackDelayRange, string boomerangTypeString, float initialAttackDelay, float health)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceDomino.Pattern, LevelProperties.DicePalaceDomino.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceDomino.Pattern[][] patterns, LevelProperties.DicePalaceDomino.States stateName, LevelProperties.DicePalaceDomino.Domino domino, LevelProperties.DicePalaceDomino.BouncyBall bouncyBall, LevelProperties.DicePalaceDomino.Boomerang boomerang)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Boomerang = 0,
			BouncyBall = 1,
			Uninitialized = 2,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceDomino(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceDomino.State[] states)
		{
		}

	}

	public class AirshipStork : AbstractLevelProperties<LevelProperties.AirshipStork.State, LevelProperties.AirshipStork.Pattern, LevelProperties.AirshipStork.States>
	{
		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float parryDamage, float movementSpeed, string[] leftMovementTime, float headHeight, float pinkDurationOff)
			{
			}

		}

		public class SpiralShot : AbstractLevelPropertyGroup
		{
			public SpiralShot(float movementSpeed, float spiralRate, string[] pinkString, string[] shotDelayString, string[] spiralDirection)
			{
			}

		}

		public class Babies : AbstractLevelPropertyGroup
		{
			public Babies(float HP, string[] YVelocityRange, string[] babyDelayString, float highVerticalSpeed, float highHorizontalSpeed, float highGravity, float lowVerticalSpeed, float lowHorizontalSpeed, float lowGravity, string[] patternString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.AirshipStork.Pattern, LevelProperties.AirshipStork.States>
		{
			public State(float healthTrigger, LevelProperties.AirshipStork.Pattern[][] patterns, LevelProperties.AirshipStork.States stateName, LevelProperties.AirshipStork.Main main, LevelProperties.AirshipStork.SpiralShot spiralShot, LevelProperties.AirshipStork.Babies babies)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public AirshipStork(int hp, Level.GoalTimes goalTimes, LevelProperties.AirshipStork.State[] states)
		{
		}

	}

	public class SnowCult : AbstractLevelProperties<LevelProperties.SnowCult.State, LevelProperties.SnowCult.Pattern, LevelProperties.SnowCult.States>
	{
		public class Wizard : AbstractLevelPropertyGroup
		{
			public Wizard(string[] wizardHesitationString)
			{
			}

		}

		public class Movement : AbstractLevelPropertyGroup
		{
			public Movement(float speed, float easing, float dipAmount)
			{
			}

		}

		public class QuadShot : AbstractLevelPropertyGroup
		{
			public QuadShot(string[] attackLocationString, float distToAttack, float preattackDelay, float attackDelay, float maxOffset, float hazardSpeed, string[] hazardDirectionString, float hazardHealth, float distanceBetween, float distanceDown, float shotVelocity, float ballDelay, float hazardMoveDelay, string[] ballDelayString, float groundHealth)
			{
			}

		}

		public class Whale : AbstractLevelPropertyGroup
		{
			public Whale(float attackDelay, float recoveryDelay, float distToDrop)
			{
			}

		}

		public class Yeti : AbstractLevelPropertyGroup
		{
			public Yeti(string yetiPatternString, float jumpApexHeight, float jumpApexTime, float jumpWarning, float slideTime, float slideWarning, float hesitate, float timeToPlatforms, float timeToCameraMove, float timeForCameraMove)
			{
			}

		}

		public class Snowman : AbstractLevelPropertyGroup
		{
			public Snowman(float startCoordLeft, float startCoordRight, int snowmanCount, float meltDelay, float runTime, float health, float timeUntilUnmelt, float unmeltLoopTime, bool enableSnowman)
			{
			}

		}

		public class IcePillar : AbstractLevelPropertyGroup
		{
			public IcePillar(string offsetCoordString, float icePillarSpacing, int icePillarCount, float moveTime, float appearDelay, float hesitate, float outTime)
			{
			}

		}

		public class SeriesShot : AbstractLevelPropertyGroup
		{
			public SeriesShot(string[] seriesShotCountString, float seriesShotWarningTime, float betweenShotDelay, float bulletSpeed, string parryString)
			{
			}

		}

		public class Snowball : AbstractLevelPropertyGroup
		{
			public Snowball(string[] snowballTypeString, float snowballThrowDelay, float hesitate, float smallVelocityY, float smallVelocityX, float smallGravity, float mediumVelocityY, float mediumVelocityX, float mediumGravity, float largeVelocityY, float largeVelocityX, float largeGravity, float shotMaxSpeed, float shotMaxAngle, float shotMinSpeed, float shotMinAngle, float shotGravity, int batCount, float batHP, float batEllipseHeight, float batEllipseWidth, float batEllipseElevation, float batCirclingSpeed, float batInitialDelay, float batShotSpeed, string batAttackInterDelay, float batAttackSpeed, string batAttackPosition, string batAttackSide, string batAttackHeight, string batAttackWidth, bool batsReaddedOnEscape, string batArcModifier, string batParryableString, float batLaunchDelay)
			{
			}

		}

		public class Platforms : AbstractLevelPropertyGroup
		{
			public Platforms(int platformNum, float loopSizeX, float loopSizeY, float platformSpeed, float pivotPointYOffset)
			{
			}

		}

		public class Face : AbstractLevelPropertyGroup
		{
			public Face(string[] faceOrientationString, float hesitate)
			{
			}

		}

		public class EyeAttack : AbstractLevelPropertyGroup
		{
			public EyeAttack(float warningLength, float eyeSize, float eyeStraightSpeed, float eyeCurveSpeed, float distanceToTurn, float loopSizeY, float loopSizeX, float attackDelay, float beamDelay, float beamDuration, MinMax initialBeamDelay)
			{
			}

		}

		public class BeamAttack : AbstractLevelPropertyGroup
		{
			public BeamAttack(float warningLength, float beamDuration, float beamOffset, float beamWidth, float attackDelay)
			{
			}

		}

		public class IcePlatform : AbstractLevelPropertyGroup
		{
			public IcePlatform(float warningLength, float icePlatformHealth, float attackDelay)
			{
			}

		}

		public class ShardAttack : AbstractLevelPropertyGroup
		{
			public ShardAttack(float warningLength, int shardNumber, float shardHesitation, float shardDelay, string angleOffset, float shardSpeed, float shardHealth, float attackDelay, float circleSizeX, float circleSizeY, float circleOffsetY)
			{
			}

		}

		public class SplitShot : AbstractLevelPropertyGroup
		{
			public SplitShot(float warningLength, string[] pinkString, float shotDelay, float shotSpeed, float spreadAngle, int shatterCount, float attackDelay, string[] shotCoordString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.SnowCult.Pattern, LevelProperties.SnowCult.States>
		{
			public State(float healthTrigger, LevelProperties.SnowCult.Pattern[][] patterns, LevelProperties.SnowCult.States stateName, LevelProperties.SnowCult.Wizard wizard, LevelProperties.SnowCult.Movement movement, LevelProperties.SnowCult.QuadShot quadShot, LevelProperties.SnowCult.Whale whale, LevelProperties.SnowCult.Yeti yeti, LevelProperties.SnowCult.Snowman snowman, LevelProperties.SnowCult.IcePillar icePillar, LevelProperties.SnowCult.SeriesShot seriesShot, LevelProperties.SnowCult.Snowball snowball, LevelProperties.SnowCult.Platforms platforms, LevelProperties.SnowCult.Face face, LevelProperties.SnowCult.EyeAttack eyeAttack, LevelProperties.SnowCult.BeamAttack beamAttack, LevelProperties.SnowCult.IcePlatform icePlatform, LevelProperties.SnowCult.ShardAttack shardAttack, LevelProperties.SnowCult.SplitShot splitShot)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Switch = 1,
			Eye = 2,
			Beam = 3,
			Hazard = 4,
			Shard = 5,
			Mouth = 6,
			Quad = 7,
			Block = 8,
			SeriesShot = 9,
			Yeti = 10,
			Uninitialized = 11,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			JackFrost = 2,
			Yeti = 3,
			EasyYeti = 4,
		}

		public SnowCult(int hp, Level.GoalTimes goalTimes, LevelProperties.SnowCult.State[] states)
		{
		}

	}

	public class Veggies : AbstractLevelProperties<LevelProperties.Veggies.State, LevelProperties.Veggies.Pattern, LevelProperties.Veggies.States>
	{
		public class Potato : AbstractLevelPropertyGroup
		{
			public Potato(int hp, float idleTime, int seriesCount, float seriesDelay, int bulletCount, MinMax bulletDelay, float bulletSpeed)
			{
			}

		}

		public class Onion : AbstractLevelPropertyGroup
		{
			public Onion(int hp, float happyTime, MinMax cryLoops, string[] tearPatterns, float tearAnticipate, float tearCommaDelay, float tearTime, MinMax pinkTearRange, float heartMaxSpeed, float heartAcceleration, float heartBounceRatio, int heartHP)
			{
			}

		}

		public class Beet : AbstractLevelPropertyGroup
		{
			public Beet(int hp, float idleTime, string[] babyPatterns, float babySpeedUp, int babySpeedSpread, float babySpreadAngle, float babyDelay, float babyGroupDelay, MinMax alternateRate)
			{
			}

		}

		public class Peas : AbstractLevelPropertyGroup
		{
			public Peas(int hp)
			{
			}

		}

		public class Carrot : AbstractLevelPropertyGroup
		{
			public Carrot(int hp, float startIdleTime, MinMax idleRange, int bulletCount, float bulletDelay, float bulletSpeed, float homingInitDelay, float homingSpeed, float homingRotation, int homingHP, float homingDelay, MinMax homingDuration, float homingBgSpeed, MinMax homingNumOfCarrots)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Veggies.Pattern, LevelProperties.Veggies.States>
		{
			public State(float healthTrigger, LevelProperties.Veggies.Pattern[][] patterns, LevelProperties.Veggies.States stateName, LevelProperties.Veggies.Potato potato, LevelProperties.Veggies.Onion onion, LevelProperties.Veggies.Beet beet, LevelProperties.Veggies.Peas peas, LevelProperties.Veggies.Carrot carrot)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Potato = 0,
			Onion = 1,
			Beet = 2,
			Peas = 3,
			Carrot = 4,
			Uninitialized = 5,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public Veggies(int hp, Level.GoalTimes goalTimes, LevelProperties.Veggies.State[] states)
		{
		}

	}

	public class Frogs : AbstractLevelProperties<LevelProperties.Frogs.State, LevelProperties.Frogs.Pattern, LevelProperties.Frogs.States>
	{
		public class TallFan : AbstractLevelPropertyGroup
		{
			public TallFan(float power, int accelerationTime, MinMax duration, int hesitate)
			{
			}

		}

		public class TallFireflies : AbstractLevelPropertyGroup
		{
			public TallFireflies(string[] patterns, float speed, float followTime, float followDelay, float followDistance, int hp, float hesitate, float invincibleDuration)
			{
			}

		}

		public class ShortRage : AbstractLevelPropertyGroup
		{
			public ShortRage(float anticipationDelay, float shotSpeed, float shotDelay, int shotCount, string[] parryPatterns, float hesitate)
			{
			}

		}

		public class ShortRoll : AbstractLevelPropertyGroup
		{
			public ShortRoll(float delay, float time, float returnDelay, float hesitate)
			{
			}

		}

		public class ShortClap : AbstractLevelPropertyGroup
		{
			public ShortClap(string[] patterns, float[] angles, float bulletSpeed, float shotDelay, float hesitate)
			{
			}

		}

		public class Morph : AbstractLevelPropertyGroup
		{
			public Morph(float armDownDelay, float slotSelectionDurationPercentage, MinMax coinSpeed, MinMax coinDelay, float coinMinMaxTime, MinMax snakeSpeed, MinMax snakeDelay, float snakeMinMaxTime, float snakeDuration, MinMax bisonSpeed, MinMax bisonDelay, float bisonMinMaxTime, float bisonSmallX, float bisonBigX, int bisonDuration, float tigerSpeed, MinMax tigerDelay, float tigerMinMaxTime, float tigerDuration)
			{
			}

		}

		public class Demon : AbstractLevelPropertyGroup
		{
			public Demon(float demonFlameHeight, float demonParryHeight, MinMax demonSpeed, MinMax demonDelay, float demonMaxTime, string[] demonString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Frogs.Pattern, LevelProperties.Frogs.States>
		{
			public State(float healthTrigger, LevelProperties.Frogs.Pattern[][] patterns, LevelProperties.Frogs.States stateName, LevelProperties.Frogs.TallFan tallFan, LevelProperties.Frogs.TallFireflies tallFireflies, LevelProperties.Frogs.ShortRage shortRage, LevelProperties.Frogs.ShortRoll shortRoll, LevelProperties.Frogs.ShortClap shortClap, LevelProperties.Frogs.Morph morph, LevelProperties.Frogs.Demon demon)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			TallFan = 0,
			ShortRage = 1,
			TallFireflies = 2,
			ShortClap = 3,
			Morph = 4,
			RagePlusFireflies = 5,
			Uninitialized = 6,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Roll = 2,
			Morph = 3,
		}

		public Frogs(int hp, Level.GoalTimes goalTimes, LevelProperties.Frogs.State[] states)
		{
		}

	}

	public class DicePalaceBooze : AbstractLevelProperties<LevelProperties.DicePalaceBooze.State, LevelProperties.DicePalaceBooze.Pattern, LevelProperties.DicePalaceBooze.States>
	{
		public class Decanter : AbstractLevelPropertyGroup
		{
			public Decanter(float decanterHP, float beamDropSpeed, string attackDelayString, MinMax beamAppearDelayRange)
			{
			}

		}

		public class Tumbler : AbstractLevelPropertyGroup
		{
			public Tumbler(float tumblerHP, string beamDelayString, float beamDuration, float beamWarningDuration)
			{
			}

		}

		public class Martini : AbstractLevelPropertyGroup
		{
			public Martini(float martiniHP, int oliveHP, float oliveSpawnDelay, string moveString, float oliveSpeed, float oliveStopDuration, string[] olivePositionStringY, string[] olivePositionStringX, float bulletSpeed, string pinkString, float oliveHesitateAfterShooting)
			{
			}

		}

		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float delaySubstractAmount)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceBooze.Pattern, LevelProperties.DicePalaceBooze.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceBooze.Pattern[][] patterns, LevelProperties.DicePalaceBooze.States stateName, LevelProperties.DicePalaceBooze.Decanter decanter, LevelProperties.DicePalaceBooze.Tumbler tumbler, LevelProperties.DicePalaceBooze.Martini martini, LevelProperties.DicePalaceBooze.Main main)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceBooze(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceBooze.State[] states)
		{
		}

	}

	public class FlyingBlimp : AbstractLevelProperties<LevelProperties.FlyingBlimp.State, LevelProperties.FlyingBlimp.Pattern, LevelProperties.FlyingBlimp.States>
	{
		public class Move : AbstractLevelPropertyGroup
		{
			public Move(float pathSpeed, MinMax initalAttackDelayRange)
			{
			}

		}

		public class DashSummon : AbstractLevelPropertyGroup
		{
			public DashSummon(string[] patternString, float hold, float dashSpeed, float reeentryDelay, float summonSpeed, float summonHesitate)
			{
			}

		}

		public class Enemy : AbstractLevelPropertyGroup
		{
			public Enemy(bool active, int hp, float speed, float shotDelay, MinMax stopDistance, float stringDelay, string[] spawnString, string[] typeString, MinMax spreadAngle, int numBullets, float ASpeed, float BSpeed, MinMax APinkOccurance)
			{
			}

		}

		public class Tornado : AbstractLevelPropertyGroup
		{
			public Tornado(float moveSpeed, float homingSpeed, float loopDuration, float hesitateAfterAttack)
			{
			}

		}

		public class Shoot : AbstractLevelPropertyGroup
		{
			public Shoot(float speedMin, float speedMax, float accelerationTime, MinMax hesitateAfterAttackRange)
			{
			}

		}

		public class Morph : AbstractLevelPropertyGroup
		{
			public Morph(float crazyAHold, float crazyBHold)
			{
			}

		}

		public class Stars : AbstractLevelPropertyGroup
		{
			public Stars(MinMax speedX, float speedY, float sineSize, float delay, string[] typeString, string[] positionString)
			{
			}

		}

		public class Sagittarius : AbstractLevelPropertyGroup
		{
			public Sagittarius(int arrowHP, int movementSpeed, MinMax attackDelayRange, float arrowInitialSpeed, float arrowWarning, MinMax homingSpreadAngle, float homingDelay, float homingSpeed, float homingRotation, MinMax homingDurationRange)
			{
			}

		}

		public class Taurus : AbstractLevelPropertyGroup
		{
			public Taurus(MinMax attackDelayRange, float movementSpeed)
			{
			}

		}

		public class Gemini : AbstractLevelPropertyGroup
		{
			public Gemini(MinMax spawnerDelay, float spawnerSpeed, float bulletDelay, float rotationSpeed, float bulletSpeed)
			{
			}

		}

		public class UFO : AbstractLevelPropertyGroup
		{
			public UFO(float UFOHP, float UFOSpeed, float UFOProximityA, float UFOProximityB, float beamDuration, float UFODelay, string[] UFOString, float UFOWarningBeamDuration, float UFOInitialDelay, float moonATKAnticipation, float moonATKDuration, float moonWaitForNextATK, bool invincibility)
			{
			}

		}

		public class Gear : AbstractLevelPropertyGroup
		{
			public Gear(int parryCount, float bounceSpeed, float bounceHeight)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.FlyingBlimp.Pattern, LevelProperties.FlyingBlimp.States>
		{
			public State(float healthTrigger, LevelProperties.FlyingBlimp.Pattern[][] patterns, LevelProperties.FlyingBlimp.States stateName, LevelProperties.FlyingBlimp.Move move, LevelProperties.FlyingBlimp.DashSummon dashSummon, LevelProperties.FlyingBlimp.Enemy enemy, LevelProperties.FlyingBlimp.Tornado tornado, LevelProperties.FlyingBlimp.Shoot shoot, LevelProperties.FlyingBlimp.Morph morph, LevelProperties.FlyingBlimp.Stars stars, LevelProperties.FlyingBlimp.Sagittarius sagittarius, LevelProperties.FlyingBlimp.Taurus taurus, LevelProperties.FlyingBlimp.Gemini gemini, LevelProperties.FlyingBlimp.UFO uFO, LevelProperties.FlyingBlimp.Gear gear)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Dash = 0,
			Tornado = 1,
			Shoot = 2,
			Uninitialized = 3,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Moon = 2,
			Sagittarius = 3,
			Taurus = 4,
			Gemini = 5,
			SagOrGem = 6,
		}

		public FlyingBlimp(int hp, Level.GoalTimes goalTimes, LevelProperties.FlyingBlimp.State[] states)
		{
		}

	}

	public class Mouse : AbstractLevelProperties<LevelProperties.Mouse.State, LevelProperties.Mouse.Pattern, LevelProperties.Mouse.States>
	{
		public class CanMove : AbstractLevelPropertyGroup
		{
			public CanMove(float speed, MinMax maxXPositionRange, float stopTime, float initialHesitate)
			{
			}

		}

		public class CanDash : AbstractLevelPropertyGroup
		{
			public CanDash(float time, float hesitate, MinMax[] springVelocityX, MinMax[] springVelocityY, float springGravity)
			{
			}

		}

		public class CanCherryBomb : AbstractLevelPropertyGroup
		{
			public CanCherryBomb(string[] patterns, float delay, MinMax xVelocity, MinMax yVelocity, float gravity, int childSpeed, float hesitate)
			{
			}

		}

		public class CanCatapult : AbstractLevelPropertyGroup
		{
			public CanCatapult(string[] patterns, float timeIn, float timeOut, float pumpDelay, float repeatDelay, int projectileSpeed, float angleOffset, float spreadAngle, int count, int hesitate)
			{
			}

		}

		public class CanRomanCandle : AbstractLevelPropertyGroup
		{
			public CanRomanCandle(MinMax count, float repeatDelay, float speed, float rotationSpeed, float timeBeforeHoming, float hesitate)
			{
			}

		}

		public class BrokenCanSawBlades : AbstractLevelPropertyGroup
		{
			public BrokenCanSawBlades(string[] patternString, float entrySpeed, float delayBeforeAttack, float delayBeforeNextSaw, float speed, MinMax fullAttackTime)
			{
			}

		}

		public class BrokenCanFlame : AbstractLevelPropertyGroup
		{
			public BrokenCanFlame(string[] attackString, float delayBeforeShot, float delayAfterShot, float shotSpeed, float chargeTime, float loopTime)
			{
			}

		}

		public class BrokenCanMove : AbstractLevelPropertyGroup
		{
			public BrokenCanMove(float speed, MinMax maxXPositionRange, float stopTime)
			{
			}

		}

		public class Claw : AbstractLevelPropertyGroup
		{
			public Claw(float attackDelay, float moveSpeed, float holdGroundTime, float leaveSpeed, string[] fallingObjectStrings, float objectStartingFallSpeed, float objectGravity, float objectSpawnDelay, float hesitateAfterAttack)
			{
			}

		}

		public class GhostMouse : AbstractLevelPropertyGroup
		{
			public GhostMouse(bool fourMice, float hp, float jailDuration, MinMax attackDelayRange, float attackAnticipation, float ballSpeed, float splitSpeed, MinMax pinkBallRange, float hesitateAfterAttack)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Mouse.Pattern, LevelProperties.Mouse.States>
		{
			public State(float healthTrigger, LevelProperties.Mouse.Pattern[][] patterns, LevelProperties.Mouse.States stateName, LevelProperties.Mouse.CanMove canMove, LevelProperties.Mouse.CanDash canDash, LevelProperties.Mouse.CanCherryBomb canCherryBomb, LevelProperties.Mouse.CanCatapult canCatapult, LevelProperties.Mouse.CanRomanCandle canRomanCandle, LevelProperties.Mouse.BrokenCanSawBlades brokenCanSawBlades, LevelProperties.Mouse.BrokenCanFlame brokenCanFlame, LevelProperties.Mouse.BrokenCanMove brokenCanMove, LevelProperties.Mouse.Claw claw, LevelProperties.Mouse.GhostMouse ghostMouse)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Move = 0,
			Dash = 1,
			CherryBomb = 2,
			Catapult = 3,
			RomanCandle = 4,
			SawBlades = 5,
			Flame = 6,
			LeftClaw = 7,
			RightClaw = 8,
			GhostMouse = 9,
			Uninitialized = 10,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			BrokenCan = 2,
			Cat = 3,
		}

		public Mouse(int hp, Level.GoalTimes goalTimes, LevelProperties.Mouse.State[] states)
		{
		}

	}

	public class Baroness : AbstractLevelProperties<LevelProperties.Baroness.State, LevelProperties.Baroness.Pattern, LevelProperties.Baroness.States>
	{
		public class BaronessVonBonbon : AbstractLevelPropertyGroup
		{
			public BaronessVonBonbon(int HP, float miniBossStart, string[] timeString, MinMax attackDelay, MinMax attackCount, int projectileHP, float projectileRotationSpeed, int projectileSpeed, float finalProjectileSpeed, float finalProjectileMoveDuration, float finalProjectileRedirectDelay, float finalProjectileRedirectCount, MinMax finalProjectileAttackDelayRange, float finalProjectileInitialDelay, string finalProjectileHeadToss)
			{
			}

		}

		public class Open : AbstractLevelPropertyGroup
		{
			public Open(int miniBossAmount, string[] miniBossString)
			{
			}

		}

		public class Jellybeans : AbstractLevelPropertyGroup
		{
			public Jellybeans(int HP, float movementSpeed, float heightDefault, float jumpSpeed, MinMax jumpHeight, float afterJumpDuration, string[] typeArray, MinMax spawnDelay, float startingPoint, float spawnDelayChangePercentage)
			{
			}

		}

		public class Gumball : AbstractLevelPropertyGroup
		{
			public Gumball(int HP, float gumballMovementSpeed, float gumballDeathSpeed, MinMax gumballAttackDurationOffRange, MinMax gumballAttackDurationOnRange, float gravity, MinMax velocityX, float rateOfFire, MinMax velocityY, MinMax offsetX)
			{
			}

		}

		public class Waffle : AbstractLevelPropertyGroup
		{
			public Waffle(int HP, float movementSpeed, float anticipation, MinMax attackDelayRange, float explodeSpeed, float explodeTwoDuration, float explodeDistance, float explodeReturnSpeed, float XAxisSpeed, float pivotPointMoveAmount)
			{
			}

		}

		public class CandyCorn : AbstractLevelPropertyGroup
		{
			public CandyCorn(int HP, float movementSpeed, string[] changeLevelString, float centerPosition, float deathMoveSpeed, float deathAcceleration, MinMax miniCornSpawnDelay, int miniCornHP, float miniCornMovementSpeed, bool spawnMinis)
			{
			}

		}

		public class Cupcake : AbstractLevelPropertyGroup
		{
			public Cupcake(int HP, string[] XSpeedString, float hold, float splashOriginalOffset, float splashOffset, bool projectileOn)
			{
			}

		}

		public class Jawbreaker : AbstractLevelPropertyGroup
		{
			public Jawbreaker(int jawbreakerMinis, float jawbreakerMiniSpace, float jawbreakerHomeDuration, int jawbreakerHomingHP, float jawbreakerHomingSpeed, float jawbreakerHomingRotation)
			{
			}

		}

		public class Peppermint : AbstractLevelPropertyGroup
		{
			public Peppermint(float peppermintSpeed, MinMax peppermintSpawnDurationRange)
			{
			}

		}

		public class Platform : AbstractLevelPropertyGroup
		{
			public Platform(float LeftBoundaryOffset, float RightBoundaryOffset, float YPosition)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Baroness.Pattern, LevelProperties.Baroness.States>
		{
			public State(float healthTrigger, LevelProperties.Baroness.Pattern[][] patterns, LevelProperties.Baroness.States stateName, LevelProperties.Baroness.BaronessVonBonbon baronessVonBonbon, LevelProperties.Baroness.Open open, LevelProperties.Baroness.Jellybeans jellybeans, LevelProperties.Baroness.Gumball gumball, LevelProperties.Baroness.Waffle waffle, LevelProperties.Baroness.CandyCorn candyCorn, LevelProperties.Baroness.Cupcake cupcake, LevelProperties.Baroness.Jawbreaker jawbreaker, LevelProperties.Baroness.Peppermint peppermint, LevelProperties.Baroness.Platform platform)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Chase = 2,
		}

		public Baroness(int hp, Level.GoalTimes goalTimes, LevelProperties.Baroness.State[] states)
		{
		}

	}

	public class RetroArcade : AbstractLevelProperties<LevelProperties.RetroArcade.State, LevelProperties.RetroArcade.Pattern, LevelProperties.RetroArcade.States>
	{
		public class Aliens : AbstractLevelPropertyGroup
		{
			public Aliens(float hp, float moveTime, float moveTimeDecrease, int numColumns, MinMax shotRate, float shotRateDecrease, float randomShotAverageTime, string[] shotColumnPattern, float bulletSpeed, MinMax bonusAppearTime, int bonusAppearCount, float bonusMoveSpeed, float pointsGained, float pointsBonus)
			{
			}

		}

		public class Caterpillar : AbstractLevelPropertyGroup
		{
			public Caterpillar(float hp, float moveTime, float moveTimeDecrease, int[] bodyParts, float shotSpeed, int dropCount, MinMax spiderDelay, int spiderCount, float spiderSpeed, MinMax spiderPathY, int spiderNumZigZags, float pointsGained, float pointsBonus)
			{
			}

		}

		public class Robots : AbstractLevelPropertyGroup
		{
			public Robots(float mainRobotHp, string mainRobotShootString, float mainRobotShootSpeed, bool mainRobotShotBounce, float mainRobotMoveSpeed, MinMax mainRobotY, float smallRobotHp, MinMax smallRobotAttackDelay, float smallRobotShootSpeed, float smallRobotRotationSpeed, float smallRobotRotationDistance, MinMax bonusDelay, int bonusCount, float bonusMoveSpeed, float bonusHp, float pointsGained, float pointsBonus, string[] robotWaves, float[] robotsXPositions, string[] robotColorPattern)
			{
			}

		}

		public class PaddleShip : AbstractLevelPropertyGroup
		{
			public PaddleShip(float hp, MinMax ySpeed, float xSpeed, float damageMultiplier, float pointsGained, float pointsBonus)
			{
			}

		}

		public class QShip : AbstractLevelPropertyGroup
		{
			public QShip(float moveSpeed, int numSpinningTiles, MinMax tileRotationSpeed, float tileRotationDistance, float shotSpreadAngle, float shotSpeed, float maxXPos, float yPos, float hp, float damageMultiplier, float tentacleWarningDuration, MinMax tentacleSpawnRange, float tentacleSpeed, float pointsGained, float pointsBonus)
			{
			}

		}

		public class UFO : AbstractLevelPropertyGroup
		{
			public UFO(float projectileSpeed, MinMax shotRate, int turretCount, float turretSpeed, float hp, MinMax bossSpeed, float moleSpeed, float moleWarningDelay, float moleAttackSpeed, float pointsGained, float pointsBonus, float initialPositionX, float[] cyclePositionX, int alienYPosition)
			{
			}

		}

		public class Toad : AbstractLevelPropertyGroup
		{
			public Toad(float hp, MinMax jumpVerticalSpeedRange, MinMax jumpHorizontalSpeedRange, float jumpGravity, MinMax jumpDelay, float pointsGained, float pointsBonus)
			{
			}

		}

		public class Worm : AbstractLevelPropertyGroup
		{
			public Worm(MinMax moveSpeed, float rocketSpeed, float rocketSpawnDelay, float rocketBrokenPieceSpeed, float hp, float tongueRotateSpeed, float pointsGained, float pointsBonus)
			{
			}

		}

		public class General : AbstractLevelPropertyGroup
		{
			public General(float accuracyBonus)
			{
			}

		}

		public class Bouncy : AbstractLevelPropertyGroup
		{
			public Bouncy(float groupMoveSpeed, float singleMoveSpeed, string[] typeString, MinMax spawnRange, int waveCount, MinMax angleRange)
			{
			}

		}

		public class Missile : AbstractLevelPropertyGroup
		{
			public Missile(string directionString, MinMax timerRelease, float missileTime, float hp, float manMoveTime, float loopYSize, float loopXSize)
			{
			}

		}

		public class Chasers : AbstractLevelPropertyGroup
		{
			public Chasers(float greenSpeed, float greenRotation, float greenHP, MinMax greenDuration, float orangeSpeed, float orangeRotation, float orangeHP, MinMax orangeDuration, float yellowSpeed, float yellowRotation, float yellowHP, MinMax yellowDuration, string[] colorString, string[] delayString, float initialSpawnTime, string[] orderString)
			{
			}

		}

		public class Sheriff : AbstractLevelPropertyGroup
		{
			public Sheriff(float moveSpeed, float moveSpeedAddition, string[] delayString, float delayMinus, string[] colorString, float shotSpeed)
			{
			}

		}

		public class Snake : AbstractLevelPropertyGroup
		{
			public Snake(float moveSpeed)
			{
			}

		}

		public class Tentacle : AbstractLevelPropertyGroup
		{
			public Tentacle(float moveSpeed, float risingSpeed, string[] targetString, int tentacleCount)
			{
			}

		}

		public class Traffic : AbstractLevelPropertyGroup
		{
			public Traffic(float moveSpeed, float moveDelay, string[] lightOrderString, float lightDelay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.RetroArcade.Pattern, LevelProperties.RetroArcade.States>
		{
			public State(float healthTrigger, LevelProperties.RetroArcade.Pattern[][] patterns, LevelProperties.RetroArcade.States stateName, LevelProperties.RetroArcade.Aliens aliens, LevelProperties.RetroArcade.Caterpillar caterpillar, LevelProperties.RetroArcade.Robots robots, LevelProperties.RetroArcade.PaddleShip paddleShip, LevelProperties.RetroArcade.QShip qShip, LevelProperties.RetroArcade.UFO uFO, LevelProperties.RetroArcade.Toad toad, LevelProperties.RetroArcade.Worm worm, LevelProperties.RetroArcade.General general, LevelProperties.RetroArcade.Bouncy bouncy, LevelProperties.RetroArcade.Missile missile, LevelProperties.RetroArcade.Chasers chasers, LevelProperties.RetroArcade.Sheriff sheriff, LevelProperties.RetroArcade.Snake snake, LevelProperties.RetroArcade.Tentacle tentacle, LevelProperties.RetroArcade.Traffic traffic)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Caterpillar = 2,
			Robots = 3,
			PaddleShip = 4,
			QShip = 5,
			UFO = 6,
			Toad = 7,
			Worm = 8,
			Aliens = 9,
			Bouncy = 10,
			MissileMan = 11,
			Chaser = 12,
			Sheriff = 13,
			Snake = 14,
			Tentacle = 15,
			Traffic = 16,
			JetpackTest = 17,
		}

		public RetroArcade(int hp, Level.GoalTimes goalTimes, LevelProperties.RetroArcade.State[] states)
		{
		}

	}

	public class AirshipCrab : AbstractLevelProperties<LevelProperties.AirshipCrab.State, LevelProperties.AirshipCrab.Pattern, LevelProperties.AirshipCrab.States>
	{
		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float openCrabOffsetY)
			{
			}

		}

		public class Barnicles : AbstractLevelPropertyGroup
		{
			public Barnicles(float bulletSpeed, float shotDelay, float hesitate, float barnicleAmount, float barnicleOffsetX, float barnicleOffsetY)
			{
			}

		}

		public class Gems : AbstractLevelPropertyGroup
		{
			public Gems(float bulletSpeed, string[] angleString, string[] gemReleaseDelay, float gemAmount, float gemATKAmount, float gemOffsetX, float gemOffsetY, float gemHoldDuration)
			{
			}

		}

		public class Bubbles : AbstractLevelPropertyGroup
		{
			public Bubbles(float bubbleSpeed, string[] bubbleCount, float bubbleRepeatDelay, float bubbleMainDelay, float openTimer, float sinWaveStrength)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.AirshipCrab.Pattern, LevelProperties.AirshipCrab.States>
		{
			public State(float healthTrigger, LevelProperties.AirshipCrab.Pattern[][] patterns, LevelProperties.AirshipCrab.States stateName, LevelProperties.AirshipCrab.Main main, LevelProperties.AirshipCrab.Barnicles barnicles, LevelProperties.AirshipCrab.Gems gems, LevelProperties.AirshipCrab.Bubbles bubbles)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public AirshipCrab(int hp, Level.GoalTimes goalTimes, LevelProperties.AirshipCrab.State[] states)
		{
		}

	}

	public class AirshipClam : AbstractLevelProperties<LevelProperties.AirshipClam.State, LevelProperties.AirshipClam.Pattern, LevelProperties.AirshipClam.States>
	{
		public class Spit : AbstractLevelPropertyGroup
		{
			public Spit(float movementSpeedScale, float bulletSpeed, float preShotDelay, string attackDelayString, float initialShotDelay)
			{
			}

		}

		public class Barnacles : AbstractLevelPropertyGroup
		{
			public Barnacles(float initialArcMovementX, float initialArcMovementY, float initialGravity, float parryArcMovementX, float parryArcMovementY, float parryGravity, float rollingSpeed, string typeString, string attackDelayString, float barnacleScale, MinMax attackDuration)
			{
			}

		}

		public class ClamOut : AbstractLevelPropertyGroup
		{
			public ClamOut(float bulletSpeed, float bulletRepeatDelay, string shotString, float bulletMainDelay, float preShotDelay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.AirshipClam.Pattern, LevelProperties.AirshipClam.States>
		{
			public State(float healthTrigger, LevelProperties.AirshipClam.Pattern[][] patterns, LevelProperties.AirshipClam.States stateName, LevelProperties.AirshipClam.Spit spit, LevelProperties.AirshipClam.Barnacles barnacles, LevelProperties.AirshipClam.ClamOut clamOut)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Spit = 0,
			Barnacles = 1,
			Uninitialized = 2,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public AirshipClam(int hp, Level.GoalTimes goalTimes, LevelProperties.AirshipClam.State[] states)
		{
		}

	}

	public class DicePalacePachinko : AbstractLevelProperties<LevelProperties.DicePalacePachinko.State, LevelProperties.DicePalacePachinko.Pattern, LevelProperties.DicePalacePachinko.States>
	{
		public class Pachinko : AbstractLevelPropertyGroup
		{
			public Pachinko(float platformWidthFour, float platformWidthThree, string platformHeights)
			{
			}

		}

		public class Balls : AbstractLevelPropertyGroup
		{
			public Balls(float movementSpeed, string directionString, string spawnOrderString, string ballDelayString, string pinkString, float initialAttackDelay)
			{
			}

		}

		public class Boss : AbstractLevelPropertyGroup
		{
			public Boss(MinMax movementSpeed, MinMax attackDelay, float warningDuration, float beamDuration, float initialAttackDelay, float leftBoundaryOffset, float rightBoundaryOffset)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalacePachinko.Pattern, LevelProperties.DicePalacePachinko.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalacePachinko.Pattern[][] patterns, LevelProperties.DicePalacePachinko.States stateName, LevelProperties.DicePalacePachinko.Pachinko pachinko, LevelProperties.DicePalacePachinko.Balls balls, LevelProperties.DicePalacePachinko.Boss boss)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalacePachinko(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalacePachinko.State[] states)
		{
		}

	}

	public class DicePalaceEightBall : AbstractLevelProperties<LevelProperties.DicePalaceEightBall.State, LevelProperties.DicePalaceEightBall.Pattern, LevelProperties.DicePalaceEightBall.States>
	{
		public class General : AbstractLevelPropertyGroup
		{
			public General(float shootSpeed, string[] shootString, float shootDelay, int idleLoopAmount, float attackDuration)
			{
			}

		}

		public class PoolBalls : AbstractLevelPropertyGroup
		{
			public PoolBalls(string[] sideString, float spawnDelay, float oneGroundDelay, float oneJumpVerticalSpeed, float oneJumpHorizontalSpeed, float oneJumpGravity, float twoGroundDelay, float twoJumpVerticalSpeed, float twoJumpHorizontalSpeed, float twoJumpGravity, float threeGroundDelay, float threeJumpVerticalSpeed, float threeJumpHorizontalSpeed, float threeJumpGravity, float fourGroundDelay, float fourJumpVerticalSpeed, float fourJumpHorizontalSpeed, float fourJumpGravity, float fiveGroundDelay, float fiveJumpVerticalSpeed, float fiveJumpHorizontalSpeed, float fiveJumpGravity)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceEightBall.Pattern, LevelProperties.DicePalaceEightBall.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceEightBall.Pattern[][] patterns, LevelProperties.DicePalaceEightBall.States stateName, LevelProperties.DicePalaceEightBall.General general, LevelProperties.DicePalaceEightBall.PoolBalls poolBalls)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceEightBall(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceEightBall.State[] states)
		{
		}

	}

	public class FlyingBird : AbstractLevelProperties<LevelProperties.FlyingBird.State, LevelProperties.FlyingBird.Pattern, LevelProperties.FlyingBird.States>
	{
		public class Floating : AbstractLevelPropertyGroup
		{
			public Floating(float time, float top, float bottom, MinMax attackInitialDelayRange)
			{
			}

		}

		public class Feathers : AbstractLevelPropertyGroup
		{
			public Feathers(string[] pattern, int count, float speed, float offset, float shotDelay, float initalShotDelay, float hesitate)
			{
			}

		}

		public class Eggs : AbstractLevelPropertyGroup
		{
			public Eggs(string[] pattern, float speed, float shotDelay, float hesitate)
			{
			}

		}

		public class Enemies : AbstractLevelPropertyGroup
		{
			public Enemies(bool active, int count, float delay, int health, float speed, float floatRange, float floatTime, float projectileHeight, float projectileFallTime, float projectileDelay, float groupDelay, float initalGroupDelay, bool aim)
			{
			}

		}

		public class Lasers : AbstractLevelPropertyGroup
		{
			public Lasers(float speed, float hesitate)
			{
			}

		}

		public class Turrets : AbstractLevelPropertyGroup
		{
			public Turrets(bool active, int health, float inTime, float bulletSpeed, float bulletDelay, float respawnDelay, float floatRange, float floatTime)
			{
			}

		}

		public class SmallBird : AbstractLevelPropertyGroup
		{
			public SmallBird(float timeX, float timeY, float minX, int eggCount, MinMax eggRange, float eggRotationSpeed, float eggMoveTime, float shotDelay, float shotSpeed, float leaveTime)
			{
			}

		}

		public class BigBird : AbstractLevelPropertyGroup
		{
			public BigBird(float speedXTime)
			{
			}

		}

		public class Nurses : AbstractLevelPropertyGroup
		{
			public Nurses(float bulletSpeed, float pillSpeed, float pillMaxHeight, float pillExplodeDelay, string pinkString, float attackRepeatDelay, string attackCount, float attackMainDelay)
			{
			}

		}

		public class Garbage : AbstractLevelPropertyGroup
		{
			public Garbage(float maxHeight, float speedY, float speedX, float speedXIncreaser, string shotCount, float shotDelay, float shotSize, MinMax hesitate, string[] garbageTypeString)
			{
			}

		}

		public class Heart : AbstractLevelPropertyGroup
		{
			public Heart(float movementSpeed, int shotCount, MinMax hesitate, MinMax spreadAngle, float projectileSpeed, float heartHeight, string[] shootString, string[] numOfProjectiles)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.FlyingBird.Pattern, LevelProperties.FlyingBird.States>
		{
			public State(float healthTrigger, LevelProperties.FlyingBird.Pattern[][] patterns, LevelProperties.FlyingBird.States stateName, LevelProperties.FlyingBird.Floating floating, LevelProperties.FlyingBird.Feathers feathers, LevelProperties.FlyingBird.Eggs eggs, LevelProperties.FlyingBird.Enemies enemies, LevelProperties.FlyingBird.Lasers lasers, LevelProperties.FlyingBird.Turrets turrets, LevelProperties.FlyingBird.SmallBird smallBird, LevelProperties.FlyingBird.BigBird bigBird, LevelProperties.FlyingBird.Nurses nurses, LevelProperties.FlyingBird.Garbage garbage, LevelProperties.FlyingBird.Heart heart)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Feathers = 0,
			Eggs = 1,
			Lasers = 2,
			SmallBird = 3,
			Garbage = 4,
			Heart = 5,
			Default = 6,
			Uninitialized = 7,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			HouseDeath = 2,
			Whistle = 3,
			BirdRevival = 4,
		}

		public FlyingBird(int hp, Level.GoalTimes goalTimes, LevelProperties.FlyingBird.State[] states)
		{
		}

	}

	public class DicePalaceFlyingMemory : AbstractLevelProperties<LevelProperties.DicePalaceFlyingMemory.State, LevelProperties.DicePalaceFlyingMemory.Pattern, LevelProperties.DicePalaceFlyingMemory.States>
	{
		public class Bots : AbstractLevelPropertyGroup
		{
			public Bots(float botsSpeed, float botsScale, float botsHP, float bulletWarningDuration, float bulletSpeed, string[] spawnOrder, float spawnDelay, string[] movementString, string[] directionString, float bulletDelay, bool botsOn)
			{
			}

		}

		public class FlippyCard : AbstractLevelPropertyGroup
		{
			public FlippyCard(string[] patternOrder, float initialRevealTime)
			{
			}

		}

		public class StuffedToy : AbstractLevelPropertyGroup
		{
			public StuffedToy(string[] angleString, string[] bounceCount, float bounceSpeed, float punishSpeed, float punishTime, float directionChangeDelay, float attackAnti, MinMax shotDelayRange, string[] shotType, float incrementSpeedBy, string[] angleAdditionString, float regularSpeed, float spreadSpeed, MinMax spreadAngle, int spreadBullets, float spiralSpeed, float spiralMovementRate, float musicDeathTimer)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceFlyingMemory.Pattern, LevelProperties.DicePalaceFlyingMemory.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceFlyingMemory.Pattern[][] patterns, LevelProperties.DicePalaceFlyingMemory.States stateName, LevelProperties.DicePalaceFlyingMemory.Bots bots, LevelProperties.DicePalaceFlyingMemory.FlippyCard flippyCard, LevelProperties.DicePalaceFlyingMemory.StuffedToy stuffedToy)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceFlyingMemory(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceFlyingMemory.State[] states)
		{
		}

	}

	public class FlyingGenie : AbstractLevelProperties<LevelProperties.FlyingGenie.State, LevelProperties.FlyingGenie.Pattern, LevelProperties.FlyingGenie.States>
	{
		public class Pyramids : AbstractLevelPropertyGroup
		{
			public Pyramids(float speedRotation, float warningDuration, float beamDuration, string[] attackDelayString, string[] pyramidAttackString, float pyramidLoopSize)
			{
			}

		}

		public class GemStone : AbstractLevelPropertyGroup
		{
			public GemStone(float bulletSpeed, float warningDuration, string[] attackDelayString, int ringAmount, string pinkString)
			{
			}

		}

		public class Swords : AbstractLevelPropertyGroup
		{
			public Swords(float swordSpeed, float appearDelay, float spawnDelay, float attackDelay, string[] patternPositionStrings, float repeatDelay, float hesitate, string swordPinkString)
			{
			}

		}

		public class Gems : AbstractLevelPropertyGroup
		{
			public Gems(float gemSmallSpeed, float gemBigSpeed, MinMax gemSmallDelayRange, MinMax gemBigDelayRange, string[] gemSmallAimOffset, string[] gemBigAimOffset, float gemSmallAttackDuration, float gemBigAttackDuration, float hesitate, float repeatDelay, string gemPinkString)
			{
			}

		}

		public class Sphinx : AbstractLevelPropertyGroup
		{
			public Sphinx(float sphinxSpeed, float sphinxSplitSpeed, string[] sphinxCount, float splitDelay, float miniSpawnDelay, float sphinxMainDelay, string[] sphinxAimX, string[] sphinxAimY, float sphinxSpawnNum, float miniHP, MinMax miniHomingDurationRange, float hesitate, bool dieOnCollisionPlayer, float repeatDelay, float miniInitialSpawnDelay, float homingSpeed, float homingRotation, string scarabPinkString)
			{
			}

		}

		public class Coffin : AbstractLevelPropertyGroup
		{
			public Coffin(float heartMovement, MinMax heartShotDelayRange, float attackDuration, float heartShotXSpeed, float heartShotYSpeed, float heartLoopYSize, float hesitate, float mummyGenieDelay, float mummyGenieHP, string[] mummyAppearString, string[] mummyGenieDirection, string[] mummyTypeString, float mummyASpeed, float mummyBSpeed, float mummyCSpeed, bool mummyASinWave, bool mummyCSlowdown)
			{
			}

		}

		public class Obelisk : AbstractLevelPropertyGroup
		{
			public Obelisk(float obeliskMovementSpeed, int obeliskCount, float obeliskAppearDelay, string[] obeliskGeniePos, float obeliskGenieHP, float obeliskShootDelay, float obeliskShootSpeed, string[] obeliskShotDirection, string[] obeliskPinkString, float bouncerSpeed, string[] bouncerPinkString, string[] bouncerAngleString, bool bounceShotOn, bool normalShotOn, float hesitate)
			{
			}

		}

		public class Scan : AbstractLevelPropertyGroup
		{
			public Scan(float scanDuration, float miniDuration, float movementSpeed, float initialDelay, float bulletSpeed, float shootDelay, string[] bulletString, float miniHP, float transitionDamage)
			{
			}

		}

		public class Bomb : AbstractLevelPropertyGroup
		{
			public Bomb(float bombSpeed, float bombDelay, float bombRegularSize, float bombPlusSize, float bombDiagonalSize, string[] bombPlacementString, float hesitate)
			{
			}

		}

		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float introHesitate)
			{
			}

		}

		public class Skull : AbstractLevelPropertyGroup
		{
			public Skull(MinMax skullDelayRange, float skullSpeed, int skullCount)
			{
			}

		}

		public class Bullets : AbstractLevelPropertyGroup
		{
			public Bullets(float shotSpeed, string[] shotCount, float shotDelay, float spawnerSpeed, float spawnerRotateSpeed, int spawnerCount, float spawnerShotDelay, float spawnerDistance, MinMax spawnerMoveCountRange, float spawnerHesitate, int spawnerShotCount, float spawnerMoveDelay, float childSpeed, MinMax hesitateRange, string pinkString, float marionetteMoveSpeed, float marionetteReturnSpeed)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.FlyingGenie.Pattern, LevelProperties.FlyingGenie.States>
		{
			public State(float healthTrigger, LevelProperties.FlyingGenie.Pattern[][] patterns, LevelProperties.FlyingGenie.States stateName, LevelProperties.FlyingGenie.Pyramids pyramids, LevelProperties.FlyingGenie.GemStone gemStone, LevelProperties.FlyingGenie.Swords swords, LevelProperties.FlyingGenie.Gems gems, LevelProperties.FlyingGenie.Sphinx sphinx, LevelProperties.FlyingGenie.Coffin coffin, LevelProperties.FlyingGenie.Obelisk obelisk, LevelProperties.FlyingGenie.Scan scan, LevelProperties.FlyingGenie.Bomb bomb, LevelProperties.FlyingGenie.Main main, LevelProperties.FlyingGenie.Skull skull, LevelProperties.FlyingGenie.Bullets bullets)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Giant = 2,
			Marionette = 3,
			Disappear = 4,
		}

		public FlyingGenie(int hp, Level.GoalTimes goalTimes, LevelProperties.FlyingGenie.State[] states)
		{
		}

	}

	public class OldMan : AbstractLevelProperties<LevelProperties.OldMan.State, LevelProperties.OldMan.Pattern, LevelProperties.OldMan.States>
	{
		public class Platforms : AbstractLevelPropertyGroup
		{
			public Platforms(float moveTime, string[] moveOrder, MinMax delayRange, float maxHeight, float minHeight, string[] removeOrder, string removeThreshold)
			{
			}

		}

		public class Turret : AbstractLevelPropertyGroup
		{
			public Turret(float hp, float warningDuration, float shotSpeed, float shotDelay, string[] attackString, MinMax appearDelayRange, int maxCount, string[] appearOrder, bool gnomesOn, string[] pinkShotString, float spawnDistanceCheck, float spawnSecondaryBuffer, float appearWarning)
			{
			}

		}

		public class Spikes : AbstractLevelPropertyGroup
		{
			public Spikes(float warningDuration, float attackDuration, float hp)
			{
			}

		}

		public class Hands : AbstractLevelPropertyGroup
		{
			public Hands(string[] leftHandPosString, string[] rightHandPosString, float handMoveTime, float throwWarningTime, float throwDelay, float ballSpeed, float ballRadius, float catchDelayTime, float postThrowMoveDelay, float bouncePositionSpacing, float endSlideUpTime)
			{
			}

		}

		public class Dwarf : AbstractLevelPropertyGroup
		{
			public Dwarf(string parryString, float arcHealth, string[] arcAttackDelayString, string[] arcAttackPosString, float arcAttackWarningTime, float arcApex, string[] arcShootHeightString)
			{
			}

		}

		public class GnomeLeader : AbstractLevelPropertyGroup
		{
			public GnomeLeader(float shotSpeed, float shotApexTime, float shotApexHeight, string[] platformParryString, float bossMoveTime, string[] shotPlatformString, string[] shotDelayString, string shotParryString)
			{
			}

		}

		public class ScubaGnomes : AbstractLevelPropertyGroup
		{
			public ScubaGnomes(float hp, string[] spawnDelayString, float shotSpeedA, float shotSpeedB, string[] scubaTypeString, float scubaMoveTime, float jumpHeight, float shootDistOffset, string dartParryableString)
			{
			}

		}

		public class SpitAttack : AbstractLevelPropertyGroup
		{
			public SpitAttack(float spitAttackWarning, string[] spitString, float spitDelay, float attackCooldown, float spitApexHeight, float spitApexTime, string spitParryString)
			{
			}

		}

		public class GooseAttack : AbstractLevelPropertyGroup
		{
			public GooseAttack(float goosePreAntic, float gooseWarning, float gooseDuration, float gooseCooldown, float gooseBSpeed, float gooseMSpeed, float gooseCSpeed, float gooseFSpeed, string[] gooseSpawnString)
			{
			}

		}

		public class CamelAttack : AbstractLevelPropertyGroup
		{
			public CamelAttack(float camelAttackWarning, float camelAttackSpeed, float endingPoint, float boredomPoint, float camelAttackCooldown, float camelOffScreenTime)
			{
			}

		}

		public class ClimberGnomes : AbstractLevelPropertyGroup
		{
			public ClimberGnomes(string[] gnomePositionStrings, MinMax spawnDelayRange, bool dualSmash, float preAttackDelay, float attackDelay, float climbSpeed, bool canDestroy, float health)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.OldMan.Pattern, LevelProperties.OldMan.States>
		{
			public State(float healthTrigger, LevelProperties.OldMan.Pattern[][] patterns, LevelProperties.OldMan.States stateName, LevelProperties.OldMan.Platforms platforms, LevelProperties.OldMan.Turret turret, LevelProperties.OldMan.Spikes spikes, LevelProperties.OldMan.Hands hands, LevelProperties.OldMan.Dwarf dwarf, LevelProperties.OldMan.GnomeLeader gnomeLeader, LevelProperties.OldMan.ScubaGnomes scubaGnomes, LevelProperties.OldMan.SpitAttack spitAttack, LevelProperties.OldMan.GooseAttack gooseAttack, LevelProperties.OldMan.CamelAttack camelAttack, LevelProperties.OldMan.ClimberGnomes climberGnomes)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Spit = 1,
			Duck = 2,
			Camel = 3,
			Uninitialized = 4,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			SockPuppet = 2,
			GnomeLeader = 3,
		}

		public OldMan(int hp, Level.GoalTimes goalTimes, LevelProperties.OldMan.State[] states)
		{
		}

	}

	public class DicePalaceRoulette : AbstractLevelProperties<LevelProperties.DicePalaceRoulette.State, LevelProperties.DicePalaceRoulette.Pattern, LevelProperties.DicePalaceRoulette.States>
	{
		public class Platform : AbstractLevelPropertyGroup
		{
			public Platform(float platformHeightRow, float platformWidth, float platformCount, float platformOpenDuration)
			{
			}

		}

		public class Twirl : AbstractLevelPropertyGroup
		{
			public Twirl(float movementSpeed, float moveDelayRange, float hesitate, string[] twirlAmount, float scale)
			{
			}

		}

		public class MarbleDrop : AbstractLevelPropertyGroup
		{
			public MarbleDrop(float marbleSpeed, string[] marblePositionStrings, float marbleDelay, float marbleInitalDelay, float hesitate)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceRoulette.Pattern, LevelProperties.DicePalaceRoulette.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceRoulette.Pattern[][] patterns, LevelProperties.DicePalaceRoulette.States stateName, LevelProperties.DicePalaceRoulette.Platform platform, LevelProperties.DicePalaceRoulette.Twirl twirl, LevelProperties.DicePalaceRoulette.MarbleDrop marbleDrop)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Twirl = 1,
			Marble = 2,
			Uninitialized = 3,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceRoulette(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceRoulette.State[] states)
		{
		}

	}

	public class Flower : AbstractLevelProperties<LevelProperties.Flower.State, LevelProperties.Flower.Pattern, LevelProperties.Flower.States>
	{
		public class Laser : AbstractLevelPropertyGroup
		{
			public Laser(float anticHold, float attackHold, string attackType, float hesitateAfterAttack)
			{
			}

		}

		public class PodHands : AbstractLevelPropertyGroup
		{
			public PodHands(float attackDelay, float attackHold, float hesitateAfterAttack, string attackAmount, string attacktype)
			{
			}

		}

		public class Boomerang : AbstractLevelPropertyGroup
		{
			public Boomerang(int speed, float offScreenDelay, float holdDelay, float initialMovementDelay)
			{
			}

		}

		public class Bullets : AbstractLevelPropertyGroup
		{
			public Bullets(float delayNextShot, MinMax speedMinMax, float acceleration, int holdDelay, int numberOfProjectiles)
			{
			}

		}

		public class PuffUp : AbstractLevelPropertyGroup
		{
			public PuffUp(int speed, float delayExplosion, int holdDelay)
			{
			}

		}

		public class GattlingGun : AbstractLevelPropertyGroup
		{
			public GattlingGun(float loopDuration, float initialSeedDelay, float hesitateAfterAttack, float fallingSeedDelay, string[] seedSpawnString)
			{
			}

		}

		public class EnemyPlants : AbstractLevelPropertyGroup
		{
			public EnemyPlants(int chomperPlantHP, int venusPlantHP, int fallingSeedSpeed, int venusTurningSpeed, float venusTurningDelay, int venusMovmentSpeed, int miniFlowerPlantHP, int miniFlowerMovmentSpeed, MinMax miniFlowerShootDelay, int miniFlowerProjectileSpeed, int miniFlowerFriendHP, int miniFlowerProjectileDamage)
			{
			}

		}

		public class VineHands : AbstractLevelPropertyGroup
		{
			public VineHands(float firstPositionHold, float secondPositionHold, MinMax attackDelay, string handAttackString)
			{
			}

		}

		public class PollenSpit : AbstractLevelPropertyGroup
		{
			public PollenSpit(string pollenAttackCount, float consecutiveAttackHold, string pollenType, float pollenCommaDelay, int pollenSpeed, float pollenUpDownStrength)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Flower.Pattern, LevelProperties.Flower.States>
		{
			public State(float healthTrigger, LevelProperties.Flower.Pattern[][] patterns, LevelProperties.Flower.States stateName, LevelProperties.Flower.Laser laser, LevelProperties.Flower.PodHands podHands, LevelProperties.Flower.Boomerang boomerang, LevelProperties.Flower.Bullets bullets, LevelProperties.Flower.PuffUp puffUp, LevelProperties.Flower.GattlingGun gattlingGun, LevelProperties.Flower.EnemyPlants enemyPlants, LevelProperties.Flower.VineHands vineHands, LevelProperties.Flower.PollenSpit pollenSpit)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Laser = 0,
			PodHands = 1,
			GattlingGun = 2,
			VineHands = 3,
			Nothing = 4,
			Uninitialized = 5,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			PhaseTwo = 2,
		}

		public Flower(int hp, Level.GoalTimes goalTimes, LevelProperties.Flower.State[] states)
		{
		}

	}

	public class Train : AbstractLevelProperties<LevelProperties.Train.State, LevelProperties.Train.Pattern, LevelProperties.Train.States>
	{
		public class BlindSpecter : AbstractLevelPropertyGroup
		{
			public BlindSpecter(int health, int attackLoops, MinMax heightMax, MinMax timeX, MinMax timeY, float hesitate, float eyeHealth)
			{
			}

		}

		public class Skeleton : AbstractLevelPropertyGroup
		{
			public Skeleton(float health, MinMax attackDelay, float appearDelay, float slapHoldTime)
			{
			}

		}

		public class LollipopGhouls : AbstractLevelPropertyGroup
		{
			public LollipopGhouls(float health, float initDelay, float mainDelay, float warningTime, float moveTime, float moveDistance, float cannonDelay, float ghostDelay, float ghostSpeed, float ghostAimSpeed, float ghostHealth, float skullSpeed)
			{
			}

		}

		public class Engine : AbstractLevelPropertyGroup
		{
			public Engine(float health, float forwardTime, float backTime, MinMax doorTime, float tailDelay, float maxDist, float minDist, float fireDelay, int fireGravity, MinMax fireVelocityX, MinMax fireVelocityY, float projectileDelay, float projectileUpSpeed, float projectileXSpeed, float projectileGravity)
			{
			}

		}

		public class Pumpkins : AbstractLevelPropertyGroup
		{
			public Pumpkins(string bossPhaseOn, float health, float speed, float fallTime, float delay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Train.Pattern, LevelProperties.Train.States>
		{
			public State(float healthTrigger, LevelProperties.Train.Pattern[][] patterns, LevelProperties.Train.States stateName, LevelProperties.Train.BlindSpecter blindSpecter, LevelProperties.Train.Skeleton skeleton, LevelProperties.Train.LollipopGhouls lollipopGhouls, LevelProperties.Train.Engine engine, LevelProperties.Train.Pumpkins pumpkins)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Train = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public Train(int hp, Level.GoalTimes goalTimes, LevelProperties.Train.State[] states)
		{
		}

	}

	public class Dragon : AbstractLevelProperties<LevelProperties.Dragon.State, LevelProperties.Dragon.Pattern, LevelProperties.Dragon.States>
	{
		public class Meteor : AbstractLevelPropertyGroup
		{
			public Meteor(string[] pattern, float speedX, float timeY, float shotDelay, float hesitate)
			{
			}

		}

		public class Tail : AbstractLevelPropertyGroup
		{
			public Tail(bool active, float warningTime, float inTime, float outTime, float holdTime, MinMax attackDelay)
			{
			}

		}

		public class Peashot : AbstractLevelPropertyGroup
		{
			public Peashot(string[] patternString, float shotDelay, float speed, string colorString, float hesitate)
			{
			}

		}

		public class FireAndSmoke : AbstractLevelPropertyGroup
		{
			public FireAndSmoke(string PatternString)
			{
			}

		}

		public class FireMarchers : AbstractLevelPropertyGroup
		{
			public FireMarchers(float moveSpeed, float spawnDelay, MinMax jumpDelay, float crouchTime, float gravity, MinMax jumpSpeed, MinMax jumpAngle, MinMax jumpX)
			{
			}

		}

		public class Potions : AbstractLevelPropertyGroup
		{
			public Potions(float potionSpeed, float spitBulletSpeed, float potionHP, string[] potionTypeString, string[] shotPositionString, float potionScale, float explosionBulletScale, string[] attackCount, float repeatDelay, float attackMainDelay, float playerAimCount)
			{
			}

		}

		public class Blowtorch : AbstractLevelPropertyGroup
		{
			public Blowtorch(string[] attackDelayString, int warningDurationOne, int warningDurationTwo, float fireOnDuration, float repeatDelay, float fireSize)
			{
			}

		}

		public class Clouds : AbstractLevelPropertyGroup
		{
			public Clouds(string[] cloudPositions, float cloudSpeed, bool movingRight, float cloudDelay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Dragon.Pattern, LevelProperties.Dragon.States>
		{
			public State(float healthTrigger, LevelProperties.Dragon.Pattern[][] patterns, LevelProperties.Dragon.States stateName, LevelProperties.Dragon.Meteor meteor, LevelProperties.Dragon.Tail tail, LevelProperties.Dragon.Peashot peashot, LevelProperties.Dragon.FireAndSmoke fireAndSmoke, LevelProperties.Dragon.FireMarchers fireMarchers, LevelProperties.Dragon.Potions potions, LevelProperties.Dragon.Blowtorch blowtorch, LevelProperties.Dragon.Clouds clouds)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Meteor = 0,
			Peashot = 1,
			Uninitialized = 2,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			ThreeHeads = 2,
			FireMarchers = 3,
		}

		public Dragon(int hp, Level.GoalTimes goalTimes, LevelProperties.Dragon.State[] states)
		{
		}

	}

	public class RumRunners : AbstractLevelProperties<LevelProperties.RumRunners.State, LevelProperties.RumRunners.Pattern, LevelProperties.RumRunners.States>
	{
		public class Spider : AbstractLevelPropertyGroup
		{
			public Spider(float copHealth, string[] spiderPositionString, string[] spiderActionString, string spiderActionPositionString, float spiderSpeed, float spiderEnterDelay, float copSpawnSpiderDist, string[] copPositionString, string[] copBulletTypeString, float copAttackWarning, float copExitDelay, float copBulletSpeed, float copBulletBossDamage)
			{
			}

		}

		public class Grubs : AbstractLevelPropertyGroup
		{
			public Grubs(float hp, float movementSpeed, string[] delayString, string[] appearPositionString, float warningDuration, float grubSummonWarning)
			{
			}

		}

		public class Moth : AbstractLevelPropertyGroup
		{
			public Moth(float hp, float mothSpeed, float mothSummonWarning, float mothShootDelay, float mothBulletSpeed, float mothLifetime)
			{
			}

		}

		public class Mine : AbstractLevelPropertyGroup
		{
			public Mine(float mineNumber, string[] minePlacementString, float mineExplosionRadius, float mineExplosionWarning, float mineTimer, float mineDistToExplode, float mineBossDamage, float mineCheckToLand)
			{
			}

		}

		public class Bouncing : AbstractLevelPropertyGroup
		{
			public Bouncing(float shootBeetleHealth, float shootBeetleInitialSpeed, int shootBeetleTimeToSlowdown, float shootBeetleSpeed, string[] shootBeetleAngleString, int maxBeetleCount)
			{
			}

		}

		public class Worm : AbstractLevelPropertyGroup
		{
			public Worm(MinMax rotationSpeedRange, MinMax attackOffDurationRange, float warningDuration, MinMax attackOnDurationRange, string[] directionAttackString, float directionTime, float moveTime, float moveDistance, float introDamageMultiplier)
			{
			}

		}

		public class AnteaterSnout : AbstractLevelPropertyGroup
		{
			public AnteaterSnout(string[] snoutPosString, string[] snoutActionArray, float anticipationBoilDelay, float snoutFullOutBoilDelay, float tongueHoldDuration, float finalAttackTauntDuration)
			{
			}

		}

		public class CopBall : AbstractLevelPropertyGroup
		{
			public CopBall(bool constSpeed, bool sideWallBounce, float copBallHP, string[] copBallLaunchAngleString, float copBallSpeed, float copBallShootHesitate, string[] copBallBulletAngleString, string[] copBallBulletTypeString, float copBallShootDelay, float copBallBulletSpeed, MinMax gradualSpeed, float gradualSpeedTime, int copBallMaxCount)
			{
			}

		}

		public class Barrels : AbstractLevelPropertyGroup
		{
			public Barrels(float barrelSpeed, int barrelHP, string barrelDelayString, string barrelParryString)
			{
			}

		}

		public class Boss : AbstractLevelPropertyGroup
		{
			public Boss(float initialDelay, MinMax coinSpeed, MinMax coinDelay, float coinMinMaxTime, string bossProjectileParryString, string anteaterEyeClosedOpenString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.RumRunners.Pattern, LevelProperties.RumRunners.States>
		{
			public State(float healthTrigger, LevelProperties.RumRunners.Pattern[][] patterns, LevelProperties.RumRunners.States stateName, LevelProperties.RumRunners.Spider spider, LevelProperties.RumRunners.Grubs grubs, LevelProperties.RumRunners.Moth moth, LevelProperties.RumRunners.Mine mine, LevelProperties.RumRunners.Bouncing bouncing, LevelProperties.RumRunners.Worm worm, LevelProperties.RumRunners.AnteaterSnout anteaterSnout, LevelProperties.RumRunners.CopBall copBall, LevelProperties.RumRunners.Barrels barrels, LevelProperties.RumRunners.Boss boss)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Worm = 2,
			Anteater = 3,
			MobBoss = 4,
		}

		public RumRunners(int hp, Level.GoalTimes goalTimes, LevelProperties.RumRunners.State[] states)
		{
		}

	}

	public class Saltbaker : AbstractLevelProperties<LevelProperties.Saltbaker.State, LevelProperties.Saltbaker.Pattern, LevelProperties.Saltbaker.States>
	{
		public class Strawberries : AbstractLevelPropertyGroup
		{
			public Strawberries(float diagAtkDuration, float startNextAtk, float diagAngle, string[] locationSpawnString, string[] bulletDelayString, float bulletSpeed, float firstDelay)
			{
			}

		}

		public class Sugarcubes : AbstractLevelPropertyGroup
		{
			public Sugarcubes(float sineAttackDuration, float startNextAttack, float centerHeight, float sineFreq, float sineAmplitude, float sineWavelength, string[] phaseString, string[] bulletDelayString, float firstDelay, string parryString)
			{
			}

		}

		public class Limes : AbstractLevelPropertyGroup
		{
			public Limes(float boomerangAttackDuration, float startNextAttack, float highStartY, float highEndY, float lowStartY, float lowEndY, float straightSpeed, float straightGravity, float distToTurn, MinMax angleSpeedToLerp, float angleLerpTime, string[] boomerangHeightString, string[] boomerangDelayString, float firstDelay)
			{
			}

		}

		public class Dough : AbstractLevelPropertyGroup
		{
			public Dough(float doughAttackDuration, float startNextAttack, float doughHealth, string[] doughSpawnSideString, string[] doughSpawnTypeString, string[] doughDelayString, float[] doughXSpeed, float[] doughYSpeed, float[] doughGravity, float firstDelay)
			{
			}

		}

		public class Swooper : AbstractLevelPropertyGroup
		{
			public Swooper(int numberFireSwoopers, float initialFallDelay, float jumpDelay, float apexHeight, float apexTime)
			{
			}

		}

		public class Leaf : AbstractLevelPropertyGroup
		{
			public Leaf(bool leavesOn, float leavesDelay, string[] leavesCountString, MinMax ySpeed, float yConstantSpeed, float xTime, float xDistance, MinMax leavesOffset, float reenterDelay)
			{
			}

		}

		public class Turrets : AbstractLevelPropertyGroup
		{
			public Turrets(bool feistTurretsOn, float turretHealth, float respawnTime, float shotSpeed, float shotDelay, float warningTime, string[] bulletTypeString)
			{
			}

		}

		public class Jumper : AbstractLevelPropertyGroup
		{
			public Jumper(int numberFireJumpers, float initialFallDelay, float jumpDelay, float apexHeight, float apexTime)
			{
			}

		}

		public class Bouncer : AbstractLevelPropertyGroup
		{
			public Bouncer(float bouncerDelay, float bouncerDuration, bool dropPestle, float dropPestleSpeed, float jumpXSpeed, float jumpYSpeed, float jumpGravity, int numBounces, float initDropYGravity)
			{
			}

		}

		public class Cutter : AbstractLevelPropertyGroup
		{
			public Cutter(int cutterCount, float cutterSpeed)
			{
			}

		}

		public class DoomPillar : AbstractLevelPropertyGroup
		{
			public DoomPillar(string[] platformXSpawnString, string[] platformYSpawnString, bool platformXYUnified, string[] platformSizeString, MinMax platformFallSpeed, MinMax pillarPosition, float phaseTime)
			{
			}

		}

		public class DarkHeart : AbstractLevelPropertyGroup
		{
			public DarkHeart(float heartSpeed, string[] angleOffsetString, float baseAngle, float parryTimeOut, float collisionTimeOut)
			{
			}

		}

		public class SideShot : AbstractLevelPropertyGroup
		{
			public SideShot(string[] shotTimeString, string[] shotHeightString, string[] shotTargetString, float gravity, float apexHeight)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Saltbaker.Pattern, LevelProperties.Saltbaker.States>
		{
			public State(float healthTrigger, LevelProperties.Saltbaker.Pattern[][] patterns, LevelProperties.Saltbaker.States stateName, LevelProperties.Saltbaker.Strawberries strawberries, LevelProperties.Saltbaker.Sugarcubes sugarcubes, LevelProperties.Saltbaker.Limes limes, LevelProperties.Saltbaker.Dough dough, LevelProperties.Saltbaker.Swooper swooper, LevelProperties.Saltbaker.Leaf leaf, LevelProperties.Saltbaker.Turrets turrets, LevelProperties.Saltbaker.Jumper jumper, LevelProperties.Saltbaker.Bouncer bouncer, LevelProperties.Saltbaker.Cutter cutter, LevelProperties.Saltbaker.DoomPillar doomPillar, LevelProperties.Saltbaker.DarkHeart darkHeart, LevelProperties.Saltbaker.SideShot sideShot)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Strawberries = 0,
			Sugarcubes = 1,
			Dough = 2,
			Limes = 3,
			Uninitialized = 4,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			PhaseTwo = 2,
			PhaseThree = 3,
			PhaseFour = 4,
		}

		public Saltbaker(int hp, Level.GoalTimes goalTimes, LevelProperties.Saltbaker.State[] states)
		{
		}

	}

	public class Pirate : AbstractLevelProperties<LevelProperties.Pirate.State, LevelProperties.Pirate.Pattern, LevelProperties.Pirate.States>
	{
		public class Squid : AbstractLevelPropertyGroup
		{
			public Squid(float startDelay, int endDelay, MinMax hp, float maxTime, MinMax xPos, float opacityAdd, float opacityAddTime, float darkHoldTime, float darkFadeTime, float blobDelay, float blobGravity, MinMax blobVelX, MinMax blobVelY)
			{
			}

		}

		public class Shark : AbstractLevelPropertyGroup
		{
			public Shark(float startDelay, float endDelay, float finTime, float exitSpeed, float shotExitSpeed, float attackDelay, float x)
			{
			}

		}

		public class DogFish : AbstractLevelPropertyGroup
		{
			public DogFish(float startDelay, float endDelay, float startSpeed, float endSpeed, float speedFalloffTime, int hp, int count, MinMax nextFishDelay, float deathSpeed)
			{
			}

		}

		public class Peashot : AbstractLevelPropertyGroup
		{
			public Peashot(float startDelay, int endDelay, string[] patterns, int damage, float speed, float shotDelay, string shotType)
			{
			}

		}

		public class Barrel : AbstractLevelPropertyGroup
		{
			public Barrel(float damage, float moveTime, float fallTime, float riseTime, float safeTime, float groundHold)
			{
			}

		}

		public class Cannon : AbstractLevelPropertyGroup
		{
			public Cannon(bool firing, float damage, float speed, MinMax delayRange)
			{
			}

		}

		public class Boat : AbstractLevelPropertyGroup
		{
			public Boat(float pirateFallDelay, float pirateFallTime, float winceDuration, float attackDelay, float bulletSpeed, float bulletRotationSpeed, float bulletDelay, int bulletCount, float bulletPostWait, float beamDelay, float beamDuration, float beamPostWait)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Pirate.Pattern, LevelProperties.Pirate.States>
		{
			public State(float healthTrigger, LevelProperties.Pirate.Pattern[][] patterns, LevelProperties.Pirate.States stateName, LevelProperties.Pirate.Squid squid, LevelProperties.Pirate.Shark shark, LevelProperties.Pirate.DogFish dogFish, LevelProperties.Pirate.Peashot peashot, LevelProperties.Pirate.Barrel barrel, LevelProperties.Pirate.Cannon cannon, LevelProperties.Pirate.Boat boat)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Shark = 0,
			Squid = 1,
			DogFish = 2,
			Peashot = 3,
			Boat = 4,
			Uninitialized = 5,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Boat = 2,
		}

		public Pirate(int hp, Level.GoalTimes goalTimes, LevelProperties.Pirate.State[] states)
		{
		}

	}

	public class DicePalaceCard : AbstractLevelProperties<LevelProperties.DicePalaceCard.State, LevelProperties.DicePalaceCard.Pattern, LevelProperties.DicePalaceCard.States>
	{
		public class Blocks : AbstractLevelPropertyGroup
		{
			public Blocks(float blockSpeed, float blockDropSpeed, string[] cardTypeString, string[] cardAmountString, float attackDelayRange, int gridWidth, int gridHeight, float blockSize)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceCard.Pattern, LevelProperties.DicePalaceCard.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceCard.Pattern[][] patterns, LevelProperties.DicePalaceCard.States stateName, LevelProperties.DicePalaceCard.Blocks blocks)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceCard(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceCard.State[] states)
		{
		}

	}

	public class ChessBOldA : AbstractLevelProperties<LevelProperties.ChessBOldA.State, LevelProperties.ChessBOldA.Pattern, LevelProperties.ChessBOldA.States>
	{
		public class Stage : AbstractLevelPropertyGroup
		{
			public Stage(float platformHeight)
			{
			}

		}

		public class Bishop : AbstractLevelPropertyGroup
		{
			public Bishop(int bishopHealth, float bishopScale, float stunnedTime, bool canHurtPlayer)
			{
			}

		}

		public class Pink : AbstractLevelPropertyGroup
		{
			public Pink(float pinkScale, float pinkPathRadius, string pinkSpeedString, string pinkDirString)
			{
			}

		}

		public class Walls : AbstractLevelPropertyGroup
		{
			public Walls(float wallPathRadius, float wallLength, string wallNumberString, string[] wallNullString, string wallSpeedString, string wallDirString)
			{
			}

		}

		public class BishopPath : AbstractLevelPropertyGroup
		{
			public BishopPath(string pathTypeString, string pathSpeedString, string pathDirString, float straightPathLength, float straightPathHeight, float infinitePathLength, float infinitePathHeight, float infinitePathWidth, float squarePathLength, float squarePathWidth, float squarePathHeight, float turretShotSpeed, string turretShotDelayString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessBOldA.Pattern, LevelProperties.ChessBOldA.States>
		{
			public State(float healthTrigger, LevelProperties.ChessBOldA.Pattern[][] patterns, LevelProperties.ChessBOldA.States stateName, LevelProperties.ChessBOldA.Stage stage, LevelProperties.ChessBOldA.Bishop bishop, LevelProperties.ChessBOldA.Pink pink, LevelProperties.ChessBOldA.Walls walls, LevelProperties.ChessBOldA.BishopPath bishopPath)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public ChessBOldA(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessBOldA.State[] states)
		{
		}

	}

	public class DicePalaceFlyingHorse : AbstractLevelProperties<LevelProperties.DicePalaceFlyingHorse.State, LevelProperties.DicePalaceFlyingHorse.Pattern, LevelProperties.DicePalaceFlyingHorse.States>
	{
		public class GiftBombs : AbstractLevelPropertyGroup
		{
			public GiftBombs(float initialSpeed, float explosionSpeed, float explosionTime, MinMax playerAimRange, string[] giftPositionStringY, string[] giftPositionStringX, string spreadCount, float giftDelay)
			{
			}

		}

		public class MiniHorses : AbstractLevelPropertyGroup
		{
			public MiniHorses(float HP, string[] delayString, MinMax miniSpeedRange, string[] miniTypeString, float miniTwoBulletSpeed, float miniThreeJockeySpeed, MinMax miniTwoShotDelayRange, string[] miniThreeProxString, string[] miniTwoPinkString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceFlyingHorse.Pattern, LevelProperties.DicePalaceFlyingHorse.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceFlyingHorse.Pattern[][] patterns, LevelProperties.DicePalaceFlyingHorse.States stateName, LevelProperties.DicePalaceFlyingHorse.GiftBombs giftBombs, LevelProperties.DicePalaceFlyingHorse.MiniHorses miniHorses)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceFlyingHorse(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceFlyingHorse.State[] states)
		{
		}

	}

	public class ChessKing : AbstractLevelProperties<LevelProperties.ChessKing.State, LevelProperties.ChessKing.Pattern, LevelProperties.ChessKing.States>
	{
		public class King : AbstractLevelPropertyGroup
		{
			public King(string[] trialPool, float bluePointSpeed, float kingAttackTimer, string[] kingAttackString)
			{
			}

		}

		public class Full : AbstractLevelPropertyGroup
		{
			public Full(float fullAttackAnti, float fullAttackDuration, float fullAttackRecovery)
			{
			}

		}

		public class Beam : AbstractLevelPropertyGroup
		{
			public Beam(float beamAttackAnti, float beamAttackDuration, float beamAttackRecovery)
			{
			}

		}

		public class Rat : AbstractLevelPropertyGroup
		{
			public Rat(float ratSummonAnti, float ratSummonDuration, float ratSummonRecovery, int maxRatNumber, float ratSpeed)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessKing.Pattern, LevelProperties.ChessKing.States>
		{
			public State(float healthTrigger, LevelProperties.ChessKing.Pattern[][] patterns, LevelProperties.ChessKing.States stateName, LevelProperties.ChessKing.King king, LevelProperties.ChessKing.Full full, LevelProperties.ChessKing.Beam beam, LevelProperties.ChessKing.Rat rat)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public ChessKing(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessKing.State[] states)
		{
		}

	}

	public class ChessKnight : AbstractLevelProperties<LevelProperties.ChessKnight.State, LevelProperties.ChessKnight.Pattern, LevelProperties.ChessKnight.States>
	{
		public class Knight : AbstractLevelPropertyGroup
		{
			public Knight(int knightHealth, string[] attackIntervalString, float parryCooldown)
			{
			}

		}

		public class Movement : AbstractLevelPropertyGroup
		{
			public Movement(string[] movementString, float movementSpeed, bool hasEasing)
			{
			}

		}

		public class Taunt : AbstractLevelPropertyGroup
		{
			public Taunt(float tauntDistance, float tauntDuration)
			{
			}

		}

		public class ShortAttack : AbstractLevelPropertyGroup
		{
			public ShortAttack(float shortAntiDuration, float shortAttackDuration, float shortRecoveryDuration)
			{
			}

		}

		public class LongAttack : AbstractLevelPropertyGroup
		{
			public LongAttack(float longAntiDuration, float longAttackDist, float longAttackTime, float longRecoveryDuration, float longReturnSpeed)
			{
			}

		}

		public class TauntAttack : AbstractLevelPropertyGroup
		{
			public TauntAttack(string numberTauntString, float tauntAttackAntiDuration, float tauntAttackDist, float tauntAttackTime, float tauntAttackRecoveryDuration, float tauntAttackReturnSpeed)
			{
			}

		}

		public class UpAttack : AbstractLevelPropertyGroup
		{
			public UpAttack(float upAntiDuration, float upAttackDuration, float upRecoveryDuration)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessKnight.Pattern, LevelProperties.ChessKnight.States>
		{
			public State(float healthTrigger, LevelProperties.ChessKnight.Pattern[][] patterns, LevelProperties.ChessKnight.States stateName, LevelProperties.ChessKnight.Knight knight, LevelProperties.ChessKnight.Movement movement, LevelProperties.ChessKnight.Taunt taunt, LevelProperties.ChessKnight.ShortAttack shortAttack, LevelProperties.ChessKnight.LongAttack longAttack, LevelProperties.ChessKnight.TauntAttack tauntAttack, LevelProperties.ChessKnight.UpAttack upAttack)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Long = 1,
			Short = 2,
			Up = 3,
			Uninitialized = 4,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public ChessKnight(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessKnight.State[] states)
		{
		}

	}

	public class Airplane : AbstractLevelProperties<LevelProperties.Airplane.State, LevelProperties.Airplane.Pattern, LevelProperties.Airplane.States>
	{
		public class Plane : AbstractLevelPropertyGroup
		{
			public Plane(MinMax tiltAngle, MinMax speedAtMaxTilt, float endScreenOffset, float decelerationAmount, float moveDown, float moveDownPhThree, float moveWhenRotate)
			{
			}

		}

		public class Turrets : AbstractLevelPropertyGroup
		{
			public Turrets(string[] positionString, float warningDuration, MinMax attackDelayRange, float gravity, float velocityX, float velocityY)
			{
			}

		}

		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float moveTime, float introTime, float firstAttackDelay, MinMax attackDelayRange, string attackType)
			{
			}

		}

		public class Rocket : AbstractLevelPropertyGroup
		{
			public Rocket(float homingSpeed, float homingRotation, float homingHP, float homingTime, string[] attackDelayString, string[] attackOrderString)
			{
			}

		}

		public class Parachute : AbstractLevelPropertyGroup
		{
			public Parachute(float shotACoordY, float shotBCoordY, float shotCCoordY, MinMax shotAReturnDelay, MinMax shotBReturnDelay, MinMax shotCReturnDelay, string[] pinkString, float dogDropSpeed, float dogDropSpeedAfter, string sideString, float speedForward, float easeDistanceForward, float speedReturn, float easeDistanceReturn)
			{
			}

		}

		public class Triple : AbstractLevelPropertyGroup
		{
			public Triple(float yHeight, float bulletSpeed, float initialDelay, float shootWarning, MinMax delayAfterFirst, MinMax delayAfterSecond, float shootRecovery, float returnDelay, MinMax attackAngleRange)
			{
			}

		}

		public class Terriers : AbstractLevelPropertyGroup
		{
			public Terriers(float rotationTime, string[] shotOrder, string[] shotTypeString, float shotSpeed, string[] shotDelayString, float shotMinus, float secretHPPercentage, float minAttackDistance)
			{
			}

		}

		public class Leader : AbstractLevelPropertyGroup
		{
			public Leader(string[] attackString, float attackDelay)
			{
			}

		}

		public class Dropshot : AbstractLevelPropertyGroup
		{
			public Dropshot(string[] bulletDelayStrings, string[] bulletColorString, float bulletDropSpeed, float bulletShootSpeed, bool rocketsOn)
			{
			}

		}

		public class Laser : AbstractLevelPropertyGroup
		{
			public Laser(string[] laserPositionStrings, float laserHesitation, float warningTime, float laserDuration, float laserDelay, bool forceHide)
			{
			}

		}

		public class SecretLeader : AbstractLevelPropertyGroup
		{
			public SecretLeader(float leaderPreAttackDelay, float leaderPostAttackDelay, float rocketHomingSpeed, float rocketHomingRotation, float rocketHomingHP, float rocketHomingTime, string[] rocketHomingSpawnLocation, float attackAnticipationHold, float attackRecoveryHold, float hideTime)
			{
			}

		}

		public class SecretTerriers : AbstractLevelPropertyGroup
		{
			public SecretTerriers(string[] dogAttackDelayString, string[] dogAttackOrderString, float dogPostAttackDelay, float dogTimeOut, float dogBulletArcSpeed, float dogBulletArcHeight, float dogBulletHealth, bool dogBulletWillSplit, float dogBulletSplitAngle, float dogBulletSplitSpeed, float dogRetreatDamage, string dogBulletParryString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Airplane.Pattern, LevelProperties.Airplane.States>
		{
			public State(float healthTrigger, LevelProperties.Airplane.Pattern[][] patterns, LevelProperties.Airplane.States stateName, LevelProperties.Airplane.Plane plane, LevelProperties.Airplane.Turrets turrets, LevelProperties.Airplane.Main main, LevelProperties.Airplane.Rocket rocket, LevelProperties.Airplane.Parachute parachute, LevelProperties.Airplane.Triple triple, LevelProperties.Airplane.Terriers terriers, LevelProperties.Airplane.Leader leader, LevelProperties.Airplane.Dropshot dropshot, LevelProperties.Airplane.Laser laser, LevelProperties.Airplane.SecretLeader secretLeader, LevelProperties.Airplane.SecretTerriers secretTerriers)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Rocket = 2,
			Terriers = 3,
			Leader = 4,
		}

		public Airplane(int hp, Level.GoalTimes goalTimes, LevelProperties.Airplane.State[] states)
		{
		}

	}

	public class SallyStagePlay : AbstractLevelProperties<LevelProperties.SallyStagePlay.State, LevelProperties.SallyStagePlay.Pattern, LevelProperties.SallyStagePlay.States>
	{
		public class Jump : AbstractLevelPropertyGroup
		{
			public Jump(string JumpAttackString, string JumpAttackCountString, MinMax JumpHesitate, float JumpDelay)
			{
			}

		}

		public class DiveKick : AbstractLevelPropertyGroup
		{
			public DiveKick(float DiveSpeed, MinMax DiveAngleRange, MinMax DiveAttackHeight)
			{
			}

		}

		public class JumpRoll : AbstractLevelPropertyGroup
		{
			public JumpRoll(float RollJumpVerticalMovement, MinMax RollJumpHorizontalMovement, MinMax JumpHeight, string JumpAttackTypeString, float JumpRollDuration, MinMax RollShotDelayRange)
			{
			}

		}

		public class Shuriken : AbstractLevelPropertyGroup
		{
			public Shuriken(float InitialMovementSpeed, float ArcOneGravity, float ArcOneVerticalVelocity, float ArcOneHorizontalVelocity, float ArcTwoGravity, float ArcTwoVerticalVelocity, float ArcTwoHorizontalVelocity, int NumberOfChildSpawns)
			{
			}

		}

		public class Projectile : AbstractLevelPropertyGroup
		{
			public Projectile(float projectileSpeed, float groundDuration, float groundSize)
			{
			}

		}

		public class Umbrella : AbstractLevelPropertyGroup
		{
			public Umbrella(float initialAttackDelay, float objectSpeed, float objectDropSpeed, int objectCount, float objectDelay, float hesitate, float homingMaxSpeed, float homingAcceleration, float homingBounceRatio, float homingUntilSwitchPlayer)
			{
			}

		}

		public class Kiss : AbstractLevelPropertyGroup
		{
			public Kiss(float heartSpeed, string heartType, float sineWaveSpeed, float sineWaveStrength, float hesitate)
			{
			}

		}

		public class Teleport : AbstractLevelPropertyGroup
		{
			public Teleport(string appearOffsetString, MinMax fallingSpeed, float acceleration, float hesitate, float offScreenDelay, float sawAttackDuration)
			{
			}

		}

		public class Baby : AbstractLevelPropertyGroup
		{
			public Baby(float bottleSpeed, float attackDelay, int HP, MinMax reappearDelayRange, string[] appearPosition, float hesitate)
			{
			}

		}

		public class Nun : AbstractLevelPropertyGroup
		{
			public Nun(float rulerSpeed, float attackDelay, int HP, MinMax reappearDelayRange, string[] appearPosition, float hesitate, string[] pinkString)
			{
			}

		}

		public class Husband : AbstractLevelPropertyGroup
		{
			public Husband(float deityHP, MinMax shotDelayRange, float shotSpeed, float shotScale)
			{
			}

		}

		public class General : AbstractLevelPropertyGroup
		{
			public General(string[] attackString, MinMax attackDelayRange, float finalMovementSpeed, float cupidDropMaxY, float cupidMoveSpeed)
			{
			}

		}

		public class Lightning : AbstractLevelPropertyGroup
		{
			public Lightning(float lightningSpeed, string lightningAngleString, MinMax lightningDirectAimRange, string lightningShotCount, MinMax lightningDelayRange, string lightningSpawnString)
			{
			}

		}

		public class Meteor : AbstractLevelPropertyGroup
		{
			public Meteor(float meteorSpeed, int meteorHP, float hookSpeed, float hookMaxHeight, float hookRevealExitDelay, float hookParryExitDelay, float meteorSize, string meteorSpawnString)
			{
			}

		}

		public class Tidal : AbstractLevelPropertyGroup
		{
			public Tidal(float tidalSpeed, float tidalSize, float tidalHesitate)
			{
			}

		}

		public class Roses : AbstractLevelPropertyGroup
		{
			public Roses(MinMax fallSpeed, float fallAcceleration, float groundDuration, string[] spawnString, MinMax spawnDelayRange, MinMax playerAimRange)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.SallyStagePlay.Pattern, LevelProperties.SallyStagePlay.States>
		{
			public State(float healthTrigger, LevelProperties.SallyStagePlay.Pattern[][] patterns, LevelProperties.SallyStagePlay.States stateName, LevelProperties.SallyStagePlay.Jump jump, LevelProperties.SallyStagePlay.DiveKick diveKick, LevelProperties.SallyStagePlay.JumpRoll jumpRoll, LevelProperties.SallyStagePlay.Shuriken shuriken, LevelProperties.SallyStagePlay.Projectile projectile, LevelProperties.SallyStagePlay.Umbrella umbrella, LevelProperties.SallyStagePlay.Kiss kiss, LevelProperties.SallyStagePlay.Teleport teleport, LevelProperties.SallyStagePlay.Baby baby, LevelProperties.SallyStagePlay.Nun nun, LevelProperties.SallyStagePlay.Husband husband, LevelProperties.SallyStagePlay.General general, LevelProperties.SallyStagePlay.Lightning lightning, LevelProperties.SallyStagePlay.Meteor meteor, LevelProperties.SallyStagePlay.Tidal tidal, LevelProperties.SallyStagePlay.Roses roses)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Jump = 0,
			Umbrella = 1,
			Kiss = 2,
			Teleport = 3,
			Uninitialized = 4,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			House = 2,
			Angel = 3,
			Final = 4,
		}

		public SallyStagePlay(int hp, Level.GoalTimes goalTimes, LevelProperties.SallyStagePlay.State[] states)
		{
		}

	}

	public class ChessBishop : AbstractLevelProperties<LevelProperties.ChessBishop.State, LevelProperties.ChessBishop.Pattern, LevelProperties.ChessBishop.States>
	{
		public class Main : AbstractLevelPropertyGroup
		{
		}

		public class Bishop : AbstractLevelPropertyGroup
		{
			public Bishop(float hp, float movementSpeed, string[] attackDelayString, float projectileSpeed, MinMax projectileDelayRange, float colliderOffTime, float fadeInTime, float fadeOutTime, string invisibleTimeString, float freqMultiplier, float xSpeed, float amplitude, float maxDistance)
			{
			}

		}

		public class Candle : AbstractLevelPropertyGroup
		{
			public Candle(string[] candleOrder, float candleDistToBlowout)
			{
			}

		}

		public class Lantern : AbstractLevelPropertyGroup
		{
		}

		public class State : AbstractLevelState<LevelProperties.ChessBishop.Pattern, LevelProperties.ChessBishop.States>
		{
			public State(float healthTrigger, LevelProperties.ChessBishop.Pattern[][] patterns, LevelProperties.ChessBishop.States stateName, LevelProperties.ChessBishop.Main main, LevelProperties.ChessBishop.Bishop bishop, LevelProperties.ChessBishop.Candle candle, LevelProperties.ChessBishop.Lantern lantern)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public ChessBishop(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessBishop.State[] states)
		{
		}

	}

	public class Robot : AbstractLevelProperties<LevelProperties.Robot.State, LevelProperties.Robot.Pattern, LevelProperties.Robot.States>
	{
		public class Hose : AbstractLevelPropertyGroup
		{
			public Hose(int health, float warningDuration, int beamDuration, MinMax attackDelayRange, MinMax aimAngleParameter, float delayMinus)
			{
			}

		}

		public class ShotBot : AbstractLevelPropertyGroup
		{
			public ShotBot(int hatchGateHealth, int shotbotHealth, int bulletSpeed, int shotbotFlightSpeed, MinMax pinkBulletCount, int shotbotCount, float shotbotDelay, MinMax initialSpawnDelay, MinMax shotbotWaveDelay, float shotbotShootDelay, float shotbotSpawnDelayMinus)
			{
			}

		}

		public class Cannon : AbstractLevelPropertyGroup
		{
			public Cannon(float attackDelay, string[] spreadVariableGroups, string[] shootString, MinMax attackDelayRange)
			{
			}

		}

		public class Orb : AbstractLevelPropertyGroup
		{
			public Orb(int chestHP, int orbHP, int orbMovementSpeed, float orbInitialLaserDelay, MinMax orbInitialSpawnDelay, float orbSpawnDelay, float orbSpawnDelayMinus, bool orbShieldIsActive, float orbInitalOpenDelay)
			{
			}

		}

		public class Arms : AbstractLevelPropertyGroup
		{
			public Arms(MinMax attackDelayRange, string attackString)
			{
			}

		}

		public class MagnetArms : AbstractLevelPropertyGroup
		{
			public MagnetArms(float magnetStartDelay, float magnetStayDelay, float magnetForce)
			{
			}

		}

		public class TwistyArms : AbstractLevelPropertyGroup
		{
			public TwistyArms(float warningArmsMoveAmount, float warningDuration, float twistyMoveSpeed, float twistyArmsStayDuration, string twistyPositionString, float bulletSpeed, bool shootTwicePerCycle)
			{
			}

		}

		public class BombBot : AbstractLevelPropertyGroup
		{
			public BombBot(int bombHP, int initialBombMovementSpeed, float bombDelay, MinMax bombInitialMovementDuration, int bombBossDamage, int bombHomingSpeed, float bombRotationSpeed, int bombLifeTime)
			{
			}

		}

		public class Heart : AbstractLevelPropertyGroup
		{
			public Heart(int heartHP, int heartDamageChangePercentage)
			{
			}

		}

		public class HeliHead : AbstractLevelPropertyGroup
		{
			public HeliHead(int heliheadMovementSpeed, float offScreenDelay, float attackDelay, string onScreenHeight)
			{
			}

		}

		public class BlueGem : AbstractLevelPropertyGroup
		{
			public BlueGem(float robotRotationSpeed, int robotVerticalMovementSpeed, MinMax bulletSpeed, int bulletSpeedAcceleration, float bulletSpawnDelay, int bulletSineWaveStrength, float bulletWaveSpeedMultiplier, float bulletLifeTime, int numberOfSpawnPoints, bool gemWaveRotation, MinMax gemRotationRange, string pinkString)
			{
			}

		}

		public class RedGem : AbstractLevelPropertyGroup
		{
			public RedGem(float robotRotationSpeed, int robotVerticalMovementSpeed, MinMax bulletSpeed, int bulletSpeedAcceleration, float bulletSpawnDelay, int bulletSineWaveStrength, float bulletWaveSpeedMultiplier, float bulletLifeTime, int numberOfSpawnPoints, bool gemWaveRotation, MinMax gemRotationRange, string pinkString)
			{
			}

		}

		public class Inventor : AbstractLevelPropertyGroup
		{
			public Inventor(float inventorIdleSpeedMultiplier, float initialAttackDelay, MinMax attackDuration, MinMax attackDelay, string gemColourString, int blockadeHorizontalSpawnOffset, int blockadeHorizontalSpeed, int blockadeVerticalSpeed, float blockadeGroupDelay, float blockadeIndividualDelay, int blockadeSegmentLength, int blockadeGroupSize)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Robot.Pattern, LevelProperties.Robot.States>
		{
			public State(float healthTrigger, LevelProperties.Robot.Pattern[][] patterns, LevelProperties.Robot.States stateName, LevelProperties.Robot.Hose hose, LevelProperties.Robot.ShotBot shotBot, LevelProperties.Robot.Cannon cannon, LevelProperties.Robot.Orb orb, LevelProperties.Robot.Arms arms, LevelProperties.Robot.MagnetArms magnetArms, LevelProperties.Robot.TwistyArms twistyArms, LevelProperties.Robot.BombBot bombBot, LevelProperties.Robot.Heart heart, LevelProperties.Robot.HeliHead heliHead, LevelProperties.Robot.BlueGem blueGem, LevelProperties.Robot.RedGem redGem, LevelProperties.Robot.Inventor inventor)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			HeliHead = 2,
			Inventor = 3,
		}

		public Robot(int hp, Level.GoalTimes goalTimes, LevelProperties.Robot.State[] states)
		{
		}

	}

	public class FlyingMermaid : AbstractLevelProperties<LevelProperties.FlyingMermaid.State, LevelProperties.FlyingMermaid.Pattern, LevelProperties.FlyingMermaid.States>
	{
		public class Yell : AbstractLevelPropertyGroup
		{
			public Yell(string[] patternString, float anticipateInitialHold, float mouthHold, MinMax spreadAngle, int numBullets, float bulletSpeed, float anticipateHold, float hesitateAfterAttack)
			{
			}

		}

		public class Summon : AbstractLevelPropertyGroup
		{
			public Summon(float holdBeforeCreature, float holdAfterCreature, float hesitateAfterAttack)
			{
			}

		}

		public class Seahorse : AbstractLevelPropertyGroup
		{
			public Seahorse(float hp, float maxSpeed, float acceleration, float bounceRatio, float waterForce, float homingDuration)
			{
			}

		}

		public class Pufferfish : AbstractLevelPropertyGroup
		{
			public Pufferfish(float hp, float floatSpeed, float delay, float spawnDuration, string[] spawnString, MinMax pinkPufferSpawnRange)
			{
			}

		}

		public class Turtle : AbstractLevelPropertyGroup
		{
			public Turtle(float hp, MinMax appearPosition, float speed, float bulletSpeed, MinMax timeUntilShoot, MinMax bulletTimeToExplode, float spreadshotBulletSpeed, string[] explodeSpreadshotString, float spiralRate)
			{
			}

		}

		public class Fish : AbstractLevelPropertyGroup
		{
			public Fish(float delayBeforeFirstAttack, float delayBeforeFly, float flyingSpeed, float flyingUpSpeed, float flyingGravity, float hesitateAfterAttack)
			{
			}

		}

		public class SpreadshotFish : AbstractLevelPropertyGroup
		{
			public SpreadshotFish(float attackDelay, string[] spreadVariableGroups, string[] shootString, string spreadshotPinkString)
			{
			}

		}

		public class SpinnerFish : AbstractLevelPropertyGroup
		{
			public SpinnerFish(float bulletSpeed, float timeBeforeTails, float rotationSpeed, float attackDelay, string[] shootString)
			{
			}

		}

		public class HomerFish : AbstractLevelPropertyGroup
		{
			public HomerFish(float initSpeed, float timeBeforeHoming, float bulletSpeed, float rotationSpeed, float timeBeforeDeath, float attackDelay, string[] shootString)
			{
			}

		}

		public class Eel : AbstractLevelPropertyGroup
		{
			public Eel(float hp, MinMax attackAmount, MinMax idleTime, MinMax appearDelay, MinMax spreadAngle, float numBullets, float bulletSpeed, float hesitateAfterAttack, string bulletPinkString)
			{
			}

		}

		public class Zap : AbstractLevelPropertyGroup
		{
			public Zap(float attackTime, MinMax hesitateAfterAttack, float stoneTime)
			{
			}

		}

		public class Bubbles : AbstractLevelPropertyGroup
		{
			public Bubbles(float movementSpeed, float waveSpeed, float waveAmount, float hp, MinMax attackDelayRange)
			{
			}

		}

		public class HeadBlast : AbstractLevelPropertyGroup
		{
			public HeadBlast(float movementSpeed, MinMax attackDelayRange)
			{
			}

		}

		public class Coral : AbstractLevelPropertyGroup
		{
			public Coral(float coralMoveSpeed, string[] yellowDotPosString, MinMax yellowSpawnDelayRange, MinMax bubbleEyewaveSpawnDelayRange)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.FlyingMermaid.Pattern, LevelProperties.FlyingMermaid.States>
		{
			public State(float healthTrigger, LevelProperties.FlyingMermaid.Pattern[][] patterns, LevelProperties.FlyingMermaid.States stateName, LevelProperties.FlyingMermaid.Yell yell, LevelProperties.FlyingMermaid.Summon summon, LevelProperties.FlyingMermaid.Seahorse seahorse, LevelProperties.FlyingMermaid.Pufferfish pufferfish, LevelProperties.FlyingMermaid.Turtle turtle, LevelProperties.FlyingMermaid.Fish fish, LevelProperties.FlyingMermaid.SpreadshotFish spreadshotFish, LevelProperties.FlyingMermaid.SpinnerFish spinnerFish, LevelProperties.FlyingMermaid.HomerFish homerFish, LevelProperties.FlyingMermaid.Eel eel, LevelProperties.FlyingMermaid.Zap zap, LevelProperties.FlyingMermaid.Bubbles bubbles, LevelProperties.FlyingMermaid.HeadBlast headBlast, LevelProperties.FlyingMermaid.Coral coral)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Yell = 0,
			Summon = 1,
			Fish = 2,
			Zap = 3,
			Eel = 4,
			Bubble = 5,
			HeadBlast = 6,
			BubbleHeadBlast = 7,
			Uninitialized = 8,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Merdusa = 2,
			Head = 3,
		}

		public FlyingMermaid(int hp, Level.GoalTimes goalTimes, LevelProperties.FlyingMermaid.State[] states)
		{
		}

	}

	public class Bee : AbstractLevelProperties<LevelProperties.Bee.State, LevelProperties.Bee.Pattern, LevelProperties.Bee.States>
	{
		public class Movement : AbstractLevelPropertyGroup
		{
			public Movement(bool moving, float speed, int missingPlatforms)
			{
			}

		}

		public class Grunts : AbstractLevelPropertyGroup
		{
			public Grunts(bool active, int health, string[] entrancePoints, float speed, float delay)
			{
			}

		}

		public class BlackHole : AbstractLevelPropertyGroup
		{
			public BlackHole(string[] patterns, float chargeTime, float attackTime, float speed, float health, float hesitate, float childDelay, int childSpeed, float childHealth, bool damageable)
			{
			}

		}

		public class Triangle : AbstractLevelPropertyGroup
		{
			public Triangle(int count, float chargeTime, float attackTime, float introTime, float speed, float rotationSpeed, float health, float hesitate, float childSpeed, float childDelay, float childHealth, int childCount, bool damageable)
			{
			}

		}

		public class Follower : AbstractLevelPropertyGroup
		{
			public Follower(int count, float chargeTime, float attackTime, float introTime, float homingSpeed, float homingRotation, float homingTime, float health, float hesitate, float childDelay, float childHealth, bool damageable, bool parryable)
			{
			}

		}

		public class Chain : AbstractLevelPropertyGroup
		{
			public Chain(int count, float delay, float timeX, float timeY, float speed, float hesitate, bool chainForever)
			{
			}

		}

		public class SecurityGuard : AbstractLevelPropertyGroup
		{
			public SecurityGuard(float speed, MinMax attackDelay, float idleTime, float warningTime, float childSpeed, int childCount)
			{
			}

		}

		public class General : AbstractLevelPropertyGroup
		{
			public General(float screenScrollSpeed, float movementSpeed, float movementOffset)
			{
			}

		}

		public class WingSwipe : AbstractLevelPropertyGroup
		{
			public WingSwipe(float movementSpeed, string[] attackCount, float attackDuration, float maxDistance, float warningDuration, float warningMovementSpeed, float warningMaxDistance, MinMax hesitateRange)
			{
			}

		}

		public class TurbineBlasters : AbstractLevelPropertyGroup
		{
			public TurbineBlasters(float bulletSpeed, float bulletCircleTime, string[] attackDirectionString, float repeatDealy, MinMax hesitateRange)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Bee.Pattern, LevelProperties.Bee.States>
		{
			public State(float healthTrigger, LevelProperties.Bee.Pattern[][] patterns, LevelProperties.Bee.States stateName, LevelProperties.Bee.Movement movement, LevelProperties.Bee.Grunts grunts, LevelProperties.Bee.BlackHole blackHole, LevelProperties.Bee.Triangle triangle, LevelProperties.Bee.Follower follower, LevelProperties.Bee.Chain chain, LevelProperties.Bee.SecurityGuard securityGuard, LevelProperties.Bee.General general, LevelProperties.Bee.WingSwipe wingSwipe, LevelProperties.Bee.TurbineBlasters turbineBlasters)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			BlackHole = 0,
			Chain = 1,
			Triangle = 2,
			Follower = 3,
			SecurityGuard = 4,
			Wing = 5,
			Turbine = 6,
			Uninitialized = 7,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Airplane = 2,
		}

		public Bee(int hp, Level.GoalTimes goalTimes, LevelProperties.Bee.State[] states)
		{
		}

	}

	public class AirshipJelly : AbstractLevelProperties<LevelProperties.AirshipJelly.State, LevelProperties.AirshipJelly.Pattern, LevelProperties.AirshipJelly.States>
	{
		public class Main : AbstractLevelPropertyGroup
		{
			public Main(float time, float orbDelay, float hurtDelay, float parryDamage, MinMax speed)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.AirshipJelly.Pattern, LevelProperties.AirshipJelly.States>
		{
			public State(float healthTrigger, LevelProperties.AirshipJelly.Pattern[][] patterns, LevelProperties.AirshipJelly.States stateName, LevelProperties.AirshipJelly.Main main)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Main = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public AirshipJelly(int hp, Level.GoalTimes goalTimes, LevelProperties.AirshipJelly.State[] states)
		{
		}

	}

	public class Slime : AbstractLevelProperties<LevelProperties.Slime.State, LevelProperties.Slime.Pattern, LevelProperties.Slime.States>
	{
		public class Jump : AbstractLevelPropertyGroup
		{
			public Jump(float groundDelay, float highJumpVerticalSpeed, float highJumpHorizontalSpeed, float highJumpGravity, float lowJumpVerticalSpeed, float lowJumpHorizontalSpeed, float lowJumpGravity, MinMax numJumps, string patternString, int bigSlimeInitialJumpPunchCount)
			{
			}

		}

		public class Punch : AbstractLevelPropertyGroup
		{
			public Punch(float preHold, float mainHold)
			{
			}

		}

		public class Tombstone : AbstractLevelPropertyGroup
		{
			public Tombstone(float moveSpeed, MinMax attackDelay, float anticipationHold, string attackOffsetString, float tinyMeltDelay, float tinyRunTime, float tinyHealth, float tinyTimeUntilUnmelt)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Slime.Pattern, LevelProperties.Slime.States>
		{
			public State(float healthTrigger, LevelProperties.Slime.Pattern[][] patterns, LevelProperties.Slime.States stateName, LevelProperties.Slime.Jump jump, LevelProperties.Slime.Punch punch, LevelProperties.Slime.Tombstone tombstone)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Jump = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			BigSlime = 2,
			Tombstone = 3,
		}

		public Slime(int hp, Level.GoalTimes goalTimes, LevelProperties.Slime.State[] states)
		{
		}

	}

	public class DicePalaceRabbit : AbstractLevelProperties<LevelProperties.DicePalaceRabbit.State, LevelProperties.DicePalaceRabbit.Pattern, LevelProperties.DicePalaceRabbit.States>
	{
		public class MagicWand : AbstractLevelPropertyGroup
		{
			public MagicWand(float spinningSpeed, float bulletSpeed, MinMax attackDelayRange, float circleDiameter, float hesitate, string safeZoneString, float bulletSize, float initialAttackDelay)
			{
			}

		}

		public class MagicParry : AbstractLevelPropertyGroup
		{
			public MagicParry(MinMax attackDelayRange, float speed, float hesitate, string pinkString, float initialAttackDelay, string magicPositions, float yOffset)
			{
			}

		}

		public class General : AbstractLevelPropertyGroup
		{
			public General(string platformOnePosition, string platformTwoPosition)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceRabbit.Pattern, LevelProperties.DicePalaceRabbit.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceRabbit.Pattern[][] patterns, LevelProperties.DicePalaceRabbit.States stateName, LevelProperties.DicePalaceRabbit.MagicWand magicWand, LevelProperties.DicePalaceRabbit.MagicParry magicParry, LevelProperties.DicePalaceRabbit.General general)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			MagicWand = 0,
			MagicParry = 1,
			Uninitialized = 2,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceRabbit(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceRabbit.State[] states)
		{
		}

	}

	public class ChessBOldB : AbstractLevelProperties<LevelProperties.ChessBOldB.State, LevelProperties.ChessBOldB.Pattern, LevelProperties.ChessBOldB.States>
	{
		public class Boss : AbstractLevelPropertyGroup
		{
			public Boss(float bossTime, string[] bulletDelayString, float bulletSpeed)
			{
			}

		}

		public class Birdie : AbstractLevelPropertyGroup
		{
			public Birdie(float flashTime, string[] spinSpeedString, string[] spinTimeString, float fadeInTime, float colliderOffTime, float prePinkTime, string[] chosenString, string[] initialDirectionString, string[] changeDirectionString, MinMax xSpeed, MinMax ySpeed, float timeToMaxSpeed, float timeToStraight)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessBOldB.Pattern, LevelProperties.ChessBOldB.States>
		{
			public State(float healthTrigger, LevelProperties.ChessBOldB.Pattern[][] patterns, LevelProperties.ChessBOldB.States stateName, LevelProperties.ChessBOldB.Boss boss, LevelProperties.ChessBOldB.Birdie birdie)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public ChessBOldB(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessBOldB.State[] states)
		{
		}

	}

	public class Test : AbstractLevelProperties<LevelProperties.Test.State, LevelProperties.Test.Pattern, LevelProperties.Test.States>
	{
		public class Moving : AbstractLevelPropertyGroup
		{
			public Moving(MinMax timeX, MinMax timeY, MinMax timeScale)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Test.Pattern, LevelProperties.Test.States>
		{
			public State(float healthTrigger, LevelProperties.Test.Pattern[][] patterns, LevelProperties.Test.States stateName, LevelProperties.Test.Moving moving)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Main = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Test = 2,
			SecondTest = 3,
		}

		public Test(int hp, Level.GoalTimes goalTimes, LevelProperties.Test.State[] states)
		{
		}

	}

	public class Clown : AbstractLevelProperties<LevelProperties.Clown.State, LevelProperties.Clown.Pattern, LevelProperties.Clown.States>
	{
		public class BumperCar : AbstractLevelPropertyGroup
		{
			public BumperCar(float movementSpeed, float dashSpeed, string[] attackDelayString, float movementDuration, string[] movementStrings, float bumperDashWarning, float movementDelay)
			{
			}

		}

		public class Duck : AbstractLevelPropertyGroup
		{
			public Duck(MinMax duckYHeightRange, string[] duckYStartPercentString, float duckXMovementSpeed, float duckYMovementSpeed, string[] duckTypeString, float duckDelay, float spinDuration, float bombSpeed)
			{
			}

		}

		public class HeliumClown : AbstractLevelPropertyGroup
		{
			public HeliumClown(bool coasterOn, float dogHP, float dogSpeed, string[] dogDelayString, string[] dogTypeString, string[] dogSpawnOrder, float heliumMoveSpeed, float heliumAcceleration, bool dogDieOnGround)
			{
			}

		}

		public class Horse : AbstractLevelPropertyGroup
		{
			public Horse(bool coasterOn, float HorseSpeed, string[] HorseString, float HorseXPosOffset, int WaveBulletCount, float WaveBulletSpeed, float WaveBulletAmount, float WaveBulletWaveSpeed, float WaveBulletDelay, float WaveATKDelay, float WaveATKRepeat, string[] WavePosString, string[] WavePinkString, float WaveHesitate, float DropBulletInitalSpeed, float DropBulletDelay, MinMax DropBulletOneDelay, MinMax DropBulletTwoDelay, float DropBulletSpeedDown, float DropATKDelay, string[] DropHorsePositionString, string[] DropBulletPositionString, float DropHesitate, float DropATKRepeat, float DropBulletReturnDelay)
			{
			}

		}

		public class Coaster : AbstractLevelPropertyGroup
		{
			public Coaster(MinMax initialDelay, float noseParrySuperGain, float coasterSpeed, float coasterBackSpeedMultipler, MinMax mainLoopDelay, string[] coasterTypeString, float coasterBackToFrontDelay)
			{
			}

		}

		public class Swing : AbstractLevelPropertyGroup
		{
			public Swing(float swingSpeed, float swingSpacing, float swingDropWarningDuration, float swingfullDropDuration, bool swingDropOn, float bulletSpeed, MinMax attackDelayRange, float HP, float movementSpeed, string[] positionString, float spawnDelay, float initialAttackDelay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Clown.Pattern, LevelProperties.Clown.States>
		{
			public State(float healthTrigger, LevelProperties.Clown.Pattern[][] patterns, LevelProperties.Clown.States stateName, LevelProperties.Clown.BumperCar bumperCar, LevelProperties.Clown.Duck duck, LevelProperties.Clown.HeliumClown heliumClown, LevelProperties.Clown.Horse horse, LevelProperties.Clown.Coaster coaster, LevelProperties.Clown.Swing swing)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			HeliumTank = 2,
			CarouselHorse = 3,
			Swing = 4,
		}

		public Clown(int hp, Level.GoalTimes goalTimes, LevelProperties.Clown.State[] states)
		{
		}

	}

	public class ChessQueen : AbstractLevelProperties<LevelProperties.ChessQueen.State, LevelProperties.ChessQueen.Pattern, LevelProperties.ChessQueen.States>
	{
		public class Turret : AbstractLevelPropertyGroup
		{
			public Turret(float leftTurretRotationTime, float rightTurretRotationTime, float middleTurretRotationTime, float turretCannonballSpeed, MinMax leftTurretRange, MinMax middleTurretRange, MinMax rightTurretRange)
			{
			}

		}

		public class Queen : AbstractLevelPropertyGroup
		{
			public Queen(float queenMovementSpeed, string[] queenAttackDelayString)
			{
			}

		}

		public class Egg : AbstractLevelPropertyGroup
		{
			public Egg(float eggGravity, MinMax eggVelocityX, MinMax eggVelocityY, float eggFireRate, MinMax eggAttackDuration, float eggSpawnCollisionTimer, float eggCooldownDuration)
			{
			}

		}

		public class Lightning : AbstractLevelPropertyGroup
		{
			public Lightning(string[] lightningPositionString, float lightningAnticipationTime, float lightningDescentTime, float lightningDelayTime, float lightningSweepSpeed)
			{
			}

		}

		public class Movement : AbstractLevelPropertyGroup
		{
			public Movement(string queenPositionString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessQueen.Pattern, LevelProperties.ChessQueen.States>
		{
			public State(float healthTrigger, LevelProperties.ChessQueen.Pattern[][] patterns, LevelProperties.ChessQueen.States stateName, LevelProperties.ChessQueen.Turret turret, LevelProperties.ChessQueen.Queen queen, LevelProperties.ChessQueen.Egg egg, LevelProperties.ChessQueen.Lightning lightning, LevelProperties.ChessQueen.Movement movement)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Egg = 1,
			Lightning = 2,
			Uninitialized = 3,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			PhaseTwo = 2,
			PhaseThree = 3,
		}

		public ChessQueen(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessQueen.State[] states)
		{
		}

	}

	public class Bat : AbstractLevelProperties<LevelProperties.Bat.State, LevelProperties.Bat.Pattern, LevelProperties.Bat.States>
	{
		public class Movement : AbstractLevelPropertyGroup
		{
			public Movement(float movementSpeed, float startPosY)
			{
			}

		}

		public class BatBouncer : AbstractLevelPropertyGroup
		{
			public BatBouncer(float mainBounceSpeed, float pinkBounceSpeed, float acceleration, float breakCounter, string[] delayBeforeAttackString, float stopDelay, string[] bounceAngleString, float hesitate)
			{
			}

		}

		public class Goblins : AbstractLevelPropertyGroup
		{
			public Goblins(bool Enabled, float HP, float runSpeed, string[] appearDelayString, string[] entranceString, MinMax shooterOccuranceRange, float timeBeforeShoot, float initalShotDelay, float bulletSpeed, float shooterHold)
			{
			}

		}

		public class BatLightning : AbstractLevelPropertyGroup
		{
			public BatLightning(float cloudCount, float cloudDistance, string[] centerOffset, string[] initialAttackDelayString, float cloudWarning, float lightningOnDuration, float cloudHeight, float hesitate)
			{
			}

		}

		public class MiniBats : AbstractLevelPropertyGroup
		{
			public MiniBats(float HP, float initialAttackDelay, float speedX, float speedY, float yMinMax, string[] batAngleString, float delay)
			{
			}

		}

		public class Pentagrams : AbstractLevelPropertyGroup
		{
			public Pentagrams(float xSpeed, float ySpeed, string[] pentagramDelayString, float pentagramSize)
			{
			}

		}

		public class CrossToss : AbstractLevelPropertyGroup
		{
			public CrossToss(float projectileSpeed, string[] attackCount, string[] crossDelayString)
			{
			}

		}

		public class WolfFire : AbstractLevelPropertyGroup
		{
			public WolfFire(float bulletSpeed, float bulletDelay, float bulletAimCount)
			{
			}

		}

		public class WolfSoul : AbstractLevelPropertyGroup
		{
			public WolfSoul(float regularSize, float attackSize, float attackDuration, string[] floatUpDuration, float floatSpeed, float floatWarningDuration, float homingSpeed, float homingRotation)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Bat.Pattern, LevelProperties.Bat.States>
		{
			public State(float healthTrigger, LevelProperties.Bat.Pattern[][] patterns, LevelProperties.Bat.States stateName, LevelProperties.Bat.Movement movement, LevelProperties.Bat.BatBouncer batBouncer, LevelProperties.Bat.Goblins goblins, LevelProperties.Bat.BatLightning batLightning, LevelProperties.Bat.MiniBats miniBats, LevelProperties.Bat.Pentagrams pentagrams, LevelProperties.Bat.CrossToss crossToss, LevelProperties.Bat.WolfFire wolfFire, LevelProperties.Bat.WolfSoul wolfSoul)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Bouncer = 0,
			Lightning = 1,
			Uninitialized = 2,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Coffin = 2,
			Wolf = 3,
		}

		public Bat(int hp, Level.GoalTimes goalTimes, LevelProperties.Bat.State[] states)
		{
		}

	}

	public class DicePalaceChips : AbstractLevelProperties<LevelProperties.DicePalaceChips.State, LevelProperties.DicePalaceChips.Pattern, LevelProperties.DicePalaceChips.States>
	{
		public class Chips : AbstractLevelPropertyGroup
		{
			public Chips(float initialAttackDelay, float chipSpeedMultiplier, string[] chipAttackString, float chipAttackDelay, MinMax attackCycleDelay, float chipSpacing)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceChips.Pattern, LevelProperties.DicePalaceChips.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceChips.Pattern[][] patterns, LevelProperties.DicePalaceChips.States stateName, LevelProperties.DicePalaceChips.Chips chips)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceChips(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceChips.State[] states)
		{
		}

	}

	public class Devil : AbstractLevelProperties<LevelProperties.Devil.State, LevelProperties.Devil.Pattern, LevelProperties.Devil.States>
	{
		public class SplitDevilWall : AbstractLevelPropertyGroup
		{
			public SplitDevilWall(MinMax xRange, MinMax speed, MinMax hesitateAfterAttack)
			{
			}

		}

		public class SplitDevilProjectiles : AbstractLevelPropertyGroup
		{
			public SplitDevilProjectiles(MinMax numProjectiles, MinMax delayBetweenProjectiles, float projectileSpeed, MinMax hesitateAfterAttack)
			{
			}

		}

		public class Demons : AbstractLevelPropertyGroup
		{
			public Demons(float hp, float speed, float delay)
			{
			}

		}

		public class Clap : AbstractLevelPropertyGroup
		{
			public Clap(MinMax delay, float warning, float speed, float hesitate)
			{
			}

		}

		public class Spider : AbstractLevelPropertyGroup
		{
			public Spider(float downSpeed, float upSpeed, string positionOffset, MinMax numAttacks, MinMax entranceDelay, float hesitate)
			{
			}

		}

		public class Dragon : AbstractLevelPropertyGroup
		{
			public Dragon(float speed, float sinHeight, float sinSpeed, string positionOffset, float returnSpeed, float returnDelay, float hesitate)
			{
			}

		}

		public class Pitchfork : AbstractLevelPropertyGroup
		{
			public Pitchfork(string[] patternString, float spawnCenterY, float spawnRadius, float dormantDuration)
			{
			}

		}

		public class PitchforkTwoFlameWheel : AbstractLevelPropertyGroup
		{
			public PitchforkTwoFlameWheel(string angleOffset, float rotationSpeed, float movementSpeed, MinMax initialtAttackDelay, MinMax secondAttackDelay, float hesitate)
			{
			}

		}

		public class PitchforkThreeFlameJumper : AbstractLevelPropertyGroup
		{
			public PitchforkThreeFlameJumper(string angleOffset, MinMax launchAngle, MinMax launchSpeed, float gravity, MinMax initialAttackDelay, float jumpDelay, int numJumps, float hesitate)
			{
			}

		}

		public class PitchforkFourFlameBouncer : AbstractLevelPropertyGroup
		{
			public PitchforkFourFlameBouncer(string angleOffset, MinMax initialAttackDelay, float speed, int numBounces, float hesitate)
			{
			}

		}

		public class PitchforkFiveFlameSpinner : AbstractLevelPropertyGroup
		{
			public PitchforkFiveFlameSpinner(string angleOffset, float rotationSpeed, float maxSpeed, float acceleration, float attackDuration, float hesitate)
			{
			}

		}

		public class PitchforkSixFlameRing : AbstractLevelPropertyGroup
		{
			public PitchforkSixFlameRing(string angleOffset, MinMax initialAttackDelay, float attackDelay, float speed, float groundDuration, float hesitate)
			{
			}

		}

		public class GiantHeadPlatforms : AbstractLevelPropertyGroup
		{
			public GiantHeadPlatforms(float exitSpeed, float riseSpeed, string riseString, float maxHeight, float holdDelay, MinMax riseDelayRange, float size, bool riseDuringTearPhase)
			{
			}

		}

		public class Fireballs : AbstractLevelPropertyGroup
		{
			public Fireballs(float initialDelay, float fallSpeed, float fallAcceleration, float spawnDelay, float size)
			{
			}

		}

		public class BombEye : AbstractLevelPropertyGroup
		{
			public BombEye(float xSinHeight, float ySinHeight, float xSinSpeed, float ySinSpeed, float explodeDelay, MinMax hesitate)
			{
			}

		}

		public class SkullEye : AbstractLevelPropertyGroup
		{
			public SkullEye(float initialMoveDuration, float initialMoveSpeed, float swirlMoveOutwardSpeed, float swirlRotationSpeed, MinMax hesitate)
			{
			}

		}

		public class Hands : AbstractLevelPropertyGroup
		{
			public Hands(float HP, MinMax yRange, float speed, MinMax shotDelay, float bulletSpeed, MinMax initialSpawnDelay, MinMax spawnDelayRange, string pinkString)
			{
			}

		}

		public class Swoopers : AbstractLevelPropertyGroup
		{
			public Swoopers(string positions, MinMax spawnCount, int maxCount, float hp, MinMax attackDelay, MinMax launchAngle, MinMax launchSpeed, float gravity, MinMax initialSpawnDelay, MinMax spawnDelay, MinMax yIdlePos)
			{
			}

		}

		public class Tears : AbstractLevelPropertyGroup
		{
			public Tears(float speed, float delay)
			{
			}

		}

		public class Firewall : AbstractLevelPropertyGroup
		{
			public Firewall(float firewallSpeed)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Devil.Pattern, LevelProperties.Devil.States>
		{
			public State(float healthTrigger, LevelProperties.Devil.Pattern[][] patterns, LevelProperties.Devil.States stateName, LevelProperties.Devil.SplitDevilWall splitDevilWall, LevelProperties.Devil.SplitDevilProjectiles splitDevilProjectiles, LevelProperties.Devil.Demons demons, LevelProperties.Devil.Clap clap, LevelProperties.Devil.Spider spider, LevelProperties.Devil.Dragon dragon, LevelProperties.Devil.Pitchfork pitchfork, LevelProperties.Devil.PitchforkTwoFlameWheel pitchforkTwoFlameWheel, LevelProperties.Devil.PitchforkThreeFlameJumper pitchforkThreeFlameJumper, LevelProperties.Devil.PitchforkFourFlameBouncer pitchforkFourFlameBouncer, LevelProperties.Devil.PitchforkFiveFlameSpinner pitchforkFiveFlameSpinner, LevelProperties.Devil.PitchforkSixFlameRing pitchforkSixFlameRing, LevelProperties.Devil.GiantHeadPlatforms giantHeadPlatforms, LevelProperties.Devil.Fireballs fireballs, LevelProperties.Devil.BombEye bombEye, LevelProperties.Devil.SkullEye skullEye, LevelProperties.Devil.Hands hands, LevelProperties.Devil.Swoopers swoopers, LevelProperties.Devil.Tears tears, LevelProperties.Devil.Firewall firewall)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			SplitDevilProjectileAttack = 1,
			SplitDevilWallAttack = 2,
			Clap = 3,
			Head = 4,
			Pitchfork = 5,
			BombEye = 6,
			SkullEye = 7,
			Uninitialized = 8,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Split = 2,
			GiantHead = 3,
			Hands = 4,
			Tears = 5,
		}

		public Devil(int hp, Level.GoalTimes goalTimes, LevelProperties.Devil.State[] states)
		{
		}

	}

	public class ChessRook : AbstractLevelProperties<LevelProperties.ChessRook.State, LevelProperties.ChessRook.Pattern, LevelProperties.ChessRook.States>
	{
		public class PinkCannonBall : AbstractLevelPropertyGroup
		{
			public PinkCannonBall(float bounceUpApexHeight, float bounceUpTargetDist, float bounceCollisionOffTimer, MinMax distanceAddition, MinMax heightAddition, MinMax distanceAdditionBack, MinMax heightAdditionBack, float pinkReactionGravity, float velocityAfterSlam, float gravityAfterSlam, float goodQuadrantClemencyLeft, float goodQuadrantClemencyBottom, string[] pinkShotDelayString, string[] pinkShotApexHeightString, string[] pinkShotTargetString, float explosionRadius, float explosionDuration, float pinkGravity)
			{
			}

		}

		public class RegularCannonBall : AbstractLevelPropertyGroup
		{
			public RegularCannonBall(string[] cannonDelayString, string[] cannonApexHeightString, string[] cannonTargetString, float cannonGravity)
			{
			}

		}

		public class StraightShooters : AbstractLevelPropertyGroup
		{
			public StraightShooters(bool straightShotOn, string[] straightShotDelayString, string[] straightShotSeqString, float straightShotBulletSpeed)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.ChessRook.Pattern, LevelProperties.ChessRook.States>
		{
			public State(float healthTrigger, LevelProperties.ChessRook.Pattern[][] patterns, LevelProperties.ChessRook.States stateName, LevelProperties.ChessRook.PinkCannonBall pinkCannonBall, LevelProperties.ChessRook.RegularCannonBall regularCannonBall, LevelProperties.ChessRook.StraightShooters straightShooters)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			PhaseTwo = 2,
			PhaseThree = 3,
			PhaseFour = 4,
			PhaseFive = 5,
			PhaseSix = 6,
		}

		public ChessRook(int hp, Level.GoalTimes goalTimes, LevelProperties.ChessRook.State[] states)
		{
		}

	}

	public class TowerOfPower : AbstractLevelProperties<LevelProperties.TowerOfPower.State, LevelProperties.TowerOfPower.Pattern, LevelProperties.TowerOfPower.States>
	{
		public class BossesPropertises : AbstractLevelPropertyGroup
		{
			public BossesPropertises(string ShmupCountString, string ShmupPlacementString, int KingDiceMiniBossCount, bool DevilFinalBoss, string MiniBossDifficultyByIndex, string PoolOneString, string PoolTwoString, string PoolThreeString, string ShmupPoolOneString, string ShmupPoolTwoString, string ShmupPoolThreeString, string KingDicePoolOneString, string KingDicePoolTwoString, string KingDicePoolThreeString, string KingDicePoolFourString)
			{
			}

		}

		public class SlotMachine : AbstractLevelPropertyGroup
		{
			public SlotMachine(int DefaultStartingToken, int MinRankToGainToken, string SlotOneWeapon, string SlotTwoWeapon, string SlotThreeCharm, string SlotFourSuper, string SlotThreeChalice, string SlotFourChalice)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.TowerOfPower.Pattern, LevelProperties.TowerOfPower.States>
		{
			public State(float healthTrigger, LevelProperties.TowerOfPower.Pattern[][] patterns, LevelProperties.TowerOfPower.States stateName, LevelProperties.TowerOfPower.BossesPropertises bossesPropertises, LevelProperties.TowerOfPower.SlotMachine slotMachine)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public TowerOfPower(int hp, Level.GoalTimes goalTimes, LevelProperties.TowerOfPower.State[] states)
		{
		}

	}

	public class Graveyard : AbstractLevelProperties<LevelProperties.Graveyard.State, LevelProperties.Graveyard.Pattern, LevelProperties.Graveyard.States>
	{
		public class SplitDevilBeam : AbstractLevelPropertyGroup
		{
			public SplitDevilBeam(MinMax speed, MinMax hesitateAfterAttack, float yPos, string attacksBeforeBeamString, float warning)
			{
			}

		}

		public class SplitDevilProjectiles : AbstractLevelPropertyGroup
		{
			public SplitDevilProjectiles(string[] numProjectiles, MinMax delayBetweenProjectiles, float projectileSpeed, MinMax hesitateAfterAttack, string angleOffsetString, string pinkString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.Graveyard.Pattern, LevelProperties.Graveyard.States>
		{
			public State(float healthTrigger, LevelProperties.Graveyard.Pattern[][] patterns, LevelProperties.Graveyard.States stateName, LevelProperties.Graveyard.SplitDevilBeam splitDevilBeam, LevelProperties.Graveyard.SplitDevilProjectiles splitDevilProjectiles)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public Graveyard(int hp, Level.GoalTimes goalTimes, LevelProperties.Graveyard.State[] states)
		{
		}

	}

	public class FlyingCowboy : AbstractLevelProperties<LevelProperties.FlyingCowboy.State, LevelProperties.FlyingCowboy.Pattern, LevelProperties.FlyingCowboy.States>
	{
		public class Cart : AbstractLevelPropertyGroup
		{
			public Cart(string[] cartAttackString, float cartMoveSpeed, float cartPopinTime)
			{
			}

		}

		public class SnakeAttack : AbstractLevelPropertyGroup
		{
			public SnakeAttack(float breakLinePosition, string[] snakeOffsetString, string[] snakeWidthString, float attackDelay, float snakeSpeed, string[] shotsPerAttack, float attackRecovery, float timeToApex, float apexHeight)
			{
			}

		}

		public class BeamAttack : AbstractLevelPropertyGroup
		{
			public BeamAttack(float beamWarningTime, float beamDuration, float attackRecovery)
			{
			}

		}

		public class UFOEnemy : AbstractLevelPropertyGroup
		{
			public UFOEnemy(float UFOHealth, float introUFOSpeed, float topUFOSpeed, float topUFOVerticalPosition, string[] topUFOShootString, float ufoPathLength, float topUFORespawnDelay, int bulletCount, float spreadAngle, float bulletSpeed, string bulletParryString)
			{
			}

		}

		public class BackshotEnemy : AbstractLevelPropertyGroup
		{
			public BackshotEnemy(float enemySpeed, float enemyHealth, float bulletSpeed, string[] highSpawnPosition, string[] lowSpawnPosition, string[] spawnDelay, string bulletParryString, string[] anticipationStartDistance)
			{
			}

		}

		public class Debris : AbstractLevelPropertyGroup
		{
			public Debris(MinMax warningDelayRange, MinMax debrisOneSpeedStartEnd, MinMax debrisTwoSpeedStartEnd, MinMax debrisThreeSpeedStartEnd, float debrisSpeedUpDistance, string[] debrisSideSpawn, string[] debrisTopSpawn, string[] debrisBottomSpawn, string debrisTypeString, float debrisDelay, float hesitate, string[] debrisCurveShotString, float debrisCurveApexTime, float vacuumWindStrength, float vacuumTimeToFullStrength, string debrisParryString, string[] transitionSideSpawn, string[] transitionTopSpawn, string[] transitionBottomSpawn, string[] transitionCurveShotString)
			{
			}

		}

		public class Can : AbstractLevelPropertyGroup
		{
			public Can(string[] sausageStringA, string[] sausageStringB, string[] gapDistA, string[] gapDistB, float sausageTrainSpeed, float sausageSweepSpeed, float maxSausageAngle, bool shootBullets, float shotDelay, string[] bulletCount, float bulletSpeed, float bulletSpreadAngle, string bulletParryString, float beanCanTriggerTime, MinMax beanCanSpeed, string[] beanCanPostionUpper, string[] beanCanPositionLower, string beanCanExtendTimer, float wobbleRadiusX, float wobbleRadiusY, float wobbleDurationX, float wobbleDurationY)
			{
			}

		}

		public class SausageRun : AbstractLevelPropertyGroup
		{
			public SausageRun(float mirrorTime, string[] timeTillSwitch, MinMax beansSpeed, string[] beansPositionString, MinMax beansSpawnDelay, string[] groupBeansDelayString, string beansExtendTimer, bool shootBullets, float bulletDelay, float bulletSpeed, float bulletRotationSpeed, float bulletRotationRadius, float bulletTopMaxUpAngle, float bulletTopMaxDownAngle, bool bulletTopRotateClockwise, float bulletBottomMaxUpAngle, float bulletBottomMaxDownAngle, bool bulletBottomRotateClockwise, string bulletParry)
			{
			}

		}

		public class Ricochet : AbstractLevelPropertyGroup
		{
			public Ricochet(bool useRicochet, string rainDelayString, string rainSpeedString, string rainTypeString, string rainSpawnString, float rainDuration, float rainRecoveryTime, int splitBulletCount, float splitSpreadAngle, float splitBulletSpeed, string splitParryString, MinMax coinCountRange, MinMax coinHeightRange, float coinGravity, MinMax coinSpeedXRange)
			{
			}

		}

		public class Bird : AbstractLevelPropertyGroup
		{
			public Bird(MinMax spawnDelayRange, float speed, float bulletArcHeight, float bulletGravity, string[] bulletLandingPosition, float shrapnelSpeed, int shrapnelCount, float shrapnelSpreadAngle, float shrapnelSecondStageDelay, float safetyZoneMaxDuration)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.FlyingCowboy.Pattern, LevelProperties.FlyingCowboy.States>
		{
			public State(float healthTrigger, LevelProperties.FlyingCowboy.Pattern[][] patterns, LevelProperties.FlyingCowboy.States stateName, LevelProperties.FlyingCowboy.Cart cart, LevelProperties.FlyingCowboy.SnakeAttack snakeAttack, LevelProperties.FlyingCowboy.BeamAttack beamAttack, LevelProperties.FlyingCowboy.UFOEnemy uFOEnemy, LevelProperties.FlyingCowboy.BackshotEnemy backshotEnemy, LevelProperties.FlyingCowboy.Debris debris, LevelProperties.FlyingCowboy.Can can, LevelProperties.FlyingCowboy.SausageRun sausageRun, LevelProperties.FlyingCowboy.Ricochet ricochet, LevelProperties.FlyingCowboy.Bird bird)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Vacuum = 1,
			Ricochet = 2,
			Uninitialized = 3,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
			Vacuum = 2,
			Sausage = 3,
			Meatball = 4,
			TBone = 5,
		}

		public FlyingCowboy(int hp, Level.GoalTimes goalTimes, LevelProperties.FlyingCowboy.State[] states)
		{
		}

	}

	public class DicePalaceCigar : AbstractLevelProperties<LevelProperties.DicePalaceCigar.State, LevelProperties.DicePalaceCigar.Pattern, LevelProperties.DicePalaceCigar.States>
	{
		public class Cigar : AbstractLevelPropertyGroup
		{
			public Cigar(float warningDelay, float platformWidthMultiplier, float platformHeight)
			{
			}

		}

		public class SpiralSmoke : AbstractLevelPropertyGroup
		{
			public SpiralSmoke(float horizontalSpeed, float circleSpeed, string rotationDirectionString, float attackDelay, string attackCount, float spiralSmokeCircleSize, float hesitateBeforeAttackDelay)
			{
			}

		}

		public class CigaretteGhost : AbstractLevelPropertyGroup
		{
			public CigaretteGhost(float verticalSpeed, float horizontalSpeed, string attackDelayString, float horizontalSpacing, string spawnPositionString)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceCigar.Pattern, LevelProperties.DicePalaceCigar.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceCigar.Pattern[][] patterns, LevelProperties.DicePalaceCigar.States stateName, LevelProperties.DicePalaceCigar.Cigar cigar, LevelProperties.DicePalaceCigar.SpiralSmoke spiralSmoke, LevelProperties.DicePalaceCigar.CigaretteGhost cigaretteGhost)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceCigar(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceCigar.State[] states)
		{
		}

	}

	public class DicePalaceMain : AbstractLevelProperties<LevelProperties.DicePalaceMain.State, LevelProperties.DicePalaceMain.Pattern, LevelProperties.DicePalaceMain.States>
	{
		public class Dice : AbstractLevelPropertyGroup
		{
			public Dice(float movementSpeed, float diceStartPositionOneX, float diceStartPositionOneY, float diceStartPositionTwoX, float diceStartPositionTwoY, float rollFrameCount, float pauseWhenRolled, float revealDelay)
			{
			}

		}

		public class Cards : AbstractLevelPropertyGroup
		{
			public Cards(float cardSpeed, string[] cardString, string[] cardSideOrder, float hesitate, float cardScale, float cardDelay)
			{
			}

		}

		public class State : AbstractLevelState<LevelProperties.DicePalaceMain.Pattern, LevelProperties.DicePalaceMain.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceMain.Pattern[][] patterns, LevelProperties.DicePalaceMain.States stateName, LevelProperties.DicePalaceMain.Dice dice, LevelProperties.DicePalaceMain.Cards cards)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceMain(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceMain.State[] states)
		{
		}

	}

	public class DicePalaceTest : AbstractLevelProperties<LevelProperties.DicePalaceTest.State, LevelProperties.DicePalaceTest.Pattern, LevelProperties.DicePalaceTest.States>
	{
		public class State : AbstractLevelState<LevelProperties.DicePalaceTest.Pattern, LevelProperties.DicePalaceTest.States>
		{
			public State(float healthTrigger, LevelProperties.DicePalaceTest.Pattern[][] patterns, LevelProperties.DicePalaceTest.States stateName)
			{
			}

		}

		public class Entity : AbstractLevelEntity
		{
		}

		public enum Pattern
		{
			Default = 0,
			Uninitialized = 1,
		}

		public enum States
		{
			Main = 0,
			Generic = 1,
		}

		public DicePalaceTest(int hp, Level.GoalTimes goalTimes, LevelProperties.DicePalaceTest.State[] states)
		{
		}

	}

}
