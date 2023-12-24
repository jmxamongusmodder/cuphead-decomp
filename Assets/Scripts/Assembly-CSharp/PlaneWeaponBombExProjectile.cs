using UnityEngine;

public class PlaneWeaponBombExProjectile : AbstractProjectile
{
	[SerializeField]
	private float spriteRotation;
	[SerializeField]
	private Effect trailFxPrefab;
	[SerializeField]
	private Transform trailFxRoot;
	[SerializeField]
	private float trailFxMaxOffset;
	[SerializeField]
	private float trailDelay;
	[SerializeField]
	private float destroyPadding;
	[SerializeField]
	private SpriteRenderer Cuphead;
	[SerializeField]
	private SpriteRenderer Mugman;
	public float speed;
	public MinMax rotationSpeed;
	public float timeBeforeEaseRotationSpeed;
	public float rotationSpeedEaseTime;
	public float rotation;
}
