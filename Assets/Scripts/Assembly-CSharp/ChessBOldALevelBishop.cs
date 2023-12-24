using UnityEngine;

public class ChessBOldALevelBishop : LevelProperties.ChessBOldA.Entity
{
	[SerializeField]
	private BasicProjectile turretShot;
	[SerializeField]
	private ParrySwitch pink;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private ChessBOldALevelWall wallPrefab;
}
