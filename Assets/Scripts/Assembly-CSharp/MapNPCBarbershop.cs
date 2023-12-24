using UnityEngine;

public class MapNPCBarbershop : AbstractMonoBehaviour
{
	[SerializeField]
	private RuntimeAnimatorController fourAnimatorController;
	[SerializeField]
	private Vector3 fourPosition;
	[SerializeField]
	protected MapNPCLostBarbershop mapNPCDistanceAnimator;
	public MapDialogueInteraction mapDialogueInteraction;
	[SerializeField]
	private int dialoguerVariableID;
}
