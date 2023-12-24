using UnityEngine;

public class RobotLevelRobot : LevelProperties.Robot.Entity
{
	[SerializeField]
	private Effect headcannonSmoke;
	[SerializeField]
	private Transform[] walkingPositions;
	[SerializeField]
	private RobotLevelRobotHead head;
	[SerializeField]
	private RobotLevelRobotBodyPart chest;
	[SerializeField]
	private RobotLevelRobotHatch hatch;
	[SerializeField]
	private GameObject finalForm;
	[SerializeField]
	private CollisionChild[] collisionChilds;
}
