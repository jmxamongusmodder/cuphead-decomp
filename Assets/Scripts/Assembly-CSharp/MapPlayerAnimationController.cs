using UnityEngine;

public class MapPlayerAnimationController : AbstractMapPlayerComponent
{
	public bool facingUpwards;
	[SerializeField]
	private SpriteRenderer Cuphead;
	[SerializeField]
	private SpriteRenderer Mugman;
	[SerializeField]
	private SpriteRenderer Chalice;
	[SerializeField]
	private SpriteRenderer[] ghostInPortal;
	[SerializeField]
	private SpriteRenderer portal;
	[SerializeField]
	private MapPlayerDust dustEffect;
}
