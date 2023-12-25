using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004FC RID: 1276
public class BaronessLevelForegroundChange : AbstractPausableComponent
{
	// Token: 0x06001678 RID: 5752 RVA: 0x000C9C98 File Offset: 0x000C8098
	protected override void Awake()
	{
		base.Awake();
		this.bossNotDead = true;
		this.currentClones = new List<OneTimeScrollingSprite>();
		base.StartCoroutine(this.start_phase2_cr());
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x000C9CC0 File Offset: 0x000C80C0
	private IEnumerator start_phase2_cr()
	{
		for (int i = 0; i < this.sprites.Length; i++)
		{
			this.sprites[i].speed = 0f;
		}
		foreach (OneTimeScrollingSprite oneTimeScrollingSprite in this.currentClones)
		{
			if (oneTimeScrollingSprite != null)
			{
				oneTimeScrollingSprite.GetComponent<OneTimeScrollingSprite>().speed = 0f;
			}
		}
		while (this.baroness.state != BaronessLevelCastle.State.Chase)
		{
			yield return null;
		}
		this.StartLoop();
		for (;;)
		{
			if (!this.baroness.pauseScrolling)
			{
				for (int j = 0; j < this.sprites.Length; j++)
				{
					this.sprites[j].speed = this.speed;
				}
				foreach (OneTimeScrollingSprite oneTimeScrollingSprite2 in this.currentClones)
				{
					if (oneTimeScrollingSprite2 != null)
					{
						oneTimeScrollingSprite2.GetComponent<OneTimeScrollingSprite>().speed = this.speed;
					}
				}
			}
			else
			{
				for (int k = 0; k < this.sprites.Length; k++)
				{
					this.sprites[k].speed = 0f;
				}
				foreach (OneTimeScrollingSprite oneTimeScrollingSprite3 in this.currentClones)
				{
					if (oneTimeScrollingSprite3 != null)
					{
						oneTimeScrollingSprite3.GetComponent<OneTimeScrollingSprite>().speed = 0f;
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x000C9CDB File Offset: 0x000C80DB
	private void StartLoop()
	{
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600167B RID: 5755 RVA: 0x000C9CEC File Offset: 0x000C80EC
	private IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		float leadTime = 0f / this.speed;
		float totalWeight = 0f;
		foreach (BaronessLevelForegroundChange.ScrollingSpriteInfo scrollingSpriteInfo in this.spritePrefabs)
		{
			totalWeight += scrollingSpriteInfo.weight;
		}
		float spacing = UnityEngine.Random.Range(0f, this.minSpacing) + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing);
		BaronessLevelForegroundChange.ScrollingSpriteInfo lastSpawned = null;
		for (;;)
		{
			if (this.bossNotDead && !this.baroness.pauseScrolling)
			{
				float waitTime = spacing / this.speed;
				if (leadTime > waitTime)
				{
					leadTime -= waitTime;
					yield return null;
				}
				else
				{
					if (leadTime > 0f)
					{
						waitTime -= leadTime;
						leadTime = 0f;
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, waitTime);
				}
				float maxP = totalWeight;
				if (lastSpawned != null)
				{
					maxP -= lastSpawned.weight;
					yield return null;
				}
				float p = UnityEngine.Random.Range(0f, maxP);
				float cumulativeWeight = 0f;
				BaronessLevelForegroundChange.ScrollingSpriteInfo toSpawn = lastSpawned;
				foreach (BaronessLevelForegroundChange.ScrollingSpriteInfo scrollingSpriteInfo2 in this.spritePrefabs)
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
				float x = -1280f - leadTime * this.speed + sprite.bounds.size.x / 2f;
				obj.transform.position = new Vector2(x, this.spawnY);
				obj.transform.SetParent(base.transform, false);
				sprite.sortingOrder = this.sortingOrder;
				OneTimeScrollingSprite scrollingSprite = obj.AddComponent<OneTimeScrollingSprite>();
				scrollingSprite.speed = this.speed;
				spacing = this.minSpacing + MathUtils.ExpRandom(this.averageSpacing - this.minSpacing) - sprite.bounds.size.x;
				this.OnSpawn(obj);
				lastSpawned = toSpawn;
				this.currentClones.Add(scrollingSprite);
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600167C RID: 5756 RVA: 0x000C9D07 File Offset: 0x000C8107
	protected virtual void OnSpawn(GameObject obj)
	{
	}

	// Token: 0x04001FCC RID: 8140
	public const float X_IN = -1280f;

	// Token: 0x04001FCD RID: 8141
	public const float X_OUT = 1280f;

	// Token: 0x04001FCE RID: 8142
	[SerializeField]
	private float spawnY;

	// Token: 0x04001FCF RID: 8143
	[SerializeField]
	[Range(0f, -2000f)]
	private float speed;

	// Token: 0x04001FD0 RID: 8144
	[SerializeField]
	[Range(0f, -2000f)]
	private float minSpacing;

	// Token: 0x04001FD1 RID: 8145
	[SerializeField]
	[Range(0f, -2000f)]
	private float averageSpacing;

	// Token: 0x04001FD2 RID: 8146
	[SerializeField]
	private int sortingOrder;

	// Token: 0x04001FD3 RID: 8147
	[SerializeField]
	private BaronessLevelForegroundChange.ScrollingSpriteInfo[] spritePrefabs;

	// Token: 0x04001FD4 RID: 8148
	[SerializeField]
	private BaronessLevelCastle baroness;

	// Token: 0x04001FD5 RID: 8149
	[SerializeField]
	private OneTimeScrollingSprite[] sprites;

	// Token: 0x04001FD6 RID: 8150
	private List<OneTimeScrollingSprite> currentClones;

	// Token: 0x04001FD7 RID: 8151
	private bool bossNotDead;

	// Token: 0x020004FD RID: 1277
	[Serializable]
	public class ScrollingSpriteInfo
	{
		// Token: 0x04001FD8 RID: 8152
		public SpriteRenderer sprite;

		// Token: 0x04001FD9 RID: 8153
		public float weight = 1f;
	}
}
