using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020009F4 RID: 2548
public class ArcadePlayerParryController : AbstractArcadePlayerComponent
{
	// Token: 0x17000515 RID: 1301
	// (get) Token: 0x06003C21 RID: 15393 RVA: 0x00218717 File Offset: 0x00216B17
	public ArcadePlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
	}

	// Token: 0x1400007E RID: 126
	// (add) Token: 0x06003C22 RID: 15394 RVA: 0x00218720 File Offset: 0x00216B20
	// (remove) Token: 0x06003C23 RID: 15395 RVA: 0x00218758 File Offset: 0x00216B58
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryStartEvent;

	// Token: 0x1400007F RID: 127
	// (add) Token: 0x06003C24 RID: 15396 RVA: 0x00218790 File Offset: 0x00216B90
	// (remove) Token: 0x06003C25 RID: 15397 RVA: 0x002187C8 File Offset: 0x00216BC8
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryEndEvent;

	// Token: 0x06003C26 RID: 15398 RVA: 0x002187FE File Offset: 0x00216BFE
	private void Start()
	{
		base.player.motor.OnParryEvent += this.StartParry;
	}

	// Token: 0x06003C27 RID: 15399 RVA: 0x0021881C File Offset: 0x00216C1C
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = ArcadePlayerParryController.ParryState.Ready;
	}

	// Token: 0x06003C28 RID: 15400 RVA: 0x0021882B File Offset: 0x00216C2B
	private void StartParry()
	{
		this.state = ArcadePlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		base.StartCoroutine(this.parry_cr());
	}

	// Token: 0x06003C29 RID: 15401 RVA: 0x00218858 File Offset: 0x00216C58
	private IEnumerator parry_cr()
	{
		this.effect.Create(base.player);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = ArcadePlayerParryController.ParryState.Ready;
		if (this.OnParryEndEvent != null)
		{
			this.OnParryEndEvent();
		}
		yield break;
	}

	// Token: 0x040043A8 RID: 17320
	public const float DURATION = 0.2f;

	// Token: 0x040043A9 RID: 17321
	private ArcadePlayerParryController.ParryState state;

	// Token: 0x040043AA RID: 17322
	[SerializeField]
	private ArcadePlayerParryEffect effect;

	// Token: 0x020009F5 RID: 2549
	public enum ParryState
	{
		// Token: 0x040043AE RID: 17326
		Init,
		// Token: 0x040043AF RID: 17327
		Ready,
		// Token: 0x040043B0 RID: 17328
		Parrying
	}
}
