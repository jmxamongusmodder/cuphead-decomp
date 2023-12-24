using UnityEngine;

public class MapNPCLostBarbershop : AbstractMapInteractiveEntity
{
	[SerializeField]
	private string triggerShow;
	[SerializeField]
	private string triggerHide;
	[SerializeField]
	private MapNPCBarbershop[] mapNPCBarbershops;
	[SerializeField]
	private int dialoguerVariableID;
	public bool SkipDialogueEvent;
}
