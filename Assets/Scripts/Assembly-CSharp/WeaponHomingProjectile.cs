using UnityEngine;

public class WeaponHomingProjectile : AbstractProjectile
{
	[SerializeField]
	private float spriteRotation;
	[SerializeField]
	private float trailSpriteRotation;
	[SerializeField]
	private Transform trail;
	[SerializeField]
	private float destroyPadding;
	public float speed;
	public MinMax rotationSpeed;
	public float timeBeforeEaseRotationSpeed;
	public float rotationSpeedEaseTime;
	public float rotation;
	public float swirlDistance;
	public float swirlEaseTime;
	public int trailFollowFrames;
	public bool isEx;
}
