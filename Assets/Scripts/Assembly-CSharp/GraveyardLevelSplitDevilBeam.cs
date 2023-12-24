using UnityEngine;

public class GraveyardLevelSplitDevilBeam : AbstractProjectile
{
	[SerializeField]
	private Animator fireAnim;
	[SerializeField]
	private Animator lightAnim;
	[SerializeField]
	private SpriteRenderer[] fireRend;
	[SerializeField]
	private SpriteRenderer[] lightRend;
	[SerializeField]
	private SpriteRenderer fireFormDissipate;
	[SerializeField]
	private Effect bottomSmokeFX;
	[SerializeField]
	private Effect midSmokeFX;
	[SerializeField]
	private Transform midSmokePos;
	[SerializeField]
	private GraveyardLevelSplitDevilBeamIgniteFX igniteFX;
	[SerializeField]
	private GraveyardLevelSplitDevilBeamTrailFX trailFX;
	[SerializeField]
	private float flameTrailSpacing;
	[SerializeField]
	private GameObject sparkleBeam;
	[SerializeField]
	private SpriteRenderer groundSpotlight;
	[SerializeField]
	private Collider2D coll;
}
