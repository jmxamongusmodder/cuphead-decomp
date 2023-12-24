using UnityEngine;

public class HealerCharmParticleEffect : AbstractPausableComponent
{
	[SerializeField]
	private float initialEmissionSpeed;
	[SerializeField]
	private MinMax timeBeforeSeekRange;
	[SerializeField]
	private float timeBeforeCanCollect;
	[SerializeField]
	private float timeBeforeLerp;
	[SerializeField]
	private float maxTime;
	[SerializeField]
	private MinMax accelerationRange;
	[SerializeField]
	private MinMax maxSpeedRange;
	[SerializeField]
	private float contactDistance;
	[SerializeField]
	private float frameTime;
}
