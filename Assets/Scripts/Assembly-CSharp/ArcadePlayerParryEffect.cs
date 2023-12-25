using System;

// Token: 0x020009F6 RID: 2550
public class ArcadePlayerParryEffect : AbstractParryEffect
{
	// Token: 0x17000516 RID: 1302
	// (get) Token: 0x06003C2B RID: 15403 RVA: 0x00218955 File Offset: 0x00216D55
	protected override bool IsHit
	{
		get
		{
			return (this.player as ArcadePlayerController).motor.IsHit;
		}
	}

	// Token: 0x17000517 RID: 1303
	// (get) Token: 0x06003C2C RID: 15404 RVA: 0x0021896C File Offset: 0x00216D6C
	private ArcadePlayerController levelPlayer
	{
		get
		{
			return this.player as ArcadePlayerController;
		}
	}

	// Token: 0x06003C2D RID: 15405 RVA: 0x0021897C File Offset: 0x00216D7C
	protected override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		this.levelPlayer.motor.OnHitEvent += this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent += this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent += this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart += this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart += this.OnWeaponCancel;
	}

	// Token: 0x06003C2E RID: 15406 RVA: 0x00218A20 File Offset: 0x00216E20
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.levelPlayer.motor.OnHitEvent -= this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent -= this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent -= this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart -= this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart -= this.OnWeaponCancel;
	}

	// Token: 0x06003C2F RID: 15407 RVA: 0x00218AC0 File Offset: 0x00216EC0
	protected override void OnHitCancel()
	{
		base.OnHitCancel();
		this.levelPlayer.motor.OnParryHit();
	}

	// Token: 0x06003C30 RID: 15408 RVA: 0x00218AD8 File Offset: 0x00216ED8
	private void OnDashCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003C31 RID: 15409 RVA: 0x00218AF8 File Offset: 0x00216EF8
	private void OnGroundedCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003C32 RID: 15410 RVA: 0x00218B18 File Offset: 0x00216F18
	private void OnWeaponCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003C33 RID: 15411 RVA: 0x00218B38 File Offset: 0x00216F38
	protected override void Cancel()
	{
		base.Cancel();
		this.levelPlayer.animationController.ResumeNormanAnim();
	}

	// Token: 0x06003C34 RID: 15412 RVA: 0x00218B50 File Offset: 0x00216F50
	protected override void CancelSwitch()
	{
		base.CancelSwitch();
		this.levelPlayer.motor.OnParryCanceled();
	}

	// Token: 0x06003C35 RID: 15413 RVA: 0x00218B68 File Offset: 0x00216F68
	protected override void OnPaused()
	{
		base.OnPaused();
		this.levelPlayer.animationController.OnParryPause();
		this.levelPlayer.weaponManager.ParrySuccess();
	}

	// Token: 0x06003C36 RID: 15414 RVA: 0x00218B90 File Offset: 0x00216F90
	protected override void OnUnpaused()
	{
		base.OnUnpaused();
		this.levelPlayer.animationController.ResumeNormanAnim();
		this.levelPlayer.motor.OnParryComplete();
	}

	// Token: 0x06003C37 RID: 15415 RVA: 0x00218BB8 File Offset: 0x00216FB8
	protected override void OnSuccess()
	{
		base.OnSuccess();
		this.levelPlayer.weaponManager.ParrySuccess();
		this.levelPlayer.animationController.OnParrySuccess();
	}

	// Token: 0x06003C38 RID: 15416 RVA: 0x00218BE0 File Offset: 0x00216FE0
	protected override void OnEnd()
	{
		base.OnEnd();
		this.levelPlayer.animationController.OnParryAnimEnd();
	}
}
