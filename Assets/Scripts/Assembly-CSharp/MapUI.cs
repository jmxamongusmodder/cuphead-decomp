using UnityEngine;

public class MapUI : AbstractMonoBehaviour
{
	[SerializeField]
	private MapPauseUI pauseUI;
	[SerializeField]
	private MapEquipUI equipUI;
	[SerializeField]
	private OptionsGUI optionsPrefab;
	[SerializeField]
	private RectTransform optionsRoot;
	[SerializeField]
	private AchievementsGUI achievementsPrefab;
	[SerializeField]
	private RectTransform achievementsRoot;
	[SerializeField]
	public Canvas sceneCanvas;
	[SerializeField]
	public Canvas screenCanvas;
	[SerializeField]
	public Canvas hudCanvas;
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;
}
