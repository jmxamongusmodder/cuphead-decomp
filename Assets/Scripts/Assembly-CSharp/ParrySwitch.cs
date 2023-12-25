using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000B25 RID: 2853
public class ParrySwitch : AbstractSwitch
{
	// Token: 0x140000D2 RID: 210
	// (add) Token: 0x0600451C RID: 17692 RVA: 0x000B2C20 File Offset: 0x000B1020
	// (remove) Token: 0x0600451D RID: 17693 RVA: 0x000B2C58 File Offset: 0x000B1058
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnPrePauseActivate;

	// Token: 0x1700062A RID: 1578
	// (get) Token: 0x0600451E RID: 17694 RVA: 0x000B2C8E File Offset: 0x000B108E
	// (set) Token: 0x0600451F RID: 17695 RVA: 0x000B2C96 File Offset: 0x000B1096
	public bool IsParryable { get; protected set; }

	// Token: 0x06004520 RID: 17696 RVA: 0x000B2C9F File Offset: 0x000B109F
	protected void FirePrePauseEvent()
	{
		if (this.OnPrePauseActivate != null)
		{
			this.OnPrePauseActivate();
		}
	}

	// Token: 0x06004521 RID: 17697 RVA: 0x000B2CB7 File Offset: 0x000B10B7
	protected override void Awake()
	{
		base.Awake();
		base.tag = "ParrySwitch";
		this.IsParryable = true;
		if (base.GetComponent<Collider2D>() == null)
		{
		}
	}

	// Token: 0x06004522 RID: 17698 RVA: 0x000B2CE2 File Offset: 0x000B10E2
	public virtual void OnParryPrePause(AbstractPlayerController player)
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
		this.FirePrePauseEvent();
	}

	// Token: 0x06004523 RID: 17699 RVA: 0x000B2D11 File Offset: 0x000B1111
	public virtual void OnParryPostPause(AbstractPlayerController player)
	{
		base.DispatchEvent();
	}

	// Token: 0x06004524 RID: 17700 RVA: 0x000B2D19 File Offset: 0x000B1119
	public void ActivateFromOtherSource()
	{
		if (this.parrySpark)
		{
			this.parrySpark.Create(base.transform.position);
		}
		base.DispatchEvent();
	}

	// Token: 0x06004525 RID: 17701 RVA: 0x000B2D48 File Offset: 0x000B1148
	public void StartParryCooldown()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.parryCooldown_cr());
	}

	// Token: 0x06004526 RID: 17702 RVA: 0x000B2D64 File Offset: 0x000B1164
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
		yield return null;
		yield break;
	}

	// Token: 0x06004527 RID: 17703 RVA: 0x000B2D7F File Offset: 0x000B117F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parrySpark = null;
	}

	// Token: 0x04004ADA RID: 19162
	[SerializeField]
	protected Effect parrySpark;

	// Token: 0x04004ADB RID: 19163
	[SerializeField]
	protected float coolDown = 0.4f;
}
