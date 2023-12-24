using UnityEngine;

public class FlyingMermaidLevelMerdusaBodyPart : LevelProperties.FlyingMermaid.Entity
{
	[SerializeField]
	private float waitTime;
	[SerializeField]
	private Vector2 velocity;
	[SerializeField]
	private float moveTime;
	[SerializeField]
	private bool stopBobbingAfterWait;
	[SerializeField]
	private float rotationSpeed;
	[SerializeField]
	private bool damagePlayer;
}
