using UnityEngine;

public class ChessPawnLevelPawn : AbstractProjectile
{
	[SerializeField]
	private Collider2D collider;
	[SerializeField]
	private SpriteRenderer bodyRenderer;
	[SerializeField]
	private SpriteRenderer headRenderer;
	[SerializeField]
	private SpriteDeathParts parriedHead;
	[SerializeField]
	private float deathTwitchDelayFixed;
	[SerializeField]
	private Rangef deathTwitchDelayRange;
	[SerializeField]
	private float deathFloatUpSpeed;
	[SerializeField]
	private Effect deathSmokeEffect;
	[SerializeField]
	private SpriteDeathParts deathBody;
	[SerializeField]
	private BoxCollider2D noHeadCollider;
}
