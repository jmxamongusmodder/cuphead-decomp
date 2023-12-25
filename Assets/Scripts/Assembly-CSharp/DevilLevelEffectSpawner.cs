using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000573 RID: 1395
public class DevilLevelEffectSpawner : AbstractPausableComponent
{
	// Token: 0x06001A6F RID: 6767 RVA: 0x000F1DE3 File Offset: 0x000F01E3
	private void Start()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06001A70 RID: 6768 RVA: 0x000F1DF4 File Offset: 0x000F01F4
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0f, this.waitTime.max));
		yield return null;
		for (;;)
		{
			this.effect = this.effectPrefab.Create(base.transform.position);
			this.effect.transform.parent = base.transform;
			while (this.effect != null)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.waitTime.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x000F1E0F File Offset: 0x000F020F
	public void KillSmoke()
	{
		this.StopAllCoroutines();
		if (this.isSmoke3)
		{
			base.StartCoroutine(this.fade_out_cr());
		}
	}

	// Token: 0x06001A72 RID: 6770 RVA: 0x000F1E30 File Offset: 0x000F0230
	private IEnumerator fade_out_cr()
	{
		float t = 0f;
		float time = 0.5f;
		while (t < time)
		{
			t += CupheadTime.Delta;
			if (this.effect != null)
			{
				this.effect.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - t / time);
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x000F1E4B File Offset: 0x000F024B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x0400239B RID: 9115
	[SerializeField]
	private bool isSmoke3;

	// Token: 0x0400239C RID: 9116
	public MinMax waitTime;

	// Token: 0x0400239D RID: 9117
	public Effect effectPrefab;

	// Token: 0x0400239E RID: 9118
	private Effect effect;
}
