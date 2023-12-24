using UnityEngine;

public class OldManLevelBleachers : AbstractPausableComponent
{
	[SerializeField]
	private GameObject gnomeBleacherRight;
	[SerializeField]
	private Transform gnomeBleacherRightEnd;
	[SerializeField]
	private GameObject gnomeBleacherLeft;
	[SerializeField]
	private Transform gnomeBleacherLeftEnd;
	[SerializeField]
	private OldManLevel level;
	[SerializeField]
	private float enterStepTime;
	[SerializeField]
	private float enterStepPause;
	[SerializeField]
	private float exitStepTime;
	[SerializeField]
	private float exitStepPause;
	[SerializeField]
	private float offset;
}
