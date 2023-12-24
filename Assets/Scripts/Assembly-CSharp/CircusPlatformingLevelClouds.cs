using System;
using UnityEngine;

public class CircusPlatformingLevelClouds : AbstractPausableComponent
{
	[Serializable]
	public class CloudPiece
	{
		public Transform cloud;
		public float cloudEndY;
		public float cameraRelativePosX;
		public float speedMultiplyAmount;
	}

	[SerializeField]
	private CloudPiece[] cloudPieces;
	[SerializeField]
	private float incrementAmount;
}
