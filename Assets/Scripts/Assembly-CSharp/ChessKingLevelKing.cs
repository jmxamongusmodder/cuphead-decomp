using UnityEngine;

public class ChessKingLevelKing : LevelProperties.ChessKing.Entity
{
	[SerializeField]
	private ChessKingLevelRat ratPrefab;
	[SerializeField]
	private Transform ratSpawn;
	[SerializeField]
	private GameObject beamAttack;
	[SerializeField]
	private GameObject fullAttack;
	[SerializeField]
	private ChessKingLevelGroundTrigger groundTrigger;
	[SerializeField]
	private ChessKingLevelParryPoint parryPoint;
}
