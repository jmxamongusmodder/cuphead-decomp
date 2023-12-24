using UnityEngine;

public class PlayerSuperGhost : AbstractPlayerSuper
{
	[SerializeField]
	private PlayerSuperGhostHeart heartPrefab;
	[SerializeField]
	private Transform heartRoot;
	[SerializeField]
	private SpriteRenderer cupheadBottom;
	[SerializeField]
	private SpriteRenderer mugmanBottom;
}
