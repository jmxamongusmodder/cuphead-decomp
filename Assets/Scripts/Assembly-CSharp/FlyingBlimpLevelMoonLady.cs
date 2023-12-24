using UnityEngine;

public class FlyingBlimpLevelMoonLady : LevelProperties.FlyingBlimp.Entity
{
	public bool changeStarted;
	[SerializeField]
	private GameObject smoke;
	[SerializeField]
	private Transform smokeFlippedPos;
	[SerializeField]
	private AudioSource pedal;
	[SerializeField]
	private AudioSource gears;
	[SerializeField]
	private FlyingBlimpLevelUFO ufoPrefabA;
	[SerializeField]
	private FlyingBlimpLevelUFO ufoPrefabB;
	[SerializeField]
	private Transform ufoStartPoint;
	[SerializeField]
	private Transform ufoMidPoint;
	[SerializeField]
	private Transform ufoStopPoint;
	[SerializeField]
	private Transform dimColor;
	[SerializeField]
	private Transform transformSpawnPoint;
	[SerializeField]
	private Transform transformMorphEndPoint;
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabA;
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabB;
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabC;
	[SerializeField]
	private FlyingBlimpLevelStars starPrefabPink;
}
