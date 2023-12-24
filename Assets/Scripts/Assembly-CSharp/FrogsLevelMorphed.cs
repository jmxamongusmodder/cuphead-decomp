using UnityEngine;

public class FrogsLevelMorphed : LevelProperties.Frogs.Entity
{
	[SerializeField]
	private FrogsLevelMorphedCoin coin;
	[SerializeField]
	private Transform coinRoot;
	public Transform switchRoot;
	[SerializeField]
	private GameObject slotsParent;
	[SerializeField]
	private Slots slots;
	[SerializeField]
	private FrogsLevelSnakeBullet snakeBullet;
	[SerializeField]
	private FrogsLevelBisonBullet bisonBullet;
	[SerializeField]
	private FrogsLevelTigerBullet tigerBullet;
	[SerializeField]
	private FrogsLevelOniBullet oniBullet;
	[SerializeField]
	private Transform slotBulletRoot;
	[SerializeField]
	private Effect dustEffect;
}
