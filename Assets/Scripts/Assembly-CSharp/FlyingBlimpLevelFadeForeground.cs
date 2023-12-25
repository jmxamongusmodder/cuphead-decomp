using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000638 RID: 1592
public class FlyingBlimpLevelFadeForeground : FlyingBlimpLevelScrollingSpriteSpawnerBase
{
	// Token: 0x060020A5 RID: 8357 RVA: 0x0012D394 File Offset: 0x0012B794
	protected override void Awake()
	{
		base.Awake();
		this.fadeTime = 10f;
		this.nightSprite = new Transform[this.spritePrefabs.Length];
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
		for (int i = 0; i < this.nightSprite.Length; i++)
		{
			this.nightSprite[i] = this.spritePrefabs[i].sprite.transform.GetChild(0);
			this.nightSprite[i].transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		}
	}

	// Token: 0x060020A6 RID: 8358 RVA: 0x0012D447 File Offset: 0x0012B847
	protected override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		this.spawnedChild = obj.transform.GetChild(0);
	}

	// Token: 0x060020A7 RID: 8359 RVA: 0x0012D462 File Offset: 0x0012B862
	private void Update()
	{
		if (this.moonLady.state == FlyingBlimpLevelMoonLady.State.Morph && !this.startedChange)
		{
			this.startedChange = true;
			this.StartChange();
		}
	}

	// Token: 0x060020A8 RID: 8360 RVA: 0x0012D48D File Offset: 0x0012B88D
	private void StartChange()
	{
		base.StartCoroutine(this.change_cr());
	}

	// Token: 0x060020A9 RID: 8361 RVA: 0x0012D49C File Offset: 0x0012B89C
	private IEnumerator change_cr()
	{
		float t = 0f;
		float startSpeed = this.speed;
		float endSpeed = this.speed + this.speed * 0.3f;
		while (t < this.fadeTime)
		{
			if (this.spawnedChild != null)
			{
				this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
				this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
			}
			for (int j = 0; j < this.nightSprite.Length; j++)
			{
				if (this.nightSprite[j].transform != null)
				{
					this.nightSprite[j].transform.gameObject.GetComponent<SpriteRenderer>().enabled = true;
					this.nightSprite[j].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / this.fadeTime);
				}
			}
			this.speed = Mathf.Lerp(startSpeed, endSpeed, t / this.fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		if (this.spawnedChild != null)
		{
			this.spawnedChild.transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		}
		for (int i = 0; i < this.nightSprite.Length; i++)
		{
			if (this.nightSprite[i].transform != null)
			{
				this.nightSprite[i].transform.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x04002929 RID: 10537
	[SerializeField]
	private FlyingBlimpLevelMoonLady moonLady;

	// Token: 0x0400292A RID: 10538
	private Transform[] nightSprite;

	// Token: 0x0400292B RID: 10539
	private Transform spawnedChild;

	// Token: 0x0400292C RID: 10540
	private float fadeTime;

	// Token: 0x0400292D RID: 10541
	private int index;

	// Token: 0x0400292E RID: 10542
	private bool startedChange;
}
