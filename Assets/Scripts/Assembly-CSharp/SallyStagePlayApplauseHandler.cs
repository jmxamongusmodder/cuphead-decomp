using UnityEngine;

public class SallyStagePlayApplauseHandler : AbstractPausableComponent
{
	[SerializeField]
	private SallyStagePlayLevelRose rose;
	[SerializeField]
	private Transform[] hands;
	[SerializeField]
	private Transform[] roseHands;
	[SerializeField]
	private Transform roseStill;
	[SerializeField]
	private Transform endPos;
	[SerializeField]
	private string pinkString;
}
