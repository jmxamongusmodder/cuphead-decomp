using UnityEngine;

public class DicePalaceDominoLevelScrollingFloor : MonoBehaviour
{
	public float speed;
	public float resetPositionX;
	[SerializeField]
	private DicePalaceDominoLevelRandomTile[] dominoLevelRandomTiles;
	[SerializeField]
	private DicePalaceDominoLevelRandomSpike[] dominoLevelRandomSpikes;
}
