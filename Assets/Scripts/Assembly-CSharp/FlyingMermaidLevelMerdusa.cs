using UnityEngine;

public class FlyingMermaidLevelMerdusa : LevelProperties.FlyingMermaid.Entity
{
	[SerializeField]
	private float introMoveTime;
	[SerializeField]
	private float transformMoveX;
	[SerializeField]
	private SpriteRenderer blinkOverlaySprite;
	[SerializeField]
	private Transform blockingColliders;
	[SerializeField]
	private FlyingMermaidLevelLaser laser;
	[SerializeField]
	private FlyingMermaidLevelEel[] eels;
	[SerializeField]
	private FlyingMermaidLevelMerdusaHead head;
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart bodyPrefab;
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart leftArmPrefab;
	[SerializeField]
	private FlyingMermaidLevelMerdusaBodyPart rightArmPrefab;
	[SerializeField]
	private Transform headRoot;
	[SerializeField]
	private Transform bodyRoot;
	[SerializeField]
	private Transform leftArmRoot;
	[SerializeField]
	private Transform rightArmRoot;
	[SerializeField]
	private Effect splashLeft;
	[SerializeField]
	private Effect splashRight;
	[SerializeField]
	private Transform splashRoot;
}
