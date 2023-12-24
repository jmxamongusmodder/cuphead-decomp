using System;
using UnityEngine;

public class TrainLevelEngineBoss : LevelProperties.Train.Entity
{
	[Serializable]
	public class DoorSprites
	{
		public SpriteRenderer open;
		public SpriteRenderer closed;
		public SpriteRenderer opening;
		public SpriteRenderer closing;
	}

	[Serializable]
	public class TailSprites
	{
		public SpriteRenderer on;
		public SpriteRenderer off;
	}

	[SerializeField]
	private DamageReceiverChild heartDamageReceiver;
	[SerializeField]
	private Transform footDustRoot;
	[SerializeField]
	private Effect footDustPrefab;
	[SerializeField]
	private Transform dropperRoot;
	[SerializeField]
	private TrainLevelEngineBossDropperProjectile dropperPrefab;
	[SerializeField]
	private Transform fireRoot;
	[SerializeField]
	private TrainLevelEngineBossFireProjectile firePrefab;
	[SerializeField]
	private DoorSprites doorSprites;
	[SerializeField]
	private GameObject door;
	[SerializeField]
	private TailSprites tailSprites;
	[SerializeField]
	private Transform tailRoot;
}
