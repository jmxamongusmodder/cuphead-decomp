using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000510 RID: 1296
public class BeeLevelBackground : LevelProperties.Bee.Entity
{
	// Token: 0x0600170F RID: 5903 RVA: 0x000CF508 File Offset: 0x000CD908
	private void Start()
	{
		this.level = (Level.Current as BeeLevel);
		base.StartCoroutine(this.middle_cr());
	}

	// Token: 0x06001710 RID: 5904 RVA: 0x000CF527 File Offset: 0x000CD927
	private void Update()
	{
		this.back.speed = -this.level.Speed * 0.35f;
	}

	// Token: 0x06001711 RID: 5905 RVA: 0x000CF548 File Offset: 0x000CD948
	public override void LevelInit(LevelProperties.Bee properties)
	{
		base.LevelInit(properties);
		int[] array = new int[this.groups.Length];
		List<int> list = new List<int>();
		for (int i = 0; i < this.groups.Length; i++)
		{
			list.Add(i);
		}
		for (int j = 0; j < this.groups.Length; j++)
		{
			int index = UnityEngine.Random.Range(0, list.Count);
			array[j] = list[index];
			list.RemoveAt(index);
			this.groups[array[j]].Init(this.platformGroup, this.groups.Length);
			this.groups[array[j]].SetY(-455f * (float)j);
		}
		this.platformGroup.gameObject.SetActive(false);
	}

	// Token: 0x06001712 RID: 5906 RVA: 0x000CF610 File Offset: 0x000CDA10
	private IEnumerator middle_cr()
	{
		SpriteRenderer[] sprites = new SpriteRenderer[this.middleGroups.Length];
		for (int j = 0; j < this.middleGroups.Length; j++)
		{
			sprites[j] = this.middleGroups[j].GetComponentInChildren<SpriteRenderer>();
			this.middleGroups[j].gameObject.SetActive(false);
		}
		int scale = (UnityEngine.Random.value <= 0.5f) ? -1 : 1;
		for (;;)
		{
			int i = UnityEngine.Random.Range(0, this.middleGroups.Length);
			float height = (float)((int)sprites[i].sprite.bounds.size.y);
			float y = (720f + height) / 2f;
			this.middleGroups[i].gameObject.SetActive(true);
			this.middleGroups[i].SetPosition(new float?(0f), new float?(y), new float?(0f));
			this.middleGroups[i].SetScale(new float?((float)scale), new float?(1f), new float?(1f));
			while (this.middleGroups[i].position.y >= -y)
			{
				this.middleGroups[i].AddPosition(0f, this.level.Speed * 0.75f * CupheadTime.Delta, 0f);
				yield return null;
			}
			this.middleGroups[i].gameObject.SetActive(false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04002056 RID: 8278
	public const float GROUP_OFFSET = 455f;

	// Token: 0x04002057 RID: 8279
	[SerializeField]
	private BeeLevelPlatforms platformGroup;

	// Token: 0x04002058 RID: 8280
	[SerializeField]
	private BeeLevelBackgroundGroup[] groups;

	// Token: 0x04002059 RID: 8281
	[SerializeField]
	private Transform[] middleGroups;

	// Token: 0x0400205A RID: 8282
	[SerializeField]
	private ScrollingSprite back;

	// Token: 0x0400205B RID: 8283
	private BeeLevel level;
}
