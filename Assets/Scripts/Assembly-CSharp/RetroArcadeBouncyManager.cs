using UnityEngine;

public class RetroArcadeBouncyManager : LevelProperties.RetroArcade.Entity
{
	[SerializeField]
	private RetroArcadeBouncyBallHolder ballHolder;
	[SerializeField]
	private Transform[] spawnPoints;
}
