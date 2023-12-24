using UnityEngine;

public class SlimeLevelTombstone : LevelProperties.Slime.Entity
{
	[SerializeField]
	private Transform dirt;
	[SerializeField]
	private Transform dust;
	[SerializeField]
	private Transform dust2;
	[SerializeField]
	private Effect dustPrefab;
	[SerializeField]
	private SlimeLevelSlime bigSlime;
	[SerializeField]
	private SlimeLevelTinySlime tinySlime;
	[SerializeField]
	private Effect smashDustBackPrefab;
	[SerializeField]
	private Effect smashDustFrontPrefab;
}
