using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AF2 RID: 2802
public class DamageReceiver : AbstractPausableComponent
{
	// Token: 0x17000613 RID: 1555
	// (get) Token: 0x060043F2 RID: 17394 RVA: 0x0023BCDD File Offset: 0x0023A0DD
	// (set) Token: 0x060043F3 RID: 17395 RVA: 0x0023BCE5 File Offset: 0x0023A0E5
	public bool IsHitPaused { get; private set; }

	// Token: 0x140000BF RID: 191
	// (add) Token: 0x060043F4 RID: 17396 RVA: 0x0023BCF0 File Offset: 0x0023A0F0
	// (remove) Token: 0x060043F5 RID: 17397 RVA: 0x0023BD28 File Offset: 0x0023A128
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event DamageReceiver.OnDamageTakenHandler OnDamageTaken;

	// Token: 0x060043F6 RID: 17398 RVA: 0x0023BD60 File Offset: 0x0023A160
	protected override void Awake()
	{
		base.Awake();
		if (base.animator != null)
		{
			this.animHelper = base.animator.GetComponent<AnimationHelper>();
		}
		if (this.type == DamageReceiver.Type.Other)
		{
			return;
		}
		base.tag = this.type.ToString();
		this.IsHitPaused = false;
	}

	// Token: 0x060043F7 RID: 17399 RVA: 0x0023BDC0 File Offset: 0x0023A1C0
	public virtual void TakeDamage(DamageDealer.DamageInfo info)
	{
		if (!base.enabled)
		{
			return;
		}
		if (this.OnDamageTaken != null)
		{
			if (DamageReceiver.DEBUG_DO_MEGA_DAMAGE && (this.type == DamageReceiver.Type.Enemy || this.type == DamageReceiver.Type.Other))
			{
				info.SetEditorPlayer();
			}
			this.OnDamageTaken(info);
			if ((this.type == DamageReceiver.Type.Enemy || this.type == DamageReceiver.Type.Other) && info.damageSource == DamageDealer.DamageSource.Super)
			{
				base.StartCoroutine(this.pauseAnim_cr());
			}
		}
	}

	// Token: 0x060043F8 RID: 17400 RVA: 0x0023BE47 File Offset: 0x0023A247
	public virtual void TakeDamageBruteForce(DamageDealer.DamageInfo info)
	{
		if (this.OnDamageTaken != null)
		{
			this.OnDamageTaken(info);
		}
	}

	// Token: 0x060043F9 RID: 17401 RVA: 0x0023BE60 File Offset: 0x0023A260
	private IEnumerator pauseAnim_cr()
	{
		if (this.animHelper != null)
		{
			this.animHelper.Speed = 0f;
		}
		if (base.animator != null)
		{
			base.animator.enabled = false;
		}
		for (int i = 0; i < this.animatorsEffectedByPause.Length; i++)
		{
			this.animatorsEffectedByPause[i].GetComponent<Animator>().enabled = false;
			this.animatorsEffectedByPause[i].Speed = 0f;
		}
		this.IsHitPaused = true;
		CupheadLevelCamera.Current.Shake(10f, 0.6f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.15f);
		this.IsHitPaused = false;
		if (base.animator != null)
		{
			base.animator.enabled = true;
		}
		if (this.animHelper != null)
		{
			this.animHelper.Speed = 1f;
		}
		for (int j = 0; j < this.animatorsEffectedByPause.Length; j++)
		{
			this.animatorsEffectedByPause[j].GetComponent<Animator>().enabled = true;
			this.animatorsEffectedByPause[j].Speed = 1f;
		}
		yield break;
	}

	// Token: 0x060043FA RID: 17402 RVA: 0x0023BE7C File Offset: 0x0023A27C
	public static void Debug_ToggleMegaDamage()
	{
		DamageReceiver.DEBUG_DO_MEGA_DAMAGE = !DamageReceiver.DEBUG_DO_MEGA_DAMAGE;
		string text = (!DamageReceiver.DEBUG_DO_MEGA_DAMAGE) ? "red" : "green";
	}

	// Token: 0x04004991 RID: 18833
	public const float ENEMY_HIT_PAUSE_TIME = 0.15f;

	// Token: 0x04004992 RID: 18834
	public DamageReceiver.Type type;

	// Token: 0x04004993 RID: 18835
	public AnimationHelper[] animatorsEffectedByPause;

	// Token: 0x04004994 RID: 18836
	[NonSerialized]
	public Vector2 OffScreenPadding = new Vector2(50f, 50f);

	// Token: 0x04004995 RID: 18837
	private AnimationHelper animHelper;

	// Token: 0x04004998 RID: 18840
	public static bool DEBUG_DO_MEGA_DAMAGE;

	// Token: 0x02000AF3 RID: 2803
	public enum Type
	{
		// Token: 0x0400499A RID: 18842
		Enemy,
		// Token: 0x0400499B RID: 18843
		Player,
		// Token: 0x0400499C RID: 18844
		Other
	}

	// Token: 0x02000AF4 RID: 2804
	// (Invoke) Token: 0x060043FD RID: 17405
	public delegate void OnDamageTakenHandler(DamageDealer.DamageInfo info);
}
