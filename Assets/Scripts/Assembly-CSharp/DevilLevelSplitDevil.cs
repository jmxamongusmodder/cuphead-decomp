using UnityEngine;

public class DevilLevelSplitDevil : LevelProperties.Devil.Entity
{
	public enum State
	{
		Idle = 0,
		Shoot = 1,
		summon = 2,
	}

	public State state;
	[SerializeField]
	private Animator headsControler;
	public bool DevilLeft;
	public bool SplitDevilAnimationDone;
	[SerializeField]
	private DevilLevelSplitDevilProjectile projectilePrefab;
	[SerializeField]
	private Transform projectileRootLeft;
	[SerializeField]
	private Transform projectileRootRight;
}
