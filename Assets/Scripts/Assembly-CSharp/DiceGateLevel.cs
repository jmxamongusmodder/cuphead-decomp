using UnityEngine;
using System.Collections.Generic;

public class DiceGateLevel : Level
{
	[SerializeField]
	private AbstractLevelInteractiveEntity toNextWorld;
	[SerializeField]
	private AbstractLevelInteractiveEntity toPrevWorld;
	public AbstractUIInteractionDialogue.Properties world2PrevProperties;
	public AbstractUIInteractionDialogue.Properties world2NextProperties;
	[SerializeField]
	private GameObject kingDice;
	[SerializeField]
	private List<GameObject> chalkboardCrosses;
	[SerializeField]
	private DialogueInteractionPoint dialogueInteractionPoint;
	[SerializeField]
	private DialoguerDialogues dialogueWorld2;
	[SerializeField]
	private string completeLevelAnimationTrigger;
	[SerializeField]
	private GameObject world1Background;
	[SerializeField]
	private GameObject world2Background;
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
}
