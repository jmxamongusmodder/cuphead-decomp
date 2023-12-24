using UnityEngine;

public class MausoleumLevelGhostBase : BasicProjectile
{
	[SerializeField]
	private MinMax idleDelay;
	[SerializeField]
	private string idleSound;
	[SerializeField]
	private bool hasIdleSFX;
	public bool Counts;
}
