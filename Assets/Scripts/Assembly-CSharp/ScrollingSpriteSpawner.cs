using System;
using UnityEngine;

public class ScrollingSpriteSpawner : AbstractPausableComponent
{
	[Serializable]
	public class ScrollingSpriteInfo
	{
		public SpriteRenderer sprite;
		public float weight;
	}

	[SerializeField]
	private bool customStart;
	[SerializeField]
	private float spawnY;
	[SerializeField]
	private bool usePrefabY;
	[SerializeField]
	private float speed;
	[SerializeField]
	private float minSpacing;
	[SerializeField]
	private float averageSpacing;
	[SerializeField]
	private int sortingOrder;
	[SerializeField]
	private ScrollingSpriteInfo[] spritePrefabs;
}
