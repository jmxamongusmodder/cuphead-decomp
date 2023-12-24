using UnityEngine.UI;
using UnityEngine;

public class MapDifficultySelectStartUI : AbstractMapSceneStartUI
{
	[SerializeField]
	private Image inAnimated;
	[SerializeField]
	private Image bossTitleImage;
	[SerializeField]
	private Image bossNameImage;
	[SerializeField]
	private Image difficultyImage;
	[SerializeField]
	private Sprite[] inSprites;
	[SerializeField]
	private Image[] separatorsAnimated;
	[SerializeField]
	private Sprite[] separatorsSprites;
	[SerializeField]
	private RectTransform cursor;
	[SerializeField]
	private RectTransform easy;
	[SerializeField]
	private RectTransform normal;
	[SerializeField]
	private RectTransform normalSeparator;
	[SerializeField]
	private RectTransform hard;
	[SerializeField]
	private RectTransform hardSeparator;
	[SerializeField]
	private RectTransform box;
	[SerializeField]
	private Color selectedColor;
	[SerializeField]
	private Color unselectedColor;
	[SerializeField]
	private LocalizationHelper bossImage;
	[SerializeField]
	private LocalizationHelper bossName;
	[SerializeField]
	private LocalizationHelper difficultyText;
	[SerializeField]
	private Material bossCardWhiteMaterial;
	[SerializeField]
	private Image difficultySelectionText;
	[SerializeField]
	private Text inText;
	[SerializeField]
	private GlowText glowScript;
	[SerializeField]
	private LocalizationHelper bossGlow;
	[SerializeField]
	private Image asianGlow;
}
