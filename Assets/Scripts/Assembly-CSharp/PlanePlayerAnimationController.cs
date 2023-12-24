using UnityEngine;

public class PlanePlayerAnimationController : AbstractPlanePlayerComponent
{
	[SerializeField]
	private Transform cuphead;
	[SerializeField]
	private Transform mugman;
	[SerializeField]
	private Transform chalice;
	[SerializeField]
	private Transform introRoot;
	[SerializeField]
	private Effect breakoutPrefab;
	[SerializeField]
	private Effect poofPrefab;
	[SerializeField]
	private Effect greenPrefab;
	[SerializeField]
	private PlaneLevelEffect puffPrefab;
	[SerializeField]
	private Effect hitSparkEffect;
	[SerializeField]
	private Effect hitDustEffect;
	[SerializeField]
	private Effect smokeDashEffect;
	[SerializeField]
	private HealerCharmSparkEffect healerCharmEffect;
	[SerializeField]
	private Effect curseEffect;
	[SerializeField]
	private Effect shrinkEffect;
	[SerializeField]
	private Effect growEffect;
	[SerializeField]
	private PlanePlayerDeathPart[] deathPieces;
	[SerializeField]
	private PlaneLevelEffect deathEffect;
	[SerializeField]
	private float curseEffectDelay;
	[SerializeField]
	private MinMax curseAngleShiftRange;
	[SerializeField]
	private MinMax curseDistanceRange;
	[SerializeField]
	private SpriteRenderer[] paladinShadows;
}
