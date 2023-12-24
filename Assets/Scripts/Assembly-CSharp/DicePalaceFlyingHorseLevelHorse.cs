using UnityEngine;

public class DicePalaceFlyingHorseLevelHorse : LevelProperties.DicePalaceFlyingHorse.Entity
{
	[SerializeField]
	private Transform bottomLine;
	[SerializeField]
	private Transform middleLine;
	[SerializeField]
	private Transform topLine;
	[SerializeField]
	private Transform bottomLineBackground;
	[SerializeField]
	private Transform middleLineBackground;
	[SerializeField]
	private Transform topLineBackground;
	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse1Prefab;
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse2Prefab;
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse3Prefab;
	[SerializeField]
	private DicePalaceFlyingHorseLevelPresent presentPrefab;
}
