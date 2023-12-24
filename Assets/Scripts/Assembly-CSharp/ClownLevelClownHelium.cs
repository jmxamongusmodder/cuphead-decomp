using System;
using UnityEngine;

public class ClownLevelClownHelium : LevelProperties.Clown.Entity
{
	[Serializable]
	public class PipePositions
	{
		public Transform pipeEntrance;
		public int orderNum;
	}

	[SerializeField]
	private Animator tankEffects;
	[SerializeField]
	private ClownLevelClownHorse clownHorse;
	[SerializeField]
	private GameObject head;
	[SerializeField]
	private Transform pivotPoint;
	[SerializeField]
	private Transform heliumStopPos;
	[SerializeField]
	private PipePositions[] pipePositions;
	[SerializeField]
	private ClownLevelDogBalloon regularDog;
	[SerializeField]
	private ClownLevelDogBalloon pinkDog;
}
