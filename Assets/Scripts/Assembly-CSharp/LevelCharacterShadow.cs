using UnityEngine;

public class LevelCharacterShadow : AbstractPausableComponent
{
	[SerializeField]
	private int maxDistance;
	[SerializeField]
	private Transform root;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private bool isBGLayer;
}
