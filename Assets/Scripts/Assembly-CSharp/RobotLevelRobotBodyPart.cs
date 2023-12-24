using UnityEngine;

public class RobotLevelRobotBodyPart : AbstractCollidableObject
{
	[SerializeField]
	protected Effect deathEffect;
	[SerializeField]
	protected GameObject primary;
	[SerializeField]
	protected GameObject secondary;
}
