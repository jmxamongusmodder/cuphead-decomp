using UnityEngine;

public class ClownLevelAnimationManager : AbstractPausableComponent
{
	[SerializeField]
	private Animator headSprite;
	[SerializeField]
	private Transform balloonSprite;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Animator[] twelveFpsAnimations;
	[SerializeField]
	private Animator[] twentyFourFpsAnimations;
}
