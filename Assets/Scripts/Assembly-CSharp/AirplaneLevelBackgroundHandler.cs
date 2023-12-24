using UnityEngine;
using System;

public class AirplaneLevelBackgroundHandler : MonoBehaviour
{
	[Serializable]
	private struct bgObject
	{
		public string nameForSanity;
		public int startFrame;
		public int duration;
		public int spriteIndex;
		public int layerOffset;
	}

	[SerializeField]
	private float frameRate;
	[SerializeField]
	private Color[] bgColor;
	[SerializeField]
	private SpriteRenderer bgFillSprite;
	[SerializeField]
	private Sprite[] hillsSprites;
	[SerializeField]
	private SpriteRenderer hillsRenderer;
	[SerializeField]
	private bgObject[] objects;
	[SerializeField]
	private SpriteRenderer[] spriteRenderers;
	[SerializeField]
	private Sprite[] objectSprites;
	[SerializeField]
	private SpriteRenderer[] cloudRenderers;
	[SerializeField]
	private Animator[] cloudAnimators;
	[SerializeField]
	private SpriteRenderer[] distantHillsRenderers;
	[SerializeField]
	private float distantHillsLoopTime;
	[SerializeField]
	private float distantHillsMaxScale;
	[SerializeField]
	private float distantHillsMinScale;
	[SerializeField]
	private float distantHillsFadeStartScale;
}
