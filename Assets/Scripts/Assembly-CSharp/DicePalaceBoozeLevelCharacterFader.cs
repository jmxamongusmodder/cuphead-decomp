using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200059F RID: 1439
public class DicePalaceBoozeLevelCharacterFader : AbstractPausableComponent
{
	// Token: 0x06001B9D RID: 7069 RVA: 0x000FB8C6 File Offset: 0x000F9CC6
	protected override void Awake()
	{
		base.Awake();
		this.mainSprite = base.GetComponent<SpriteRenderer>();
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06001B9E RID: 7070 RVA: 0x000FB8E8 File Offset: 0x000F9CE8
	private IEnumerator main_cr()
	{
		foreach (SpriteRenderer spriteRenderer in this.sprites)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		}
		this.mainSprite.color = new Color(1f, 1f, 1f, 0f);
		float fadeTime = 0.5f;
		for (;;)
		{
			float holdTime = UnityEngine.Random.Range(3f, 6f);
			yield return CupheadTime.WaitForSeconds(this, holdTime);
			if (this.fadingOut)
			{
				float t = 0f;
				while (t < fadeTime)
				{
					this.mainSprite.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					foreach (SpriteRenderer spriteRenderer2 in this.sprites)
					{
						spriteRenderer2.color = new Color(1f, 1f, 1f, 1f - t / fadeTime);
					}
					t += CupheadTime.Delta;
					yield return null;
				}
			}
			else
			{
				float t2 = 0f;
				while (t2 < fadeTime)
				{
					this.mainSprite.color = new Color(1f, 1f, 1f, t2 / fadeTime);
					foreach (SpriteRenderer spriteRenderer3 in this.sprites)
					{
						spriteRenderer3.color = new Color(1f, 1f, 1f, t2 / fadeTime);
					}
					t2 += CupheadTime.Delta;
					yield return null;
				}
			}
			this.fadingOut = !this.fadingOut;
		}
		yield break;
	}

	// Token: 0x040024AE RID: 9390
	[SerializeField]
	private SpriteRenderer[] sprites;

	// Token: 0x040024AF RID: 9391
	private SpriteRenderer mainSprite;

	// Token: 0x040024B0 RID: 9392
	private bool fadingOut;
}
