using UnityEngine;

public class GraveyardLevelSplitDevil : LevelProperties.Graveyard.Entity
{
	[SerializeField]
	private GraveyardLevelSplitDevilProjectile projectilePrefab;
	[SerializeField]
	private GraveyardLevelSplitDevilProjectile projectilePinkPrefab;
	[SerializeField]
	private GraveyardLevelSplitDevilBeam beamPrefab;
	[SerializeField]
	private GameObject bgSkellyMask;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private SpriteRenderer headRend;
	[SerializeField]
	private SpriteRenderer mainRend;
	[SerializeField]
	private SpriteRenderer headlessRend;
	[SerializeField]
	private SpriteRenderer haloRend;
	[SerializeField]
	private Transform projectileRoot;
	public bool isAngel;
	public bool dead;
	[SerializeField]
	private SpriteRenderer[] shootFXRend;
}
