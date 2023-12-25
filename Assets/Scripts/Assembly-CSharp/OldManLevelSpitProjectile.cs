using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000713 RID: 1811
public class OldManLevelSpitProjectile : AbstractProjectile
{
	// Token: 0x0600275B RID: 10075 RVA: 0x001719F8 File Offset: 0x0016FDF8
	public void Move(Vector3 position, float speedX, float speedY, float stopPosX, float gravity, float apexTime, int count)
	{
		base.transform.position = position;
		this.speed = new Vector3(speedX, speedY);
		this.stopPosX = stopPosX;
		this.gravity = gravity;
		this.apexTime = apexTime;
		this.smokeTimer = this.firstSmokeDelay;
		this.count = count;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.changeAnimations_cr());
		this.SFX_OMM_MouthCauldron_ProjLoop();
	}

	// Token: 0x0600275C RID: 10076 RVA: 0x00171A6A File Offset: 0x0016FE6A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600275D RID: 10077 RVA: 0x00171A88 File Offset: 0x0016FE88
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.GetComponent<SpriteRenderer>().color = ((!parryable) ? Color.white : Color.magenta);
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x00171AB4 File Offset: 0x0016FEB4
	private IEnumerator changeAnimations_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.apexTime + 0.22916667f);
		base.animator.SetTrigger("OnApex");
		yield return null;
		yield break;
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x00171AD0 File Offset: 0x0016FED0
	private IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.22916667f);
		while (base.transform.position.x > this.stopPosX)
		{
			this.speed += new Vector3(this.gravity * CupheadTime.FixedDelta, 0f);
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		float time = 0.6f;
		float t = 0f;
		while (base.transform.position.x > (float)Level.Current.Left - 100f)
		{
			if (base.transform.position.x <= (float)Level.Current.Left)
			{
				this.SFX_OMM_MouthCauldron_ProjLoopEnd();
			}
			if (this.speed.y > 0f)
			{
				this.speed.y = Mathf.Lerp(this.speed.y, 0f, t / time);
				t += CupheadTime.FixedDelta;
			}
			base.transform.Translate(this.speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
		}
		this.SFX_OMM_MouthCauldron_ProjLoopEnd();
		this.Recycle<OldManLevelSpitProjectile>();
		yield break;
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x00171AEC File Offset: 0x0016FEEC
	protected override void Update()
	{
		base.Update();
		this.smokeTimer -= CupheadTime.Delta;
		if (this.smokeTimer <= 0f)
		{
			this.smokeTimer += this.smokeDelay;
			((OldManLevel)Level.Current).CreateFX(base.transform.position, false, base.CanParry);
		}
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x00171B5A File Offset: 0x0016FF5A
	private void AnimationEvent_SFX_OMM_MouthCauldron_ProjStart()
	{
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_loop_start");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_start");
	}

	// Token: 0x06002762 RID: 10082 RVA: 0x00171B78 File Offset: 0x0016FF78
	private void SFX_OMM_MouthCauldron_ProjLoop()
	{
		AudioManager.PlayLoop("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
	}

	// Token: 0x06002763 RID: 10083 RVA: 0x00171BCB File Offset: 0x0016FFCB
	private void AnimationEvent_SFX_OMM_MouthCauldron_ProjHitPlayer()
	{
		this.SFX_OMM_MouthCauldron_ProjLoopEnd();
		AudioManager.Play("sfx_dlc_omm_mouthcauldron_projectile_damageplayer");
		this.emitAudioFromObject.Add("sfx_dlc_omm_mouthcauldron_projectile_damageplayer");
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x00171BED File Offset: 0x0016FFED
	private void SFX_OMM_MouthCauldron_ProjLoopEnd()
	{
		AudioManager.Stop("sfx_dlc_omm_mouthcauldron_projectile_loop_0" + this.count.ToString());
	}

	// Token: 0x04003019 RID: 12313
	private const float OFFSCREEN_OFFSET = 100f;

	// Token: 0x0400301A RID: 12314
	private Vector3 speed;

	// Token: 0x0400301B RID: 12315
	private float gravity;

	// Token: 0x0400301C RID: 12316
	private float stopPosX;

	// Token: 0x0400301D RID: 12317
	private float apexTime;

	// Token: 0x0400301E RID: 12318
	[SerializeField]
	private float firstSmokeDelay = 1f;

	// Token: 0x0400301F RID: 12319
	[SerializeField]
	private float smokeDelay = 0.05f;

	// Token: 0x04003020 RID: 12320
	private float smokeTimer;

	// Token: 0x04003021 RID: 12321
	private int count;
}
