using UnityEngine;

public class ChangeLightingZone : AbstractCollidableObject
{
	[SerializeField]
	private Color _minTint;
	[SerializeField]
	private Color _maxTint;
	[SerializeField]
	private BoxCollider2D _collider;
	[SerializeField]
	private float _maxDistance;
}
