using UnityEngine;

public class EffectRadius : AbstractPausableComponent
{
	[SerializeField]
	private Effect effect;
	[SerializeField]
	private float _radius;
	[SerializeField]
	private Vector2 _offset;
}
