using UnityEngine;

public class SplitScrollingSprite : ScrollingSprite
{
	[SerializeField]
	private bool ignoreSelfWhenHandlingSplitSprites;
	[SerializeField]
	private Vector2 splitOffset;
	[SerializeField]
	private Sprite[] splitSprites;
}
