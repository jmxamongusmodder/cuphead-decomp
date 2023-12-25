using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200063D RID: 1597
public class FlyingBlimpLevelScrollingSpriteSpawnerBase : AbstractPausableComponent
{
	// Token: 0x060020CD RID: 8397 RVA: 0x0012CF37 File Offset: 0x0012B337
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060020CE RID: 8398 RVA: 0x0012CF4C File Offset: 0x0012B34C
	protected IEnumerator loop_cr()
	{
		float leadTime = 2560f / this.speed;
		float totalWeight = 0f;
		foreach (FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = UnityEngine.Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			float waitTime = spacing / this.speed;
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
			FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo toSpawn = lastSpawned;
			foreach (FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
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
			obj.transform.position = new Vector2(x, this.spawnY);
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

	// Token: 0x060020CF RID: 8399 RVA: 0x0012CF67 File Offset: 0x0012B367
	protected virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x060020D0 RID: 8400 RVA: 0x0012CF69 File Offset: 0x0012B369
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.spritePrefabs = null;
	}

	// Token: 0x04002962 RID: 10594
	public const float X_IN = 1280f;

	// Token: 0x04002963 RID: 10595
	public const float X_OUT = -1280f;

	// Token: 0x04002964 RID: 10596
	[SerializeField]
	protected float spawnY;

	// Token: 0x04002965 RID: 10597
	[SerializeField]
	[Range(0f, 2000f)]
	protected float speed = 100f;

	// Token: 0x04002966 RID: 10598
	[SerializeField]
	protected float minSpacing = 50f;

	// Token: 0x04002967 RID: 10599
	[SerializeField]
	protected float averageSpacing = 100f;

	// Token: 0x04002968 RID: 10600
	[SerializeField]
	protected int sortingOrder;

	// Token: 0x04002969 RID: 10601
	[SerializeField]
	protected FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x0200063E RID: 1598
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x0400296A RID: 10602
		public SpriteRenderer sprite;

		// Token: 0x0400296B RID: 10603
		public float weight = 1f;
	}
}
