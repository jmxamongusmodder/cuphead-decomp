using System;
using UnityEngine;

// Token: 0x02000A37 RID: 2615
public class LevelPlayerParryEffect : AbstractParryEffect
{
	// Token: 0x17000562 RID: 1378
	// (get) Token: 0x06003E34 RID: 15924 RVA: 0x00223969 File Offset: 0x00221D69
	protected override bool IsHit
	{
		get
		{
			return (this.player as LevelPlayerController).motor.IsHit;
		}
	}

	// Token: 0x17000563 RID: 1379
	// (get) Token: 0x06003E35 RID: 15925 RVA: 0x00223980 File Offset: 0x00221D80
	private LevelPlayerController levelPlayer
	{
		get
		{
			return this.player as LevelPlayerController;
		}
	}

	// Token: 0x06003E36 RID: 15926 RVA: 0x00223990 File Offset: 0x00221D90
	protected override void SetPlayer(AbstractPlayerController player)
	{
		base.SetPlayer(player);
		if (player.stats.Loadout.charm == Charm.charm_chalice)
		{
			base.GetComponent<CircleCollider2D>().offset = new Vector2(29.5f, 10f);
			base.GetComponent<CircleCollider2D>().radius = 80f;
		}
		this.levelPlayer.motor.OnHitEvent += this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent += this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent += this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart += this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart += this.OnWeaponCancel;
	}

	// Token: 0x06003E37 RID: 15927 RVA: 0x00223A78 File Offset: 0x00221E78
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.levelPlayer.motor.OnHitEvent -= this.OnHitCancel;
		this.levelPlayer.motor.OnGroundedEvent -= this.OnGroundedCancel;
		this.levelPlayer.motor.OnDashStartEvent -= this.OnDashCancel;
		this.levelPlayer.weaponManager.OnExStart -= this.OnWeaponCancel;
		this.levelPlayer.weaponManager.OnSuperStart -= this.OnWeaponCancel;
	}

	// Token: 0x06003E38 RID: 15928 RVA: 0x00223B18 File Offset: 0x00221F18
	protected override void OnHitCancel()
	{
		base.OnHitCancel();
		this.levelPlayer.motor.OnParryHit();
	}

	// Token: 0x06003E39 RID: 15929 RVA: 0x00223B30 File Offset: 0x00221F30
	private void OnDashCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003E3A RID: 15930 RVA: 0x00223B50 File Offset: 0x00221F50
	private void OnGroundedCancel()
	{
		if (this.player.stats.isChalice)
		{
			return;
		}
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003E3B RID: 15931 RVA: 0x00223B86 File Offset: 0x00221F86
	private void OnWeaponCancel()
	{
		if (this.didHitSomething || this == null)
		{
			return;
		}
		this.Cancel();
	}

	// Token: 0x06003E3C RID: 15932 RVA: 0x00223BA6 File Offset: 0x00221FA6
	protected override void Cancel()
	{
		base.Cancel();
		this.levelPlayer.animationController.ResumeNormanAnim();
	}

	// Token: 0x06003E3D RID: 15933 RVA: 0x00223BBE File Offset: 0x00221FBE
	protected override void CancelSwitch()
	{
		base.CancelSwitch();
		this.levelPlayer.motor.OnParryCanceled();
	}

	// Token: 0x06003E3E RID: 15934 RVA: 0x00223BD6 File Offset: 0x00221FD6
	protected override void OnPaused()
	{
		base.OnPaused();
		this.levelPlayer.animationController.OnParryPause();
		this.levelPlayer.weaponManager.ParrySuccess();
	}

	// Token: 0x06003E3F RID: 15935 RVA: 0x00223BFE File Offset: 0x00221FFE
	protected override void OnUnpaused()
	{
		base.OnUnpaused();
		this.levelPlayer.animationController.ResumeNormanAnim();
		this.levelPlayer.motor.OnParryComplete();
	}

	// Token: 0x06003E40 RID: 15936 RVA: 0x00223C26 File Offset: 0x00222026
	protected override void OnSuccess()
	{
		base.OnSuccess();
		this.levelPlayer.weaponManager.ParrySuccess();
		this.levelPlayer.animationController.OnParrySuccess();
	}

	// Token: 0x06003E41 RID: 15937 RVA: 0x00223C4E File Offset: 0x0022204E
	protected override void OnEnd()
	{
		base.OnEnd();
		this.levelPlayer.motor.ResetChaliceDoubleJump();
		this.levelPlayer.animationController.OnParryAnimEnd();
	}

	// Token: 0x04004564 RID: 17764
	private const float CHALICE_X_OFFSET = 29.5f;

	// Token: 0x04004565 RID: 17765
	private const float CHALICE_Y_OFFSET = 10f;

	// Token: 0x04004566 RID: 17766
	private const float CHALICE_RADIUS = 80f;
}
