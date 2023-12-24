using UnityEngine;

public class FlyingGenieLevelGenie : LevelProperties.FlyingGenie.Entity
{
	[SerializeField]
	private Transform hieroBG;
	[SerializeField]
	private Transform brickBG;
	[SerializeField]
	private ScrollingSprite hiero;
	[SerializeField]
	private ScrollingSprite brick;
	[SerializeField]
	private Transform sawMask;
	[SerializeField]
	private GameObject casket;
	[SerializeField]
	private FlyingGenieLevelMeditateFX meditateEffect;
	[SerializeField]
	private BasicProjectile skullPrefab;
	[SerializeField]
	private FlyingGenieLevelBouncer bouncerPrefab;
	[SerializeField]
	private FlyingGenieLevelObelisk obeliskPrefab;
	[SerializeField]
	private FlyingGenieLevelSphinx sphinxPrefab;
	[SerializeField]
	private FlyingGenieLevelGem gemPrefab;
	[SerializeField]
	private FlyingGenieLevelGoop goop;
	[SerializeField]
	private FlyingGenieLevelMummy mummyClassic;
	[SerializeField]
	private FlyingGenieLevelMummy mummyChomper;
	[SerializeField]
	private FlyingGenieLevelMummy mummyChaser;
	[SerializeField]
	private FlyingGenieLevelSword swordPrefab;
	[SerializeField]
	private FlyingGenieLevelGenieTransform genieTransformed;
	[SerializeField]
	private Effect puffEffect;
	[SerializeField]
	private Transform puffRoot;
	[SerializeField]
	private Transform skullRoot;
	[SerializeField]
	private SpriteRenderer carpet;
	[SerializeField]
	private Transform morphRoot;
	[SerializeField]
	private Transform treasureRoot;
}
