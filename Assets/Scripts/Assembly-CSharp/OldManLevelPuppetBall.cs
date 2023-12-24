using UnityEngine;

public class OldManLevelPuppetBall : AbstractProjectile
{
	[SerializeField]
	private float shadowRange;
	[SerializeField]
	private SpriteRenderer sprite;
	[SerializeField]
	private SpriteRenderer shadowRend;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private Effect coinPrefab;
	[SerializeField]
	private Effect featherPrefab;
}
