using UnityEngine;

public class FlyingGenieLevelObelisk : AbstractCollidableObject
{
	[SerializeField]
	private GameObject baseA;
	[SerializeField]
	private GameObject baseB;
	[SerializeField]
	private BoxCollider2D bouncerWall;
	[SerializeField]
	private BoxCollider2D ceiling;
	[SerializeField]
	private BoxCollider2D ground;
	[SerializeField]
	private FlyingGenieLevelGenieHead genieHead;
	[SerializeField]
	private FlyingGenieLevelObeliskBlock obeliskBlock;
}
