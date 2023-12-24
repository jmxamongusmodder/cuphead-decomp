using UnityEngine;

public class ChessBishopLevelBishop : LevelProperties.ChessBishop.Entity
{
	[SerializeField]
	private ChessBishopLevelBell bellProjectile;
	[SerializeField]
	private SpriteRenderer mainRenderer;
	[SerializeField]
	private SpriteRenderer summonOverlayRenderer;
	[SerializeField]
	private Transform projectileSpawnPoint;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private GameObject candlesHolder;
	[SerializeField]
	private ChessBishopLevelCandle[] candles;
	[SerializeField]
	private Animator bodyAnimator;
	[SerializeField]
	private Effect bodyExplosion;
	[SerializeField]
	private Transform bodyExplosionSpawnPoint;
	[SerializeField]
	private SpriteRenderer bodyRenderer;
	[SerializeField]
	private float fadeRate;
	[SerializeField]
	private GameObject[] playerMask;
}
