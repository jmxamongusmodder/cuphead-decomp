using UnityEngine;

public class DicePalaceDominoLevelFloor : AbstractCollidableObject
{
	[SerializeField]
	private DicePalaceDominoLevelScrollingFloor[] _floors;
	[SerializeField]
	private ScrollingSprite _teethSprite;
}
