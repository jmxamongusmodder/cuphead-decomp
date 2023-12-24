using UnityEngine;

public class FrogsLevelTall : LevelProperties.Frogs.Entity
{
	[SerializeField]
	private FrogsLevelTallFirefly fireflyPrefab;
	[SerializeField]
	private FrogsLevelTallFireflyRoot[] fireflyRoots;
	[SerializeField]
	private Transform spitRoot;
	public Transform shortMorphRoot;
}
