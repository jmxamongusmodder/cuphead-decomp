using UnityEngine;

public class PlayerSuperChaliceBounceBall : AbstractProjectile
{
	[SerializeField]
	private Effect smokePuffEffect;
	public Vector2 velocity;
	public LevelPlayerController player;
	public PlayerSuperChaliceBounce super;
}
