using UnityEngine;

public class DicePalaceFlyingMemoryLevelStuffedToy : LevelProperties.DicePalaceFlyingMemory.Entity
{
	public enum State
	{
		Open = 0,
		Closed = 1,
	}

	[SerializeField]
	private Transform projectileRoot;
	[SerializeField]
	private DicePalaceFlyingMemoryMusicNote projectile;
	[SerializeField]
	private DicePalaceFlyingMemoryLevelSpiralProjectile spiralProjectile;
	[SerializeField]
	private GameObject sprite;
	[SerializeField]
	private SpriteRenderer hand;
	public bool guessedWrong;
	public State state;
	public bool currentlyColliding;
}
