using UnityEngine;

public class OldManLevelSockPuppet : AbstractCollidableObject
{
	[SerializeField]
	private GameObject arms;
	[SerializeField]
	private GameObject armsHolding;
	[SerializeField]
	private Transform throwPos;
	[SerializeField]
	private Transform catchPos;
	public bool ready;
	[SerializeField]
	private float armBowingXModifier;
	[SerializeField]
	private float wobbleX;
	[SerializeField]
	private float wobbleY;
	public Vector3 rootPosition;
	public Vector3 startPosition;
	[SerializeField]
	private float wobbleTimer;
	[SerializeField]
	private float moveTimeShort;
	[SerializeField]
	private float moveTimeLong;
	[SerializeField]
	private float moveOvershoot;
	[SerializeField]
	private bool isLeft;
	[SerializeField]
	private OldManLevelSockPuppetHandler main;
}
