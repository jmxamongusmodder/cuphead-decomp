using UnityEngine;

public class FrogsLevelShort : LevelProperties.Frogs.Entity
{
	[SerializeField]
	private Effect introDust;
	[SerializeField]
	private Transform[] rageRoots;
	[SerializeField]
	private FrogsLevelShortRageBullet rageFireball;
	[SerializeField]
	private Effect rageFireballSpark;
	[SerializeField]
	private FrogsLevelShortClapBullet clapBullet;
	[SerializeField]
	private Effect clapEffect;
	[SerializeField]
	private Transform clapRoot;
}
