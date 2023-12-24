using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SpeechBubble : AbstractPausableComponent
{
	[SerializeField]
	private TextMeshProUGUI mainText;
	[SerializeField]
	private TextMeshProUGUI choiceText;
	[SerializeField]
	private VerticalLayoutGroup layout;
	[SerializeField]
	private Image tail;
	[SerializeField]
	private List<Sprite> tailVariants;
	[SerializeField]
	private RectTransform arrowBox;
	[SerializeField]
	private Image arrow;
	[SerializeField]
	private Image cursor;
	[SerializeField]
	private RectTransform cursorRoot;
	[SerializeField]
	private RectTransform box;
	[SerializeField]
	private List<Sprite> arrowVariants;
	[SerializeField]
	private CanvasGroup canvasGroup;
	[SerializeField]
	private List<RectTransform> bullets;
	public Vector2 basePosition;
	public Vector2 panPosition;
	public string setBossRefText;
	public int maxLines;
	public bool tailOnTheLeft;
	public bool expandOnTheRight;
	public bool waitForRealease;
	public bool waitForFade;
	public bool hideTail;
	[SerializeField]
	private LayoutElement textLayoutElement;
	public bool preventQuit;
}
