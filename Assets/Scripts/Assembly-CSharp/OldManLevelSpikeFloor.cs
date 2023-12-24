using UnityEngine;

public class OldManLevelSpikeFloor : AbstractCollidableObject
{
	public enum SpikeState
	{
		Idle = 0,
		Spiked = 1,
		Gnomed = 2,
	}

	[SerializeField]
	private Effect deathPuff;
	[SerializeField]
	private SpriteDeathParts[] deathParts;
	[SerializeField]
	private Transform shootRoot;
	[SerializeField]
	private BasicProjectile gnomeProjectile;
	[SerializeField]
	private BasicProjectile gnomePinkProjectile;
	public SpikeState spikeState;
	[SerializeField]
	private bool dontShootLeft;
	[SerializeField]
	private bool dontShootRight;
	[SerializeField]
	private SpriteRenderer gnomeRenderer;
	[SerializeField]
	private SpriteRenderer tuftRenderer;
	[SerializeField]
	private SpriteRenderer shootFXRenderer;
}
