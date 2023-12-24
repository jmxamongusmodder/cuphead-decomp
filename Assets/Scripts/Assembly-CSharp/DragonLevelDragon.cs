using UnityEngine;

public class DragonLevelDragon : LevelProperties.Dragon.Entity
{
	[SerializeField]
	private Transform mouthRoot;
	[SerializeField]
	private DragonLevelMeteor meteorPrefab;
	[SerializeField]
	private Effect smokePrefab;
	[SerializeField]
	private Transform smokeRoot;
	[SerializeField]
	private DragonLevelPeashot peashotPrefab;
	[SerializeField]
	private Transform peashotRoot;
	[SerializeField]
	private Transform chargeRoot;
	[SerializeField]
	private Transform dash;
	[SerializeField]
	private DragonLevelLeftSideDragon leftSideDragon;
	[SerializeField]
	private GameObject damages;
}
