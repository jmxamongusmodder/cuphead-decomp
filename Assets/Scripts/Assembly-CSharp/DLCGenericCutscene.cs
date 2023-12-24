using UnityEngine;
using UnityEngine.UI;

public class DLCGenericCutscene : Cutscene
{
	protected enum TrappedChar
	{
		None = -1,
		Cuphead = 0,
		Mugman = 1,
		Chalice = 2,
	}

	[SerializeField]
	private float cursorToVisableTime;
	[SerializeField]
	private float mainDelay;
	[SerializeField]
	private Image arrow;
	[SerializeField]
	protected GameObject[] text;
	[SerializeField]
	protected Animator[] screens;
	public Image fader;
}
