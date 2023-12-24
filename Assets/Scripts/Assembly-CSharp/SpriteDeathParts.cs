using UnityEngine;

public class SpriteDeathParts : AbstractCollidableObject
{
	public float bottomOffset;
	public float VelocityXMin;
	public float VelocityXMax;
	public float VelocityYMin;
	public float VelocityYMax;
	public float GRAVITY;
	[SerializeField]
	protected bool clampFallVelocity;
	[SerializeField]
	private bool rotate;
	[SerializeField]
	private Rangef rotationSpeedRange;
}
