using UnityEngine;

public class AirplaneLevelBulldogParachute : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private Transform spawnRoot1;
	[SerializeField]
	private Transform spawnRoot2;
	[SerializeField]
	private AirplaneLevelBoomerang boomerang;
	[SerializeField]
	private AirplaneLevelBoomerang boomerangPink;
	[SerializeField]
	private AirplaneLevelBulldogPlane main;
	[SerializeField]
	private Animator mainAnimator;
	[SerializeField]
	private GameObject collider;
}
