using UnityEngine;
using UnityEngine.UI;

public class TutorialShmupLevelParryNext : AbstractCollidableObject
{
	[SerializeField]
	private TutorialShmupLevelParryNext nextSphere;
	[SerializeField]
	private Image image;
	[SerializeField]
	private bool startAsParry;
	[SerializeField]
	private ParrySwitch parrySwitch;
}
