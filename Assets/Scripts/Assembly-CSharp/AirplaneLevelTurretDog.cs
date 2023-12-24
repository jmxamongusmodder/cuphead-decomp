using UnityEngine;

public class AirplaneLevelTurretDog : AbstractPausableComponent
{
	[SerializeField]
	private AirplaneLevelTurretBullet bulletPrefab;
	[SerializeField]
	private Transform rootPos;
	[SerializeField]
	private Effect FX;
}
