using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelGameOverGUI : AbstractMonoBehaviour
{
	[Serializable]
	public class TimelineObjects
	{
		public RectTransform timeline;
		public RectTransform line;
		public Image cuphead;
		public Image mugman;
		public Image chalice;
		public Transform start;
		public Transform end;
	}

	[SerializeField]
	private Image youDiedText;
	[SerializeField]
	private CanvasGroup cardCanvasGroup;
	[SerializeField]
	private CanvasGroup helpCanvasGroup;
	[SerializeField]
	private Image bossPortraitImage;
	[SerializeField]
	private Text bossQuoteText;
	[SerializeField]
	private LocalizationHelper bossQuoteLocalization;
	[SerializeField]
	private Text[] menuItems;
	[SerializeField]
	private TimelineObjects timeline;
	[SerializeField]
	private GameObject timelineObj;
	[SerializeField]
	private Sprite timelineSecret;
	[SerializeField]
	private LevelEquipUI equipUI;
	[SerializeField]
	private GameObject equipToolTip;
	[SerializeField]
	private LocalizationHelper retryLocHelper;
}
