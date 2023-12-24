using UnityEngine;

public class CutsceneGUI : AbstractMonoBehaviour
{
	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	public CutscenePauseGUI pause;
	[SerializeField]
	private CupheadUICamera uiCameraPrefab;
}
