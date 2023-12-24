using UnityEngine;

public class OldManLevelDwarf : AbstractProjectile
{
	[SerializeField]
	private Effect deathPuff;
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private Effect beardPopA;
	[SerializeField]
	private Effect beardPopB;
	[SerializeField]
	private Effect beardHealA;
	[SerializeField]
	private Effect beardHealB;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private Collider2D coll;
	[SerializeField]
	private float shadowRange;
	[SerializeField]
	private SpriteRenderer shadowRend;
	[SerializeField]
	private Sprite[] shadowSprites;
	[SerializeField]
	private OldManLevelBeardController beardController;
	[SerializeField]
	private int rufflePos;
}
