using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043F RID: 1087
public class ScrollingSpriteSpawner : AbstractPausableComponent
{
	// Token: 0x06000FF9 RID: 4089 RVA: 0x0009E074 File Offset: 0x0009C474
	protected override void Awake()
	{
		base.Awake();
		if (!this.customStart)
		{
			base.StartCoroutine(this.loop_cr(false));
		}
	}

	// Token: 0x06000FFA RID: 4090 RVA: 0x0009E095 File Offset: 0x0009C495
	public void StartLoop(bool ensureInitialOffscreenSpawn = false)
	{
		base.StartCoroutine(this.loop_cr(ensureInitialOffscreenSpawn));
	}

	// Token: 0x06000FFB RID: 4091 RVA: 0x0009E0A8 File Offset: 0x0009C4A8
	private IEnumerator loop_cr(bool ensureInitialOffscreenSpawn)
	{
		float leadTime = 2560f / this.speed;
		float totalWeight = 0f;
		foreach (ScrollingSpriteSpawner.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = UnityEngine.Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		ScrollingSpriteSpawner.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			while (this.pauseScrolling)
			{
				yield return null;
			}
			float waitTime = spacing / this.speed;
			if (ensureInitialOffscreenSpawn)
			{
				waitTime = Mathf.Max(waitTime, leadTime * 1.1f);
			}
			if (leadTime > waitTime)
			{
				leadTime -= waitTime;
			}
			else
			{
				if (leadTime > 0f)
				{
					waitTime -= leadTime;
					leadTime = 0f;
				}
				yield return CupheadTime.WaitForSeconds(this, waitTime);
			}
			float maxP = totalWeight;
			if (lastSpawned != null)
			{
				maxP -= lastSpawned.weight;
			}
			float p = UnityEngine.Random.Range(0f, maxP);
			float cumulativeWeight = 0f;
			ScrollingSpriteSpawner.ScrollingSpriteInfo toSpawn = lastSpawned;
			foreach (ScrollingSpriteSpawner.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
			{
				toSpawn = scrollingSpriteInfo2;
				if (scrollingSpriteInfo2 != lastSpawned)
				{
					cumulativeWeight += scrollingSpriteInfo2.weight;
					if (cumulativeWeight >= p)
					{
						break;
					}
				}
			}
			SpriteRenderer sprite = UnityEngine.Object.Instantiate<SpriteRenderer>(toSpawn.sprite);
			GameObject obj = sprite.gameObject;
			float x = 1280f - leadTime * this.speed + sprite.bounds.size.x / 2f;
			if (this.usePrefabY)
			{
				obj.transform.position = new Vector3(x, toSpawn.sprite.transform.position.y);
			}
			else
			{
				obj.transform.position = new Vector2(x, this.spawnY);
			}
			obj.transform.SetParent(base.transform, false);
			sprite.sortingOrder = this.sortingOrder;
			OneTimeScrollingSprite scrollingSprite = obj.AddComponent<OneTimeScrollingSprite>();
			scrollingSprite.speed = this.speed;
			spacing = this.minSpacing + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing) + sprite.bounds.size.x;
			this.OnSpawn(obj);
			lastSpawned = toSpawn;
		}
		yield break;
	}

	// Token: 0x06000FFC RID: 4092 RVA: 0x0009E0CA File Offset: 0x0009C4CA
	public void HandlePausing(bool pause)
	{
		this.pauseScrolling = pause;
	}

	// Token: 0x06000FFD RID: 4093 RVA: 0x0009E0D3 File Offset: 0x0009C4D3
	protected virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x0400198F RID: 6543
	public const float X_IN = 1280f;

	// Token: 0x04001990 RID: 6544
	public const float X_OUT = -1280f;

	// Token: 0x04001991 RID: 6545
	[SerializeField]
	private bool customStart;

	// Token: 0x04001992 RID: 6546
	[SerializeField]
	private float spawnY;

	// Token: 0x04001993 RID: 6547
	[SerializeField]
	private bool usePrefabY;

	// Token: 0x04001994 RID: 6548
	[SerializeField]
	[Range(0f, 2000f)]
	private float speed = 100f;

	// Token: 0x04001995 RID: 6549
	[SerializeField]
	private float minSpacing = 50f;

	// Token: 0x04001996 RID: 6550
	[SerializeField]
	private float averageSpacing = 100f;

	// Token: 0x04001997 RID: 6551
	[SerializeField]
	private int sortingOrder;

	// Token: 0x04001998 RID: 6552
	[SerializeField]
	private ScrollingSpriteSpawner.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x04001999 RID: 6553
	private bool pauseScrolling;

	// Token: 0x02000440 RID: 1088
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x0400199A RID: 6554
		public SpriteRenderer sprite;

		// Token: 0x0400199B RID: 6555
		public float weight = 1f;
	}
}
