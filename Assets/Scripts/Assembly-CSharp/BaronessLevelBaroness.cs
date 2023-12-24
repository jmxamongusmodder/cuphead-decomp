using UnityEngine;

public class BaronessLevelBaroness : AbstractCollidableObject
{
	[SerializeField]
	private Transform baronessTossPoint;
	[SerializeField]
	private Transform baronessProjectileShootPoint;
	[SerializeField]
	private BaronessLevelBaronessProjectileBunch baronessProjectileBunch;
	[SerializeField]
	private BaronessLevelFollowingProjectile baronessFollowProjectile;
	[SerializeField]
	public Transform shootPoint;
	public bool isEasyFinal;
	public int shootCounter;
	public int popUpCounter;
	public int transformCounter;
	public bool shotEnough;
}
