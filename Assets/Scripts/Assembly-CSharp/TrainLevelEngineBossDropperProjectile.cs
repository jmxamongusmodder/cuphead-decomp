using UnityEngine;

public class TrainLevelEngineBossDropperProjectile : AbstractProjectile
{
	[SerializeField]
	private Effect dustFX;
	[SerializeField]
	private CircleCollider2D verticalCollider;
	[SerializeField]
	private BoxCollider2D horizontalCollider;
}
