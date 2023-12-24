using System;
using UnityEngine;

public class FlyingBlimpLevelScrollingSpriteSpawnerBase : AbstractPausableComponent
{
	[Serializable]
	public class ScrollingSpriteInfo
	{
		public SpriteRenderer sprite;
		public float weight;
	}

	[SerializeField]
	protected float spawnY;
	[SerializeField]
	protected float speed;
	[SerializeField]
	protected float minSpacing;
	[SerializeField]
	protected float averageSpacing;
	[SerializeField]
	protected int sortingOrder;
	[SerializeField]
	protected ScrollingSpriteInfo[] spritePrefabs;
}
