using UnityEngine;

public class BatLevelBat : LevelProperties.Bat.Entity
{
	[SerializeField]
	private Transform bouncerRoot;
	[SerializeField]
	private Transform coffinRoot;
	[SerializeField]
	private Transform pentagramRoot;
	[SerializeField]
	private BatLevelBouncer bouncerPrefab;
	[SerializeField]
	private BatLevelGoblin goblinPrefab;
	[SerializeField]
	private BatLevelMiniBat minibatPrefab;
	[SerializeField]
	private BatLevelLightning lightningPrefab;
	[SerializeField]
	private BatLevelPentagram pentagramPrefab;
	[SerializeField]
	private BasicProjectile wolfProjectile;
	[SerializeField]
	private BatLevelCross crossPrefab;
	[SerializeField]
	private BatLevelHomingSoul soulPrefab;
}
