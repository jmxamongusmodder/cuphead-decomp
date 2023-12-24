using UnityEngine;

public class RobotLevelScrollingSprite : ScrollingSpriteSpawner
{
	[SerializeField]
	private SpriteLayer layer;
	[SerializeField]
	private MinMax yOffset;
	[SerializeField]
	private Sprite[] sprites;
}
