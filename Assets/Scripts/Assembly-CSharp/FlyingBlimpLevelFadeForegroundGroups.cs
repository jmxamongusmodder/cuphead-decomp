using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000639 RID: 1593
public class FlyingBlimpLevelFadeForegroundGroups : FlyingBlimpLevelScrollingSpriteSpawnerBase
{
	// Token: 0x060020AB RID: 8363 RVA: 0x0012D820 File Offset: 0x0012BC20
	protected override void Awake()
	{
		base.Awake();
		this.fadeTime = 10f;
		this.daySprites = new List<Transform>();
		this.nightSprites = new List<Transform>();
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
		}
		for (int i = 0; i < this.spritePrefabs.Length; i++)
		{
			foreach (Transform transform in this.spritePrefabs[i].sprite.transform.GetChildTransforms())
			{
				this.daySprites.Add(transform.transform);
				this.nightSprites.Add(transform.transform.GetChild(0));
			}
		}
		for (int k = 0; k < this.nightSprites.Count; k++)
		{
			if (this.nightSprites[k].transform != null)
			{
				this.nightSprites[k].transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
			}
		}
	}

	// Token: 0x060020AC RID: 8364 RVA: 0x0012D950 File Offset: 0x0012BD50
	protected override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		this.spawnedChild = obj.transform.GetChild(0);
	}

	// Token: 0x060020AD RID: 8365 RVA: 0x0012D96B File Offset: 0x0012BD6B
	private void Update()
	{
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x060020AE RID: 8366 RVA: 0x0012D996 File Offset: 0x0012BD96
	private void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x060020AF RID: 8367 RVA: 0x0012D9A8 File Offset: 0x0012BDA8
	private IEnumerator change_cr()
	{
		float t = 0f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			for (int i = 0; i < this.nightSprites.Count; i++)
			{
				if (this.nightSprites[i].transform != null)
				{
					this.nightSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.nightSprites[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		for (int j = 0; j < this.nightSprites.Count; j++)
		{
			if (this.nightSprites[j].transform != null)
			{
				this.nightSprites[j].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			}
		}
		yield break;
	}

	// Token: 0x0400292F RID: 10543
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x04002930 RID: 10544
	private List<Transform> daySprites;

	// Token: 0x04002931 RID: 10545
	private List<Transform> nightSprites;

	// Token: 0x04002932 RID: 10546
	private Transform spawnedChild;

	// Token: 0x04002933 RID: 10547
	private float fadeTime;

	// Token: 0x04002934 RID: 10548
	private int index;

	// Token: 0x04002935 RID: 10549
	private int allDayChildren;

	// Token: 0x04002936 RID: 10550
	private bool startedChange;
}
