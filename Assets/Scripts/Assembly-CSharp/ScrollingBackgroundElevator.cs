using UnityEngine;

public class ScrollingBackgroundElevator : AbstractPausableComponent
{
	[SerializeField]
	private bool isClouds;
	[SerializeField]
	private bool isBackground;
	[SerializeField]
	private SpriteRenderer firstSprite;
	[SerializeField]
	private SpriteRenderer lastSprite;
	public bool ending;
	public bool easingOut;
}
