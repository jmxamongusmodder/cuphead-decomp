using UnityEngine;

public class RobotLevelRobotChest : RobotLevelRobotBodyPart
{
	[SerializeField]
	private SpriteRenderer torsoTop;
	[SerializeField]
	private GameObject[] damagedPortholes;
	[SerializeField]
	private GameObject frontArm;
	[SerializeField]
	private GameObject backArm;
	[SerializeField]
	private Transform portholeRoot;
	[SerializeField]
	private Transform portholeOffsetRoot;
	[SerializeField]
	private Transform[] panicArmsPath;
	[SerializeField]
	private Transform magnetStartRoot;
	[SerializeField]
	private Transform magnetEndRoot;
	[SerializeField]
	private GameObject damageEffect;
}
