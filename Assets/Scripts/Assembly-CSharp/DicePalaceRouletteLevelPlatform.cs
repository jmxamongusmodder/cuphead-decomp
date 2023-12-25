using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005E4 RID: 1508
public class DicePalaceRouletteLevelPlatform : ParrySwitch
{
	// Token: 0x17000372 RID: 882
	// (get) Token: 0x06001DE1 RID: 7649 RVA: 0x00112D52 File Offset: 0x00111152
	// (set) Token: 0x06001DE2 RID: 7650 RVA: 0x00112D7A File Offset: 0x0011117A
	public new bool enabled
	{
		get
		{
			return base.GetComponent<CircleCollider2D>().enabled && !this.platform.GetComponent<BoxCollider2D>().enabled;
		}
		set
		{
			base.GetComponent<CircleCollider2D>().enabled = value;
			this.platform.GetComponent<BoxCollider2D>().enabled = !value;
		}
	}

	// Token: 0x06001DE3 RID: 7651 RVA: 0x00112D9C File Offset: 0x0011119C
	private void Start()
	{
		this.maxCounter = UnityEngine.Random.Range(1, 4);
		base.animator.SetBool("isOffset", this.isOffset);
		this.enabled = true;
		base.StartCoroutine(this.sparkles_cr());
	}

	// Token: 0x06001DE4 RID: 7652 RVA: 0x00112DD5 File Offset: 0x001111D5
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.enabled = false;
		base.animator.SetBool("isFlipped", !this.enabled);
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001DE5 RID: 7653 RVA: 0x00112E04 File Offset: 0x00111204
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.platformOpenDuration);
		this.enabled = true;
		base.animator.SetBool("isFlipped", !this.enabled);
		yield break;
	}

	// Token: 0x06001DE6 RID: 7654 RVA: 0x00112E1F File Offset: 0x0011121F
	public void Init(LevelProperties.DicePalaceRoulette.Platform properties)
	{
		this.properties = properties;
	}

	// Token: 0x06001DE7 RID: 7655 RVA: 0x00112E28 File Offset: 0x00111228
	private void CheckSheen()
	{
		if (this.counter < this.maxCounter)
		{
			this.sheen.enabled = false;
			this.counter++;
		}
		else
		{
			this.sheen.enabled = true;
			this.maxCounter = UnityEngine.Random.Range(1, 4);
			this.counter = 0;
		}
	}

	// Token: 0x06001DE8 RID: 7656 RVA: 0x00112E88 File Offset: 0x00111288
	private IEnumerator sparkles_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.25f, 1f));
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_1") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle_2"))
			{
				base.animator.SetTrigger("onSparkle");
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x040026AE RID: 9902
	[SerializeField]
	private SpriteRenderer sheen;

	// Token: 0x040026AF RID: 9903
	[SerializeField]
	private bool isOffset;

	// Token: 0x040026B0 RID: 9904
	[SerializeField]
	private GameObject platform;

	// Token: 0x040026B1 RID: 9905
	private LevelProperties.DicePalaceRoulette.Platform properties;

	// Token: 0x040026B2 RID: 9906
	private Color pink;

	// Token: 0x040026B3 RID: 9907
	private int maxCounter;

	// Token: 0x040026B4 RID: 9908
	private int counter;
}
