using UnityEngine;

public class CircusPlatformingLevelCannon : AbstractPausableComponent
{
	[SerializeField]
	private float health;
	[SerializeField]
	private DamageReceiver[] cannons;
	[SerializeField]
	private Transform[] shootRoots;
	[SerializeField]
	private CircusPlatformingLevelCannonProjectile projectile;
	[SerializeField]
	private float projectileSpeed;
	[SerializeField]
	private float projectileDelay;
	[SerializeField]
	private Transform startTrigger;
	[SerializeField]
	private Transform endTrigger;
	[SerializeField]
	private string pinkString;
}
