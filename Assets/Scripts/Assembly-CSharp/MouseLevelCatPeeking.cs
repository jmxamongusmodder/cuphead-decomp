using UnityEngine;

public class MouseLevelCatPeeking : MonoBehaviour
{
	[SerializeField]
	private Animator catAnimator;
	[SerializeField]
	private MinMax catDelay;
	[SerializeField]
	private MinMax catRotationRange;
	[SerializeField]
	private float peek1Threshold;
	[SerializeField]
	private float peek2Threshold;
}
