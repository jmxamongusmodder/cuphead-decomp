using UnityEngine;

public class GraveyardLevelSplitDevilProjectile : BasicProjectileContinuesOnLevelEnd
{
	[SerializeField]
	private SpriteRenderer[] fireRend;
	[SerializeField]
	private SpriteRenderer[] lightRend;
	[SerializeField]
	private Effect fireFX;
	[SerializeField]
	private Effect lightFX;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private float fxSpawnDelay;
	[SerializeField]
	private MinMax fxAngleShiftRange;
	[SerializeField]
	private MinMax fxDistanceRange;
}
