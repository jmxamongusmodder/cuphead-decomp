using UnityEngine;

public class LevelPlayerAnimationController : AbstractLevelPlayerComponent
{
	[SerializeField]
	private GameObject cuphead;
	[SerializeField]
	private GameObject mugman;
	[SerializeField]
	private GameObject chalice;
	[SerializeField]
	private SpriteRenderer[] chaliceSprites;
	[SerializeField]
	private Transform runDustRoot;
	[SerializeField]
	private Transform sparkleRoot;
	[SerializeField]
	private Effect dashEffect;
	[SerializeField]
	private Effect groundedEffect;
	[SerializeField]
	private Effect hitEffect;
	[SerializeField]
	private Effect runEffect;
	[SerializeField]
	private Effect curseEffect;
	[SerializeField]
	private Effect smokeDashEffect;
	[SerializeField]
	private HealerCharmSparkEffect healerCharmEffect;
	[SerializeField]
	private Effect powerUpBurstEffect;
	[SerializeField]
	private Effect chaliceDoubleJumpEffect;
	[SerializeField]
	private Effect chaliceDashEffect;
	[SerializeField]
	private Effect chaliceDashSparkle;
	[SerializeField]
	private SpriteRenderer[] chaliceJumpShootRenderers;
	[SerializeField]
	private Material chaliceDuckDashMaterial;
	[SerializeField]
	private Effect chaliceDuckDashSparkles;
	[SerializeField]
	private LevelPlayerChaliceIntroAnimation chaliceIntroAnimation;
	[SerializeField]
	private Sprite cupheadScaredSprite;
	[SerializeField]
	private Sprite mugmanScaredSprite;
	[SerializeField]
	private float curseEffectDelay;
	[SerializeField]
	private MinMax curseAngleShiftRange;
	[SerializeField]
	private MinMax curseDistanceRange;
	[SerializeField]
	private SpriteRenderer[] paladinShadows;
}
