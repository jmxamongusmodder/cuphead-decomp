using UnityEngine;

public class DLCIntroCutscene : DLCGenericCutscene
{
	[SerializeField]
	private GameObject canvas;
	[SerializeField]
	private GameObject cameraPos;
	[SerializeField]
	private Animator BGanim;
	[SerializeField]
	private Transform astralPlaneController;
	[SerializeField]
	private Transform[] astralPlanePositions;
	[SerializeField]
	private float screen4BGScrollSpeed;
	[SerializeField]
	private float screen4ForestScrollStartSpeed;
	[SerializeField]
	private GameObject[] screen4Characters;
	[SerializeField]
	private GameObject screen4Forest;
	[SerializeField]
	private GameObject screen4Clouds;
	[SerializeField]
	private GameObject screen4FG;
	[SerializeField]
	private GameObject screen4ScrollEnd;
	[SerializeField]
	private SpriteRenderer screen4ScrollStart;
	[SerializeField]
	private SpriteRenderer screen4EndLoopBack;
	[SerializeField]
	private int fastForward;
}
