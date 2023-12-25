using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000A35 RID: 2613
public class LevelPlayerParryController : AbstractLevelPlayerComponent, IParryAttack
{
	// Token: 0x1700055F RID: 1375
	// (get) Token: 0x06003E24 RID: 15908 RVA: 0x002236C6 File Offset: 0x00221AC6
	public LevelPlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x14000093 RID: 147
	// (add) Token: 0x06003E25 RID: 15909 RVA: 0x002236D0 File Offset: 0x00221AD0
	// (remove) Token: 0x06003E26 RID: 15910 RVA: 0x00223708 File Offset: 0x00221B08
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryStartEvent;

	// Token: 0x14000094 RID: 148
	// (add) Token: 0x06003E27 RID: 15911 RVA: 0x00223740 File Offset: 0x00221B40
	// (remove) Token: 0x06003E28 RID: 15912 RVA: 0x00223778 File Offset: 0x00221B78
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryEndEvent;

	// Token: 0x17000560 RID: 1376
	// (get) Token: 0x06003E29 RID: 15913 RVA: 0x002237AE File Offset: 0x00221BAE
	// (set) Token: 0x06003E2A RID: 15914 RVA: 0x002237B6 File Offset: 0x00221BB6
	public bool AttackParryUsed { get; set; }

	// Token: 0x17000561 RID: 1377
	// (get) Token: 0x06003E2B RID: 15915 RVA: 0x002237BF File Offset: 0x00221BBF
	// (set) Token: 0x06003E2C RID: 15916 RVA: 0x002237C7 File Offset: 0x00221BC7
	public bool HasHitEnemy { get; set; }

	// Token: 0x06003E2D RID: 15917 RVA: 0x002237D0 File Offset: 0x00221BD0
	private void Start()
	{
		base.player.motor.OnParryEvent += this.StartParry;
		base.player.motor.OnGroundedEvent += this.OnGround;
	}

	// Token: 0x06003E2E RID: 15918 RVA: 0x0022380A File Offset: 0x00221C0A
	private void OnGround()
	{
		this.AttackParryUsed = false;
	}

	// Token: 0x06003E2F RID: 15919 RVA: 0x00223813 File Offset: 0x00221C13
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = LevelPlayerParryController.ParryState.Ready;
	}

	// Token: 0x06003E30 RID: 15920 RVA: 0x00223822 File Offset: 0x00221C22
	private void StartParry()
	{
		this.state = LevelPlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		base.StartCoroutine(this.parry_cr());
	}

	// Token: 0x06003E31 RID: 15921 RVA: 0x00223850 File Offset: 0x00221C50
	private IEnumerator parry_cr()
	{
		this.effect.Create(base.player);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = LevelPlayerParryController.ParryState.Ready;
		if (this.OnParryEndEvent != null)
		{
			this.OnParryEndEvent();
		}
		this.HasHitEnemy = false;
		yield break;
	}

	// Token: 0x06003E32 RID: 15922 RVA: 0x0022386B File Offset: 0x00221C6B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effect = null;
	}

	// Token: 0x04004559 RID: 17753
	public const float DURATION = 0.2f;

	// Token: 0x0400455A RID: 17754
	private LevelPlayerParryController.ParryState state;

	// Token: 0x0400455B RID: 17755
	[SerializeField]
	private LevelPlayerParryEffect effect;

	// Token: 0x02000A36 RID: 2614
	public enum ParryState
	{
		// Token: 0x04004561 RID: 17761
		Init,
		// Token: 0x04004562 RID: 17762
		Ready,
		// Token: 0x04004563 RID: 17763
		Parrying
	}
}
