using UnityEngine;

public class MapNPCCoinMoneyman : MonoBehaviour
{
	[SerializeField]
	private Animator animator;
	[SerializeField]
	private float idleDurationMin;
	[SerializeField]
	private float idleDurationMax;
	[SerializeField]
	private string[] hiddenCoinIds;
	[SerializeField]
	private int dialoguerVariableID;
}
