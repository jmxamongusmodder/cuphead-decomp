using System;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000AA0 RID: 2720
public class PlanePlayerParryController : AbstractPlanePlayerComponent, IParryAttack
{
	// Token: 0x170005B2 RID: 1458
	// (get) Token: 0x06004132 RID: 16690 RVA: 0x00236599 File Offset: 0x00234999
	// (set) Token: 0x06004133 RID: 16691 RVA: 0x002365A1 File Offset: 0x002349A1
	public PlanePlayerParryController.ParryState State
	{
		get
		{
			return this.state;
		}
		set
		{
			this.state = value;
		}
	}

	// Token: 0x140000A2 RID: 162
	// (add) Token: 0x06004134 RID: 16692 RVA: 0x002365AC File Offset: 0x002349AC
	// (remove) Token: 0x06004135 RID: 16693 RVA: 0x002365E4 File Offset: 0x002349E4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParryStartEvent;

	// Token: 0x140000A3 RID: 163
	// (add) Token: 0x06004136 RID: 16694 RVA: 0x0023661C File Offset: 0x00234A1C
	// (remove) Token: 0x06004137 RID: 16695 RVA: 0x00236654 File Offset: 0x00234A54
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnParrySuccessEvent;

	// Token: 0x170005B3 RID: 1459
	// (get) Token: 0x06004138 RID: 16696 RVA: 0x0023668A File Offset: 0x00234A8A
	// (set) Token: 0x06004139 RID: 16697 RVA: 0x00236692 File Offset: 0x00234A92
	public bool AttackParryUsed { get; set; }

	// Token: 0x170005B4 RID: 1460
	// (get) Token: 0x0600413A RID: 16698 RVA: 0x0023669B File Offset: 0x00234A9B
	// (set) Token: 0x0600413B RID: 16699 RVA: 0x002366A3 File Offset: 0x00234AA3
	public bool HasHitEnemy { get; set; }

	// Token: 0x0600413C RID: 16700 RVA: 0x002366AC File Offset: 0x00234AAC
	private void Start()
	{
		base.player.OnReviveEvent += this.OnRevive;
		base.player.stats.OnStoned += this.OnStoned;
	}

	// Token: 0x0600413D RID: 16701 RVA: 0x002366E4 File Offset: 0x00234AE4
	private void FixedUpdate()
	{
		PlanePlayerParryController.ParryState parryState = this.state;
		if (parryState != PlanePlayerParryController.ParryState.Ready)
		{
			if (parryState == PlanePlayerParryController.ParryState.Cooldown)
			{
				this.UpdateCooldown();
			}
		}
		else
		{
			this.UpdateReady();
		}
	}

	// Token: 0x0600413E RID: 16702 RVA: 0x00236728 File Offset: 0x00234B28
	private void UpdateReady()
	{
		if (base.player.Shrunk || base.player.WeaponBusy || base.player.stats.StoneTime > 0f)
		{
			return;
		}
		if (this.state != PlanePlayerParryController.ParryState.Ready)
		{
			return;
		}
		if (base.player.input.actions.GetButtonDown(2) || base.player.motor.HasBufferedInput(PlanePlayerMotor.BufferedInput.Jump))
		{
			base.player.motor.ClearBufferedInput();
			this.StartParry();
		}
	}

	// Token: 0x0600413F RID: 16703 RVA: 0x002367C4 File Offset: 0x00234BC4
	private void UpdateCooldown()
	{
		this.timeSinceParry += CupheadTime.FixedDelta;
		if (this.timeSinceParry > 0.3f)
		{
			this.state = PlanePlayerParryController.ParryState.Ready;
			this.AttackParryUsed = false;
		}
	}

	// Token: 0x06004140 RID: 16704 RVA: 0x002367F6 File Offset: 0x00234BF6
	public override void OnLevelStart()
	{
		base.OnLevelStart();
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x06004141 RID: 16705 RVA: 0x00236808 File Offset: 0x00234C08
	private void StartParry()
	{
		if (this.state != PlanePlayerParryController.ParryState.Ready)
		{
			return;
		}
		this.state = PlanePlayerParryController.ParryState.Parrying;
		if (this.OnParryStartEvent != null)
		{
			this.OnParryStartEvent();
		}
		this.effectInstance = (this.effect.Create(base.player) as PlanePlayerParryEffect);
		if (base.player.stats.isChalice)
		{
			this.effectInstance.GetComponent<CircleCollider2D>().radius *= 1.3f;
		}
	}

	// Token: 0x06004142 RID: 16706 RVA: 0x0023688C File Offset: 0x00234C8C
	public void OnParrySuccess()
	{
		if (this.OnParrySuccessEvent != null)
		{
			this.OnParrySuccessEvent();
		}
		this.state = PlanePlayerParryController.ParryState.Cooldown;
		this.timeSinceParry = 0f;
		if (this.effectInstance != null)
		{
			UnityEngine.Object.Destroy(this.effectInstance.gameObject);
		}
	}

	// Token: 0x06004143 RID: 16707 RVA: 0x002368E2 File Offset: 0x00234CE2
	private void OnParryEnd()
	{
		this.state = PlanePlayerParryController.ParryState.Cooldown;
		this.timeSinceParry = 0f;
		this.HasHitEnemy = false;
		if (this.effectInstance != null)
		{
			UnityEngine.Object.Destroy(this.effectInstance.gameObject);
		}
	}

	// Token: 0x06004144 RID: 16708 RVA: 0x0023691E File Offset: 0x00234D1E
	private void OnRevive(Vector3 pos)
	{
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x06004145 RID: 16709 RVA: 0x00236927 File Offset: 0x00234D27
	private void OnStoned()
	{
		this.state = PlanePlayerParryController.ParryState.Ready;
	}

	// Token: 0x06004146 RID: 16710 RVA: 0x00236930 File Offset: 0x00234D30
	private void SoundPlaneParry()
	{
		AudioManager.Play("player_plane_parry");
		this.emitAudioFromObject.Add("player_plane_parry");
	}

	// Token: 0x040047CF RID: 18383
	public const float COOLDOWN_DURATION = 0.3f;

	// Token: 0x040047D0 RID: 18384
	public const float CHALICE_PARRY_SIZE_MODIFIER = 1.3f;

	// Token: 0x040047D1 RID: 18385
	private PlanePlayerParryController.ParryState state;

	// Token: 0x040047D2 RID: 18386
	[SerializeField]
	private PlanePlayerParryEffect effect;

	// Token: 0x040047D5 RID: 18389
	private PlanePlayerParryEffect effectInstance;

	// Token: 0x040047D6 RID: 18390
	private float timeSinceParry;

	// Token: 0x02000AA1 RID: 2721
	public enum ParryState
	{
		// Token: 0x040047DA RID: 18394
		Init,
		// Token: 0x040047DB RID: 18395
		Ready,
		// Token: 0x040047DC RID: 18396
		Cooldown,
		// Token: 0x040047DD RID: 18397
		Parrying,
		// Token: 0x040047DE RID: 18398
		Disabled
	}
}
