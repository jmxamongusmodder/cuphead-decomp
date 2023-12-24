using UnityEngine;

public class HomingProjectile : AbstractProjectile
{
	[SerializeField]
	private bool trackGround;
	[SerializeField]
	private bool faceMoveDirection;
	[SerializeField]
	private float spriteRotation;
}
