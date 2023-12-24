using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TowerOfPowerContinueCardGUI : AbstractMonoBehaviour
{
	[SerializeField]
	private SpriteRenderer PlayerName;
	[SerializeField]
	private Sprite CupheadNameData;
	[SerializeField]
	private Sprite CupmanNameData;
	[SerializeField]
	private Text TokenLeft_text;
	[SerializeField]
	private Text Continue;
	[SerializeField]
	private Text CountDown_text;
	[SerializeField]
	private List<UIImageAnimationLoop> CardCupheadAnimation;
	[SerializeField]
	private List<UIImageAnimationLoop> CardMugmanAnimation;
	[SerializeField]
	private CanvasGroup canvas;
}
