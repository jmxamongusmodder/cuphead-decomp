using UnityEngine;

public class DicePalaceDominoLevelDomino : LevelProperties.DicePalaceDomino.Entity
{
	[SerializeField]
	private Transform bouncySpawnpoint;
	[SerializeField]
	private Transform birdSpawnpoint;
	[SerializeField]
	private DicePalaceDominoLevelBouncyBall bouncyBallPrefab;
	[SerializeField]
	private DicePalaceDominoLevelBoomerang boomerangPrefab;
	[SerializeField]
	private DicePalaceDominoLevelFloor floor;
}
