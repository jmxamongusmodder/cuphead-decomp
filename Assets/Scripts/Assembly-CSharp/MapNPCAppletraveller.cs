using UnityEngine;

public class MapNPCAppletraveller : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private int dialoguerVariableID;
	[SerializeField]
	private string coinID1;
	[SerializeField]
	private string coinID2;
	[SerializeField]
	private string coinID3;
	[SerializeField]
	private float radiusStartWaving;
	[SerializeField]
	private float radiusStopWaving;
	public bool SkipDialogueEvent;
}
