using System;
using UnityEngine;
using System.Collections.Generic;

public class Map : AbstractMonoBehaviour
{
	[Serializable]
	public class Camera
	{
		public bool moveX;
		public bool moveY;
		public CupheadBounds bounds;
	}

	public MapResources MapResources;
	[SerializeField]
	private Camera cameraProperties;
	[SerializeField]
	private AbstractMapInteractiveEntity firstNode;
	[SerializeField]
	private AbstractMapInteractiveEntity[] entryPoints;
	public Levels level;
	public List<CoinPositionAndID> LevelCoinsIDs;
}
