using UnityEngine;

public class DevilLevel : Level
{
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitPhaseTwo;
	[SerializeField]
	private Sprite _bossPortraitPhaseThree;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuotePhaseTwo;
	[SerializeField]
	private string _bossQuotePhaseThree;
	[SerializeField]
	private GameObject groundHandler;
	[SerializeField]
	private ParallaxLayer[] parallax;
	[SerializeField]
	private GameObject pit;
	[SerializeField]
	private GameObject middlePiece;
	[SerializeField]
	private Transform phase1Scroll;
	[SerializeField]
	private SpriteRenderer phase1Foreground;
	[SerializeField]
	private GameObject phase2Background;
	[SerializeField]
	private GameObject phase3Platforms;
	[SerializeField]
	private SpriteRenderer phase1Fade;
	[SerializeField]
	private DevilLevelSittingDevil sittingDevil;
	[SerializeField]
	private DevilLevelGiantHead giantHead;
	[SerializeField]
	private DevilLevelEffectSpawner[] smokeSpawners;
	[SerializeField]
	private Transform Phase2P1spawn;
	[SerializeField]
	private Transform Phase2P2spawn;
}
