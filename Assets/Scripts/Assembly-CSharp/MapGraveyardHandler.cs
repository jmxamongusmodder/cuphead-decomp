using UnityEngine;

public class MapGraveyardHandler : MapDialogueInteraction
{
	[SerializeField]
	private GameObject graveFire;
	[SerializeField]
	private MapGraveyardGrave[] grave;
	[SerializeField]
	private float pressDurationToReEnable;
	[SerializeField]
	private GameObject ghostPrefab;
	[SerializeField]
	private Animator beamAnimator;
}
