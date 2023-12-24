using UnityEngine;

public class SnowCultLevelBat : AbstractProjectile
{
	public bool reachedCircle;
	public bool moving;
	[SerializeField]
	private SnowCultLevelBatEffect explosionPrefab;
	[SerializeField]
	private SnowCultLevelBatEffect dripPrefab;
	[SerializeField]
	private Collider2D collider;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
}
