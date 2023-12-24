using UnityEngine;

public class AirplaneLevelBulldogPlane : LevelProperties.Airplane.Entity
{
	public enum State
	{
		Intro = 0,
		Main = 1,
		Parachute = 2,
		TripleAttack = 3,
		CatAttack = 4,
	}

	public State state;
	[SerializeField]
	private Animator leaderIntroBG;
	[SerializeField]
	private Animator hydrantAttackBG;
	[SerializeField]
	private AirplaneLevelTurretDog[] turretSpawnPoints;
	[SerializeField]
	private Transform rocketSpawnLeft;
	[SerializeField]
	private Transform rocketSpawnRight;
	[SerializeField]
	private AirplaneLevelRocket rocketPrefab;
	[SerializeField]
	private Animator bullDogPlane;
	[SerializeField]
	private AirplaneLevelBulldogParachute bulldogParachute;
	[SerializeField]
	private AirplaneLevelBulldogCatAttack bulldogCatAttack;
	[SerializeField]
	private GameObject canteenPlane;
	[SerializeField]
	private float wobbleX;
	[SerializeField]
	private float wobbleY;
	[SerializeField]
	private float wobbleSpeed;
	public bool endPhaseOne;
	[SerializeField]
	private Animator[] smokePuff;
	public bool isDead;
	public bool startPhaseTwo;
}
