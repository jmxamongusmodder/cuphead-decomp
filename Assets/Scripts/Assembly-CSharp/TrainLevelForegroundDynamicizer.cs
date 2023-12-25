using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000818 RID: 2072
public class TrainLevelForegroundDynamicizer : AbstractPausableComponent
{
	// Token: 0x0600300B RID: 12299 RVA: 0x001C631E File Offset: 0x001C471E
	protected override void Awake()
	{
		base.Awake();
		this.ResetPositions();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600300C RID: 12300 RVA: 0x001C633C File Offset: 0x001C473C
	private void ResetPositions()
	{
		foreach (SpriteRenderer spriteRenderer in this.sprites)
		{
			spriteRenderer.transform.SetLocalPosition(new float?(-1280f), new float?(0f), new float?(0f));
		}
	}

	// Token: 0x0600300D RID: 12301 RVA: 0x001C6394 File Offset: 0x001C4794
	private IEnumerator loop_cr()
	{
		for (;;)
		{
			this.ResetPositions();
			float t = 0f;
			Transform trans = this.sprites[UnityEngine.Random.Range(0, this.sprites.Length)].transform;
			trans.SetScale(new float?((float)((UnityEngine.Random.value <= 0.5f) ? -1 : 1)), new float?(1f), new float?(1f));
			while (t < 1.3f)
			{
				float x = Mathf.Lerp(1280f, -1280f, t / 1.3f);
				trans.SetLocalPosition(new float?(x), new float?(0f), new float?(0f));
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(4f, 4f));
		}
		yield break;
	}

	// Token: 0x040038DF RID: 14559
	public const float X_OUT = -1280f;

	// Token: 0x040038E0 RID: 14560
	public const float X_IN = 1280f;

	// Token: 0x040038E1 RID: 14561
	public const float DELAY_MIN = 1f;

	// Token: 0x040038E2 RID: 14562
	public const float DELAY_MAX = 4f;

	// Token: 0x040038E3 RID: 14563
	public const float TIME = 1.3f;

	// Token: 0x040038E4 RID: 14564
	[SerializeField]
	private SpriteRenderer[] sprites;
}
