using UnityEngine;

public class KitchenLevel : Level
{
	[SerializeField]
	private Sprite _bossPortrait;
	[SerializeField]
	[MultilineAttribute]
	private string _bossQuote;
	[SerializeField]
	private GameObject beforeGettingIngredients;
	[SerializeField]
	private GameObject afterGettingIngredients;
	[SerializeField]
	private SpriteRenderer[] sunbeams;
	[SerializeField]
	private float sunbeamCycleSpeed;
	[SerializeField]
	private Animator saltbakerShadow;
	[SerializeField]
	private Transform triggerEndGame;
	[SerializeField]
	private SpriteRenderer trapDoorOverlay;
	[SerializeField]
	private GameObject kitchenBG;
	[SerializeField]
	private GameObject basementBG;
	[SerializeField]
	private Material playerBasementMaterial;
	[SerializeField]
	private Transform[] torchPositions;
}
