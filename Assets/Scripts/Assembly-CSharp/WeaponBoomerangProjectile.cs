using UnityEngine;

public class WeaponBoomerangProjectile : AbstractProjectile
{
	public float Speed;
	public float forwardDistance;
	public float lateralDistance;
	public float maxDamage;
	public float hitFreezeTime;
	public LevelPlayerController player;
	[SerializeField]
	private bool isEx;
	[SerializeField]
	private Transform trail1;
	[SerializeField]
	private Transform trail2;
	[SerializeField]
	private Effect hitFXPrefab;
}
