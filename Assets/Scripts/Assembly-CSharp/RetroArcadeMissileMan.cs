using UnityEngine;

public class RetroArcadeMissileMan : RetroArcadeEnemy
{
	[SerializeField]
	private RetroArcadeMissile missilePrefab;
	[SerializeField]
	private Transform shootRootLeft;
	[SerializeField]
	private Transform shootRootRight;
	[SerializeField]
	private Transform pivotPoint;
}
