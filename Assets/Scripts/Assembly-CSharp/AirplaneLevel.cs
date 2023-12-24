using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class AirplaneLevel : Level
{
	[SerializeField]
	private AirplaneLevelPlayerPlane airplane;
	[SerializeField]
	private AirplaneLevelCanteenAnimator canteenAnimator;
	[SerializeField]
	private AirplaneLevelBulldogPlane bulldogPlane;
	[SerializeField]
	private AirplaneLevelBulldogParachute bulldogParachute;
	[SerializeField]
	private AirplaneLevelBulldogCatAttack bulldogCatAttack;
	[SerializeField]
	public AirplaneLevelLeader leader;
	[SerializeField]
	private Animator secretIntro;
	[SerializeField]
	private Animator leaderAnimator;
	[SerializeField]
	private AirplaneLevelSecretLeader secretLeader;
	[SerializeField]
	private AirplaneLevelSecretTerrier[] secretTerriers;
	[SerializeField]
	private AnimationClip rotateClip;
	[SerializeField]
	private LevelPit pitTop;
	[SerializeField]
	private Transform terrierPivotPoint;
	[SerializeField]
	private Transform[] secretPhaseTerrierPositions;
	[SerializeField]
	private Transform[] secretPhaseLeaderPositions;
	[SerializeField]
	private Transform[] leaderDeathPositions;
	[SerializeField]
	private BlurOptimized bgBlur;
	[SerializeField]
	private Camera bgCamera;
	[SerializeField]
	private AirplaneLevelTerrier terrierPrefab;
	public bool terriersIntroFinished;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;
	[SerializeField]
	private Sprite _bossPortraitPhaseSecret;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuotePhaseTwo;
	[SerializeField]
	private string _bossQuotePhaseThree;
	[SerializeField]
	private AirplaneLevelTerrierSmokeFX smokePrefab;
}
