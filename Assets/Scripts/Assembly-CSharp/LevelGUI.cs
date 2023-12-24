using UnityEngine;

public class LevelGUI : AbstractMonoBehaviour
{
	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private LevelPauseGUI pause;
	[SerializeField]
	private LevelGameOverGUI gameOver;
	[SerializeField]
	private OptionsGUI optionsPrefab;
	[SerializeField]
	private RectTransform optionsRoot;
	[SerializeField]
	private RestartTowerConfirmGUI restartTowerConfirmPrefab;
	[SerializeField]
	private RectTransform restartTowerConfirmRoot;
	[SerializeField]
	private AchievementsGUI achievementsPrefab;
	[SerializeField]
	private RectTransform achievementsRoot;
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;
}
