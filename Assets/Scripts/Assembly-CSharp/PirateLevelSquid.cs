using UnityEngine;

public class PirateLevelSquid : LevelProperties.Pirate.Entity
{
	[SerializeField]
	private Transform inkOrigin;
	[SerializeField]
	private PirateLevelSquidProjectile inkBlob;
	[SerializeField]
	private Effect splashPrefab;
}
