using System;
using UnityEngine;

public class BaronessLevelForegroundChange : AbstractPausableComponent
{
	[Serializable]
	public class ScrollingSpriteInfo
	{
		public SpriteRenderer sprite;
		public float weight;
	}

	[SerializeField]
	private float spawnY;
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
	[SerializeField]
	private BaronessLevelCastle baroness;
	[SerializeField]
	private OneTimeScrollingSprite[] sprites;
}
