using UnityEngine;

public class CollisionChild : AbstractCollidableObject
{
	[SerializeField]
	private AbstractCollidableObject collisionParent;
	[SerializeField]
	private bool forwardParry;
}
