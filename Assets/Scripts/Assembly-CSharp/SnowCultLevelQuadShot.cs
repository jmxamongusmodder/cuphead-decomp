using UnityEngine;

public class SnowCultLevelQuadShot : AbstractProjectile
{
	[SerializeField]
	private float popOutWarningTimeNormalized;
	[SerializeField]
	private Effect sparkEffect;
	[SerializeField]
	private Effect snowLandEffect;
	[SerializeField]
	private Effect snowPopOutEffect;
	[SerializeField]
	private SpriteRenderer deathPuff;
	[SerializeField]
	private SpriteRenderer rend;
}
