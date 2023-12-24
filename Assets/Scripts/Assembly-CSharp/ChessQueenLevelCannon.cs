using UnityEngine;

public class ChessQueenLevelCannon : AbstractCollidableObject
{
	[SerializeField]
	private ChessQueenLevelCannonball cannonBall;
	[SerializeField]
	private ParrySwitch[] parry;
	[SerializeField]
	private SpriteRenderer baseRenderer;
	[SerializeField]
	private SpriteRenderer barrelHighlightRenderer;
	[SerializeField]
	private SpriteRenderer baseTopperRenderer;
	[SerializeField]
	private Transform barrelTransform;
	[SerializeField]
	private Transform bulletSpawnPoint;
	[SerializeField]
	private Transform blastFXSpawnPoint;
	[SerializeField]
	private Transform blastFXTransform;
	[SerializeField]
	private Sprite[] baseSprites;
	[SerializeField]
	private Sprite[] barrelHighlightSprites;
	[SerializeField]
	private Transform wickTransform;
	[SerializeField]
	private Transform wickParabolaEndTransform;
	[SerializeField]
	private Transform wickBlastPositionerTransform;
	[SerializeField]
	private Animator mouseAnimator;
	[SerializeField]
	private ChessQueenLevelLooseMouse looseMouse;
	[SerializeField]
	private bool mouseReverses;
}
