using UnityEngine;

public class DragonLevelLeftSideDragon : LevelProperties.Dragon.Entity
{
	[SerializeField]
	private Collider2D damageBox;
	[SerializeField]
	private DragonLevelSpire spire;
	[SerializeField]
	private DragonLevelRain rain;
	[SerializeField]
	private DragonLevelBackgroundChange[] backgrounds;
	[SerializeField]
	private GameObject[] backgroundsToHide;
	[SerializeField]
	private DragonLevelFire fire;
	[SerializeField]
	private Transform fireMarcherRoot;
	[SerializeField]
	private DragonLevelFireMarcher[] fireMarcherPrefabs;
	[SerializeField]
	private DragonLevelFireMarcher fireMarcherLeaderPrefab;
	[SerializeField]
	private Transform topHead;
	[SerializeField]
	private Transform bottomHead;
	[SerializeField]
	private Transform middleHead;
	[SerializeField]
	private DragonLevelPotion horizontalPotionPrefab;
	[SerializeField]
	private DragonLevelPotion verticalPotionPrefab;
	[SerializeField]
	private DragonLevelPotion bothPotionPrefab;
	[SerializeField]
	private Transform spittle;
}
