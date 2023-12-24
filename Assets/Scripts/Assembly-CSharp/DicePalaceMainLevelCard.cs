using UnityEngine;

public class DicePalaceMainLevelCard : AbstractProjectile
{
	[SerializeField]
	private float coolDown;
	[SerializeField]
	private GameObject chaliceParryableHearts;
	[SerializeField]
	private Animator[] risingHeartAnimator;
	[SerializeField]
	private SpriteRenderer[] risingHeartRenderer;
	[SerializeField]
	private MinMax risingHeartSpawnTimeRange;
}
