using UnityEngine;

public class FunhousePlatformingLevelDuck : AbstractPlatformingLevelEnemy
{
	[SerializeField]
	private bool isBigDuck;
	[SerializeField]
	private bool parryable;
	[SerializeField]
	private CollisionChild child;
	public bool smallFirst;
	public bool smallLast;
}
