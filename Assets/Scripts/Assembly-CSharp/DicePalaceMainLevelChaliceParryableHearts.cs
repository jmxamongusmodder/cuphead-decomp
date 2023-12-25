using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
public class DicePalaceMainLevelChaliceParryableHearts : AbstractProjectile
{
	// Token: 0x14000041 RID: 65
	// (add) Token: 0x06001D45 RID: 7493 RVA: 0x0010CA94 File Offset: 0x0010AE94
	// (remove) Token: 0x06001D46 RID: 7494 RVA: 0x0010CACC File Offset: 0x0010AECC
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPrePauseActivate;

	// Token: 0x1700036B RID: 875
	// (get) Token: 0x06001D47 RID: 7495 RVA: 0x0010CB02 File Offset: 0x0010AF02
	// (set) Token: 0x06001D48 RID: 7496 RVA: 0x0010CB0A File Offset: 0x0010AF0A
	public bool IsParryable { get; protected set; }

	// Token: 0x1700036C RID: 876
	// (get) Token: 0x06001D49 RID: 7497 RVA: 0x0010CB13 File Offset: 0x0010AF13
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x0010CB1A File Offset: 0x0010AF1A
	protected override void Awake()
	{
		base.Awake();
		this.SetParryable(true);
		if (base.GetComponent<Collider2D>() == null)
		{
		}
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x0010CB3A File Offset: 0x0010AF3A
	public virtual void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x0010CB63 File Offset: 0x0010AF63
	public override void OnParry(AbstractPlayerController player)
	{
		this.SetParryable(false);
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x0010CB7C File Offset: 0x0010AF7C
	private IEnumerator parryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		this.SetParryable(true);
		yield return null;
		yield break;
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x0010CB97 File Offset: 0x0010AF97
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parrySpark = null;
	}

	// Token: 0x0400262D RID: 9773
	[SerializeField]
	private Effect parrySpark;

	// Token: 0x0400262E RID: 9774
	[SerializeField]
	protected float coolDown = 0.4f;
}
