using System;
using UnityEngine;

public class BaronessLevelWaffle : BaronessLevelMiniBossBase
{
	[Serializable]
	public class WafflePieces
	{
		public Transform wafflepiece;
		public Animator waffleFX;
		public Vector3 direction;
		public float distanceFromCenter;
	}

	[SerializeField]
	private Effect explosion;
	[SerializeField]
	private Effect explosionReverse;
	[SerializeField]
	private WafflePieces[] diagonalPieces;
	[SerializeField]
	private WafflePieces[] straightPieces;
	[SerializeField]
	private Transform mouth;
}
