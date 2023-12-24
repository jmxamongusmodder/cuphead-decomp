using UnityEngine;

public class RetroArcadeCaterpillarBodyPart : RetroArcadeEnemy
{
	[SerializeField]
	private BasicProjectile bulletPrefab;
	[SerializeField]
	private Transform bulletRoot;
	[SerializeField]
	private bool isHead;
}
