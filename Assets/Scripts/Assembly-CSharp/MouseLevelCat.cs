using UnityEngine;

public class MouseLevelCat : LevelProperties.Mouse.Entity
{
	[SerializeField]
	private Vector2 startPosition;
	[SerializeField]
	private MouseLevelBrokenCanMouse mouse;
	[SerializeField]
	private Animator wallAnimator;
	[SerializeField]
	private LevelPlatform wallPlatform;
	[SerializeField]
	private GameObject foreground;
	[SerializeField]
	private GameObject alternateForeground;
	[SerializeField]
	private GameObject[] toDestroyOnWallBreakStart;
	[SerializeField]
	private GameObject[] toDestroyOnWallBreakEnd;
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;
	[SerializeField]
	private Transform head;
	[SerializeField]
	private MouseLevelCatPaw leftPaw;
	[SerializeField]
	private MouseLevelCatPaw rightPaw;
	[SerializeField]
	private Transform headMoveTransform;
	[SerializeField]
	private MouseLevelFallingObject[] fallingObjectPrefabs;
	[SerializeField]
	private MouseLevelGhostMouse[] twoGhostMice;
	[SerializeField]
	private MouseLevelGhostMouse[] fourGhostMice;
	[SerializeField]
	private SpriteRenderer headFrontRenderer;
}
