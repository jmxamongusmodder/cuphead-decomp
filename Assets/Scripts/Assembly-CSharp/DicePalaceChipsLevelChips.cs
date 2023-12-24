using System;
using UnityEngine;

public class DicePalaceChipsLevelChips : LevelProperties.DicePalaceChips.Entity
{
	[Serializable]
	public class ChipPieces
	{
		public Transform chipTransform;
		public Vector3 startPosition;
		public float rotationSpeed;
	}

	[SerializeField]
	private ChipPieces[] chips;
	[SerializeField]
	private Transform mainLayer;
	[SerializeField]
	private GameObject hat;
}
