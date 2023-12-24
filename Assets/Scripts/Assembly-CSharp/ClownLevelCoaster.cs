using UnityEngine;

public class ClownLevelCoaster : AbstractCollidableObject
{
	[SerializeField]
	private SpriteRenderer knobSprite;
	[SerializeField]
	private Collider2D knobCollider;
	public Transform pieceRoot;
}
