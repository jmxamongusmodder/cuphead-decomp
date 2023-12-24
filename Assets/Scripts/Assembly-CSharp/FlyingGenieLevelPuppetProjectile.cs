using UnityEngine;

public class FlyingGenieLevelPuppetProjectile : BasicProjectile
{
	[SerializeField]
	private float _minRadius;
	[SerializeField]
	private float _maxRadius;
	[SerializeField]
	private Effect[] sparksBlue;
	[SerializeField]
	private Effect[] sparksPink;
}
