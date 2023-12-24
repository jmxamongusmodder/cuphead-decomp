using UnityEngine;

public class RumRunnersLevelGrub : AbstractProjectile
{
	[SerializeField]
	private float yOffset;
	[SerializeField]
	private SpriteRenderer mainRenderer;
	[SerializeField]
	private SpriteRenderer blinkRenderer;
	[SerializeField]
	private Transform shadowTransform;
	[SerializeField]
	private float wobbleX;
	[SerializeField]
	private float wobbleY;
	[SerializeField]
	private float wobbleSpeed;
	[SerializeField]
	private Effect deathEffect;
}
