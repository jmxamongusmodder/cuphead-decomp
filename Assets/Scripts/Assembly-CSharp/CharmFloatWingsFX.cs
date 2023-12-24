using UnityEngine;

public class CharmFloatWingsFX : Effect
{
	[SerializeField]
	private Animator anim;
	[SerializeField]
	private Vector3 vel;
	[SerializeField]
	private float startSpeedMin;
	[SerializeField]
	private float startSpeedMax;
	[SerializeField]
	private float slowFactor;
	[SerializeField]
	private float riseFactor;
}
