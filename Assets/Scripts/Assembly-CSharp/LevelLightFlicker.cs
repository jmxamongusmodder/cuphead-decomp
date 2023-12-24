using UnityEngine;

public class LevelLightFlicker : AbstractPausableComponent
{
	[SerializeField]
	private float fadeWaitMinSecond;
	[SerializeField]
	private float fadeWaitMaxSecond;
	[SerializeField]
	private int countUntilPause;
}
