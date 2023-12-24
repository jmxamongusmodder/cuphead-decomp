using UnityEngine;

public class ChessQueenLevelLightning : AbstractProjectile
{
	[SerializeField]
	private SpriteRenderer bottomRenderer;
	[SerializeField]
	private SpriteRenderer middleRenderer;
	[SerializeField]
	private SpriteRenderer topRenderer;
	[SerializeField]
	private SpriteRenderer dustRenderer;
	[SerializeField]
	private SpriteRenderer deathSparkRenderer;
	[SerializeField]
	private Sprite[] rotatingSprites;
	[SerializeField]
	private Sprite[] deathSparkSprites;
	[SerializeField]
	private Effect lionsLandDustFX;
	[SerializeField]
	private Transform dropDustPos;
	[SerializeField]
	private SpriteDeathParts deathParts;
	[SerializeField]
	private SpriteDeathPartsDLC deathDust;
}
