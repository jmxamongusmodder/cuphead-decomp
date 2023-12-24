using UnityEngine;

public class HouseLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private PlayerDeathEffect[] playerTutorialEffects;
	[SerializeField]
	private HouseElderKettle elderDialoguePoint;
	[SerializeField]
	private GameObject tutorialGameObject;
	[SerializeField]
	private int dialoguerVariableID;
}
