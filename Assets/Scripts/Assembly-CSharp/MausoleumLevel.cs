using UnityEngine;

public class MausoleumLevel : Level
{
	[SerializeField]
	private GameObject[] WorldBackgrounds;
	[SerializeField]
	private MausoleumLevelCircleGhost circleGhost;
	[SerializeField]
	private MausoleumLevelRegularGhost regularGhost;
	[SerializeField]
	private MausoleumLevelBigGhost bigGhost;
	[SerializeField]
	private MausoleumLevelDelayGhost delayGhost;
	[SerializeField]
	private MausoleumLevelSineGhost sineGhost;
	[SerializeField]
	private Transform[] positions;
	[SerializeField]
	private MausoleumLevelUrn urn;
	[SerializeField]
	private Sprite _bossPortraitEasy;
	[SerializeField]
	private Sprite _bossPortraitNormal;
	[SerializeField]
	private Sprite _bossPortraitHard;
	[SerializeField]
	private string _bossQuoteEasy;
	[SerializeField]
	private string _bossQuoteNormal;
	[SerializeField]
	private string _bossQuoteHard;
	[SerializeField]
	private Animator helpSignAnimator;
	[SerializeField]
	private Animator[] urnsAnimator;
	[SerializeField]
	private Animator[] chaliceCharacterAnimators;
	[SerializeField]
	private Effect chaliceBeamEffect;
	[SerializeField]
	private MausoleumDialogueInteraction dialogue;
	[SerializeField]
	private int dialoguerVariableID;
}
