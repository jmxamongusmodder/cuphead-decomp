using UnityEngine;

public class FlyingMermaidLevelMerdusaHead : LevelProperties.FlyingMermaid.Entity
{
	[SerializeField]
	private BasicProjectile yellowDot;
	[SerializeField]
	private SpriteRenderer wave1;
	[SerializeField]
	private SpriteRenderer wave2;
	[SerializeField]
	private ScrollingSpriteSpawner[] scrollingSpritesToEnd;
	[SerializeField]
	private ScrollingSpriteSpawner[] scrollingSprites;
	[SerializeField]
	private FlyingMermaidLevelBackgroundChange coral;
	[SerializeField]
	private Transform snakeRoot;
	[SerializeField]
	private Transform eyebeamRoot;
	[SerializeField]
	private FlyingMermaidLevelSkullBubble bubblePrefab;
	[SerializeField]
	private BasicProjectile heatBlastPrefab;
	[SerializeField]
	private float xPosition;
	[SerializeField]
	private float headBackMoveTime;
}
