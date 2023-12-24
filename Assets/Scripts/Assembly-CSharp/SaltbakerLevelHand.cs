using UnityEngine;

public class SaltbakerLevelHand : AbstractCollidableObject
{
	[SerializeField]
	private BasicProjectile projectile;
	[SerializeField]
	private Transform root;
	[SerializeField]
	private bool leftHand;
}
