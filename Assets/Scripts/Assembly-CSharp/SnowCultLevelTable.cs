using System;
using UnityEngine;

// Token: 0x020007FA RID: 2042
public class SnowCultLevelTable : AbstractPausableComponent
{
	// Token: 0x06002EEE RID: 12014 RVA: 0x001BB0C8 File Offset: 0x001B94C8
	public void Intro(Vector3 startVel)
	{
		base.transform.parent = null;
		base.transform.position = this.wiz.transform.position;
		this.vel = startVel / CupheadTime.FixedDelta;
		this.chase = true;
		base.animator.Play("Intro");
		this.SFX_SNOWCULT_WizardTableCrystalballLoop();
	}

	// Token: 0x06002EEF RID: 12015 RVA: 0x001BB12A File Offset: 0x001B952A
	public void Outro()
	{
		this.chase = false;
		this.vel = (base.transform.position - this.lastPos) / CupheadTime.FixedDelta;
		this.outroTimer = 0.33333334f;
	}

	// Token: 0x06002EF0 RID: 12016 RVA: 0x001BB164 File Offset: 0x001B9564
	private void FixedUpdate()
	{
		if (!this.rend.enabled)
		{
			return;
		}
		this.lastPos = base.transform.position;
		base.transform.position += this.vel * CupheadTime.FixedDelta;
		if (this.chase)
		{
			this.vel += (this.wiz.transform.position - base.transform.position).normalized * this.accel * CupheadTime.FixedDelta;
			if (this.vel.magnitude > this.maxVel)
			{
				this.vel = this.vel.normalized * this.maxVel;
			}
			if (Vector2.Distance(this.wiz.transform.position, base.transform.position) > this.maxDistance)
			{
				base.transform.position = this.wiz.transform.position + (base.transform.position - this.wiz.transform.position).normalized * this.maxDistance;
			}
		}
		else
		{
			this.vel -= this.vel * this.decelOnDeactivate * CupheadTime.FixedDelta;
		}
		if (this.outroTimer > 0f)
		{
			this.outroTimer -= CupheadTime.FixedDelta;
			if (this.outroTimer <= 0f)
			{
				base.animator.Play("Outro");
				this.SFX_SNOWCULT_WizardTableDisappear();
			}
		}
	}

	// Token: 0x06002EF1 RID: 12017 RVA: 0x001BB345 File Offset: 0x001B9745
	private void AnimationEvent_SFX_SNOWCULT_WizardTableAppear()
	{
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_appear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_appear");
	}

	// Token: 0x06002EF2 RID: 12018 RVA: 0x001BB361 File Offset: 0x001B9761
	private void SFX_SNOWCULT_WizardTableDisappear()
	{
		AudioManager.Stop("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
		AudioManager.Play("sfx_dlc_snowcult_p1_wizard_crystalball_disappear");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_disappear");
	}

	// Token: 0x06002EF3 RID: 12019 RVA: 0x001BB387 File Offset: 0x001B9787
	private void SFX_SNOWCULT_WizardTableCrystalballLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
		AudioManager.FadeSFXVolume("sfx_dlc_snowcult_p1_wizard_crystalball_loop", 0.15f, 1f);
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_wizard_crystalball_loop");
	}

	// Token: 0x040037A3 RID: 14243
	[SerializeField]
	private SnowCultLevelWizard wiz;

	// Token: 0x040037A4 RID: 14244
	private Vector3 vel;

	// Token: 0x040037A5 RID: 14245
	private bool chase;

	// Token: 0x040037A6 RID: 14246
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x040037A7 RID: 14247
	[SerializeField]
	private float accel = 1f;

	// Token: 0x040037A8 RID: 14248
	[SerializeField]
	private float decelOnDeactivate = 1f;

	// Token: 0x040037A9 RID: 14249
	[SerializeField]
	private float maxVel = 200f;

	// Token: 0x040037AA RID: 14250
	[SerializeField]
	private float maxDistance = 20f;

	// Token: 0x040037AB RID: 14251
	private float outroTimer;

	// Token: 0x040037AC RID: 14252
	private Vector3 lastPos;
}
