using UnityEngine;

public class FlyingCowboyLevelBackground : AbstractPausableComponent
{
	[SerializeField]
	private float sunsetDuration;
	[SerializeField]
	private float sunsetTargetY;
	[SerializeField]
	private Transform skyLoopTransform;
	[SerializeField]
	private FlyingCowboyLevelOverlayScrollingSprite initialScrollingMidLayer;
	[SerializeField]
	private ScrollingSpriteSpawner[] initialFGSpawners;
	[SerializeField]
	private GameObject transitionBackground;
	[SerializeField]
	private GameObject phase3Background;
	[SerializeField]
	private ScrollingSprite phase3Scrolling;
	[SerializeField]
	private ScrollingSpriteSpawner[] phase3MidSpawners;
	[SerializeField]
	private GameObject phase3Foreground;
	[SerializeField]
	private GameObject phase3ForegroundStart;
	[SerializeField]
	private ScrollingSprite phase3ForegroundScrolling;
	[SerializeField]
	private ScrollingSpriteSpawner[] phase3FGSpawners;
}
