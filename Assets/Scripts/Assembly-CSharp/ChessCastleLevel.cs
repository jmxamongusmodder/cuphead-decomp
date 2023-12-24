using UnityEngine;

public class ChessCastleLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private AbstractLevelInteractiveEntity startEntity;
	[SerializeField]
	private AbstractLevelInteractiveEntity exitEntity;
	[SerializeField]
	private PlayerDeathEffect[] playerStartLevelEffects;
	[SerializeField]
	private ChessCastleLevelKingInteractionPoint dialogueInteractionPoint;
	[SerializeField]
	private SpeechBubble speechBubble;
	[SerializeField]
	private Animator castleAnimator;
	[SerializeField]
	private Animator platformAnimator;
	[SerializeField]
	private GameObject cloudPrefab;
	[SerializeField]
	private GameObject coinPrefab;
	[SerializeField]
	private Transform coinSparkSpawnPoint;
	[SerializeField]
	private Animator kingAnimator;
	[SerializeField]
	private Effect sparkleEffect;
	[SerializeField]
	private Transform sparklesCenter;
	[SerializeField]
	private float sinePeriod;
	[SerializeField]
	private float sineAmplitude;
	[SerializeField]
	private float _rotationMultiplier;
	[SerializeField]
	private float introPanAmount;
	[SerializeField]
	private float introPanDuration;
}
