using UnityEngine;

public class MouseLevel : Level
{
	[SerializeField]
	private MouseLevelCanMouse mouseCan;
	[SerializeField]
	private MouseLevelBrokenCanMouse mouseBrokenCan;
	[SerializeField]
	private MouseLevelCat cat;
	[SerializeField]
	private Animator wallAnimator;
	[SerializeField]
	private Sprite _bossPortraitMain;
	[SerializeField]
	private Sprite _bossPortraitBrokenCan;
	[SerializeField]
	private Sprite _bossPortraitCat;
	[SerializeField]
	private string _bossQuoteMain;
	[SerializeField]
	private string _bossQuoteBrokenCan;
	[SerializeField]
	private string _bossQuoteCat;
}
