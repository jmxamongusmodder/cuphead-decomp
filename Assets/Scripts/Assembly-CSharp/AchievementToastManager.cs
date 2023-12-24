using UnityEngine;
using UnityEngine.UI;

public class AchievementToastManager : AbstractMonoBehaviour
{
	[SerializeField]
	private GameObject uiCameraPrefab;
	[SerializeField]
	private GameObject visual;
	[SerializeField]
	private RectTransform toastTransform;
	[SerializeField]
	private LocalizationHelper titleLocalization;
	[SerializeField]
	private Image icon;
}
