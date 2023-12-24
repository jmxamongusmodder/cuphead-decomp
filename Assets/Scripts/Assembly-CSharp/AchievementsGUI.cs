using System;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsGUI : AbstractMonoBehaviour
{
	[Serializable]
	public class IconRow
	{
		public AchievementIcon[] achievementIcons;
	}

	[SerializeField]
	private IconRow[] iconRows;
	[SerializeField]
	private RectTransform cursor;
	[SerializeField]
	private Image topArrow;
	[SerializeField]
	private Image bottomArrow;
	[SerializeField]
	private Image background;
	[SerializeField]
	private Image unearnedBackground;
	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text descriptionText;
	[SerializeField]
	private LocalizationHelper titleLocalization;
	[SerializeField]
	private LocalizationHelper descriptionLocalization;
	[SerializeField]
	private Image largeIcon;
	[SerializeField]
	private Image noise;
	[SerializeField]
	private Sprite[] arrowSprites;
}
