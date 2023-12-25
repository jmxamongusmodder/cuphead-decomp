using System;
using UnityEngine;

// Token: 0x0200074C RID: 1868
public class RetroArcadeBigPlayer : AbstractPausableComponent
{
	// Token: 0x060028B2 RID: 10418 RVA: 0x0017BD6C File Offset: 0x0017A16C
	public void Init(ArcadePlayerController player)
	{
		this.player = player;
		if (player == null)
		{
			base.gameObject.SetActive(false);
			return;
		}
		player.motor.OnHitEvent += this.OnHit;
		base.animator.SetBool("IsCuphead", this.isCuphead);
	}

	// Token: 0x060028B3 RID: 10419 RVA: 0x0017BDC8 File Offset: 0x0017A1C8
	private void FixedUpdate()
	{
		if (this.player == null || !this.trackingInputs)
		{
			return;
		}
		base.animator.SetBool("Dead", this.player.IsDead);
		base.animator.Update(Time.deltaTime);
		base.animator.Update(0f);
		if (base.animator.GetCurrentAnimatorStateInfo(3).IsName("Idle"))
		{
			if (this.player.input.actions.GetButtonDown(3))
			{
				base.animator.SetTrigger("A");
			}
			if (this.player.input.actions.GetButtonDown(2))
			{
				base.animator.SetTrigger("B");
			}
			if (this.player.input.actions.GetButtonDown(7))
			{
				base.animator.SetTrigger("C");
			}
			base.animator.SetInteger("MoveX", this.player.input.GetAxisInt(PlayerInput.Axis.X, false, false));
		}
		else
		{
			base.animator.Play("Idle", 2);
			base.animator.Play("Idle", 1);
			base.animator.SetInteger("MoveX", 0);
		}
	}

	// Token: 0x060028B4 RID: 10420 RVA: 0x0017BF27 File Offset: 0x0017A327
	private void OnHit()
	{
		base.animator.SetTrigger("Hit");
	}

	// Token: 0x060028B5 RID: 10421 RVA: 0x0017BF39 File Offset: 0x0017A339
	public void LevelStart()
	{
		this.trackingInputs = true;
	}

	// Token: 0x060028B6 RID: 10422 RVA: 0x0017BF42 File Offset: 0x0017A342
	public void OnVictory()
	{
		if (this.player != null && !this.player.IsDead)
		{
			base.animator.SetTrigger("Victory");
		}
	}

	// Token: 0x060028B7 RID: 10423 RVA: 0x0017BF75 File Offset: 0x0017A375
	private void PlayButtonASound()
	{
		AudioManager.Play("level_button_a");
		this.emitAudioFromObject.Add("level_button_a");
	}

	// Token: 0x060028B8 RID: 10424 RVA: 0x0017BF91 File Offset: 0x0017A391
	private void PlayButtonBSound()
	{
		AudioManager.Play("level_button_b");
		this.emitAudioFromObject.Add("level_button_b");
	}

	// Token: 0x060028B9 RID: 10425 RVA: 0x0017BFAD File Offset: 0x0017A3AD
	private void PlayButtonCSound()
	{
		AudioManager.Play("level_button_c");
		this.emitAudioFromObject.Add("level_button_c");
	}

	// Token: 0x0400318C RID: 12684
	private const int BOIL_LAYER = 0;

	// Token: 0x0400318D RID: 12685
	private const int BUTTON_LAYER = 1;

	// Token: 0x0400318E RID: 12686
	private const int STICK_LAYER = 2;

	// Token: 0x0400318F RID: 12687
	private const int MAIN_LAYER = 3;

	// Token: 0x04003190 RID: 12688
	private ArcadePlayerController player;

	// Token: 0x04003191 RID: 12689
	[SerializeField]
	private bool isCuphead;

	// Token: 0x04003192 RID: 12690
	private bool trackingInputs;
}
