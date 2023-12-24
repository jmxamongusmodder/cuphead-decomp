using UnityEngine;

public class ArcadePlayerAnimationController : AbstractArcadePlayerComponent
{
	[SerializeField]
	private GameObject prong;
	[SerializeField]
	private GameObject cuphead;
	[SerializeField]
	private GameObject mugman;
	[SerializeField]
	private GameObject cupheadArm;
	[SerializeField]
	private GameObject mugmanArm;
	[SerializeField]
	private Transform runDustRoot;
	[SerializeField]
	private Effect dashEffect;
	[SerializeField]
	private Effect groundedEffect;
	[SerializeField]
	private Effect hitEffect;
	[SerializeField]
	private Effect runEffect;
}
