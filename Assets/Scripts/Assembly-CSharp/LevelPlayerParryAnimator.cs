using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A34 RID: 2612
public class LevelPlayerParryAnimator : AbstractMonoBehaviour
{
	// Token: 0x06003E1D RID: 15901 RVA: 0x00223480 File Offset: 0x00221880
	protected override void Awake()
	{
		base.Awake();
		this.r = base.GetComponent<SpriteRenderer>();
		foreach (Sprite sprite in this.sprites)
		{
			sprite.name = sprite.name.Replace("_pink", string.Empty);
		}
	}

	// Token: 0x06003E1E RID: 15902 RVA: 0x002234DC File Offset: 0x002218DC
	private void Set()
	{
		foreach (Sprite sprite in this.sprites)
		{
			if (sprite.name.Contains(this.r.sprite.name))
			{
				this.r.sprite = sprite;
				return;
			}
		}
	}

	// Token: 0x06003E1F RID: 15903 RVA: 0x00223535 File Offset: 0x00221935
	public void StartSet()
	{
		base.StartCoroutine(this.set_cr());
		base.StartCoroutine(this.setLate_cr());
	}

	// Token: 0x06003E20 RID: 15904 RVA: 0x00223551 File Offset: 0x00221951
	public void StopSet()
	{
		this.StopAllCoroutines();
	}

	// Token: 0x06003E21 RID: 15905 RVA: 0x0022355C File Offset: 0x0022195C
	private IEnumerator set_cr()
	{
		for (;;)
		{
			this.Set();
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003E22 RID: 15906 RVA: 0x00223578 File Offset: 0x00221978
	private IEnumerator setLate_cr()
	{
		for (;;)
		{
			this.Set();
			yield return new WaitForEndOfFrame();
		}
		yield break;
	}

	// Token: 0x04004557 RID: 17751
	[SerializeField]
	private Sprite[] sprites;

	// Token: 0x04004558 RID: 17752
	private SpriteRenderer r;
}
