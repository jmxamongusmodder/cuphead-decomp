using UnityEngine;

public class BeeLevelSecurityGuard : LevelProperties.Bee.Entity
{
	[SerializeField]
	private Transform bombRoot;
	[SerializeField]
	private BeeLevelSecurityGuardBomb bombPrefab;
}
