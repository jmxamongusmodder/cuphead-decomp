using UnityEngine;

public class TrainLevelBlindSpecterEyeProjectile : AbstractProjectile
{
	[SerializeField]
	private Effect effectPrefab;
	[SerializeField]
	private Transform sprite;
	[SerializeField]
	private Collider2D eyeCollider;
}
