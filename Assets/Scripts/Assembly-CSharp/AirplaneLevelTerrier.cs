using UnityEngine;

public class AirplaneLevelTerrier : AbstractCollidableObject
{
	[SerializeField]
	private BoxCollider2D coll;
	[SerializeField]
	private AirplaneLevelTerrierBullet regularProjectile;
	[SerializeField]
	private AirplaneLevelTerrierBullet pinkProjectile;
	[SerializeField]
	private GameObject[] terrierLayers;
	[SerializeField]
	private SpriteRenderer deathRenderer;
	[SerializeField]
	private SpriteRenderer[] rends;
	[SerializeField]
	private Vector3[] flameOffset;
	[SerializeField]
	private SpriteRenderer flame;
	[SerializeField]
	private Animator flameAnimator;
	[SerializeField]
	private SpriteRenderer barkFXRenderer;
	[SerializeField]
	private Animator barkFXAnimator;
	public float angle;
	[SerializeField]
	private float wobbleX;
	[SerializeField]
	private float wobbleY;
	[SerializeField]
	private float wobbleSpeed;
	public bool lastOne;
}
