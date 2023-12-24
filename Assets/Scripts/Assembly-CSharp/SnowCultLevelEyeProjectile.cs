using UnityEngine;

public class SnowCultLevelEyeProjectile : AbstractProjectile
{
	public bool readyToOpenMouth;
	public bool readyToCloseMouth;
	[SerializeField]
	private Animator beamAnimator;
	[SerializeField]
	private float openMouthDistance;
	[SerializeField]
	private float beamEndDistance;
	[SerializeField]
	private float animatorTakeoverDistance;
	public SnowCultLevelJackFrost main;
	[SerializeField]
	private GameObject shadow;
}
