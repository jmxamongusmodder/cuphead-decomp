using UnityEngine;

public class FunhousePlatformingLevelJackinBox : PlatformingLevelShootingEnemy
{
	[SerializeField]
	private FunhousePlatformingLevelJackinBoxProjectile projectile;
	[SerializeField]
	private GameObject jack;
	[SerializeField]
	private Transform jackRoot;
	[SerializeField]
	private Transform topRoot;
	[SerializeField]
	private Transform bottomRoot;
	[SerializeField]
	private Transform rightRoot;
	[SerializeField]
	private Transform leftRoot;
	[SerializeField]
	private Transform top;
	[SerializeField]
	private Transform bottom;
	[SerializeField]
	private Transform right;
	[SerializeField]
	private Transform left;
}
