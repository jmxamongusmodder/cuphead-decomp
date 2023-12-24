using UnityEngine;

public class PlayerSuperChaliceIIISpear : AbstractProjectile
{
	[SerializeField]
	private BoxCollider2D coll;
	[SerializeField]
	private float floatAmplitude;
	[SerializeField]
	private float floatT;
	[SerializeField]
	private float floatSpeed;
	public LevelPlayerController sourcePlayer;
}
