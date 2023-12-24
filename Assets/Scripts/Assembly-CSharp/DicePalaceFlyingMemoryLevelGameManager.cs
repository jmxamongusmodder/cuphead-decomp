using UnityEngine;
using System.Collections.Generic;

public class DicePalaceFlyingMemoryLevelGameManager : LevelProperties.DicePalaceFlyingMemory.Entity
{
	public int contactDimX;
	public int contactDimY;
	[SerializeField]
	private Transform cardStopRoot;
	[SerializeField]
	private List<Sprite> chosenFlippedDownCards;
	[SerializeField]
	private DicePalaceFlyingMemoryLevelContactPoint contactPointPrefab;
	[SerializeField]
	private DicePalaceFlyingMemoryLevelStuffedToy stuffedToy;
	[SerializeField]
	private DicePalaceFlyingMemoryLevelCard cardPrefab;
	[SerializeField]
	private DicePalaceFlyingMemoryLevelBot botPrefab;
}
