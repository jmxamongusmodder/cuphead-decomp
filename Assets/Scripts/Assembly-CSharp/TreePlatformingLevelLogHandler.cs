using UnityEngine;

public class TreePlatformingLevelLogHandler : AbstractPausableComponent
{
	private enum LogTypes
	{
		A = 0,
		B = 1,
		C = 2,
		D = 3,
		E = 4,
		F = 5,
	}

	[SerializeField]
	private float maxHP;
	[SerializeField]
	private bool facingRight;
	[SerializeField]
	private LogTypes[] logOrder;
	[SerializeField]
	private TreePlatformingLevelLog[] logPrefabs;
	[SerializeField]
	private Effect effect;
}
