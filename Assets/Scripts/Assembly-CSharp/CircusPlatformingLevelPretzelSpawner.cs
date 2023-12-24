using UnityEngine;

public class CircusPlatformingLevelPretzelSpawner : PlatformingLevelEnemySpawner
{
	[SerializeField]
	private CircusPlatformingLevelPretzel pretzelPrefab;
	[SerializeField]
	private string sideString;
	[SerializeField]
	private Transform[] path;
}
