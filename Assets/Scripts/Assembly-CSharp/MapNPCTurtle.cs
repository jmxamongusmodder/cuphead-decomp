using UnityEngine;

public class MapNPCTurtle : MapDialogueInteraction
{
	[SerializeField]
	private BoxCollider2D colliderB;
	[SerializeField]
	private int dialoguerVariableID;
	public bool SkipDialogueEvent;
}
