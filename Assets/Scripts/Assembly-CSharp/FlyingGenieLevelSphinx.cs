using UnityEngine;

public class FlyingGenieLevelSphinx : AbstractProjectile
{
	[SerializeField]
	private SpriteRenderer sphinxRenderer;
	[SerializeField]
	private float outOfChestY;
	[SerializeField]
	private float outOfChestSpeed;
	public FlyingGenieLevelSphinxPiece[] sphinxPieces;
}
