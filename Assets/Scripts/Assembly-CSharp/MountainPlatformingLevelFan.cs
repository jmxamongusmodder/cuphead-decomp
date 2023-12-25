using System;
using System.Collections;

// Token: 0x020008E2 RID: 2274
public class MountainPlatformingLevelFan : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600353B RID: 13627 RVA: 0x001F061E File Offset: 0x001EEA1E
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.check_to_start_cr());
	}

	// Token: 0x0600353C RID: 13628 RVA: 0x001F0634 File Offset: 0x001EEA34
	public float GetSpeed()
	{
		if (base.transform.localScale.x == 1f)
		{
			this.speed = -base.Properties.fanVelocity;
		}
		else
		{
			this.speed = base.Properties.fanVelocity;
		}
		return this.speed;
	}

	// Token: 0x0600353D RID: 13629 RVA: 0x001F068C File Offset: 0x001EEA8C
	protected override void OnStart()
	{
		base.StartCoroutine(this.fan_cr());
	}

	// Token: 0x0600353E RID: 13630 RVA: 0x001F069C File Offset: 0x001EEA9C
	private IEnumerator check_to_start_cr()
	{
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset)
		{
			yield return null;
		}
		this.OnStart();
		yield return null;
		yield break;
	}

	// Token: 0x0600353F RID: 13631 RVA: 0x001F06B7 File Offset: 0x001EEAB7
	private void FanOn()
	{
		this.PlayLionRoarSFX();
		this.fanOn = true;
	}

	// Token: 0x06003540 RID: 13632 RVA: 0x001F06C6 File Offset: 0x001EEAC6
	private void FanOff()
	{
		this.fanOn = false;
	}

	// Token: 0x06003541 RID: 13633 RVA: 0x001F06D0 File Offset: 0x001EEAD0
	private IEnumerator fan_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			base.animator.SetBool("IsFan", true);
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
			base.animator.SetBool("WindOn", true);
			yield return null;
			float t = 0f;
			float time = base.Properties.fanWaitTime.RandomFloat();
			while (t < time)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			base.animator.SetBool("IsFan", false);
			base.animator.SetBool("WindOn", false);
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Roar_End", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003542 RID: 13634 RVA: 0x001F06EB File Offset: 0x001EEAEB
	private void PlayLionRoarSFX()
	{
		AudioManager.Play("castle_rock_lion_roar");
		this.emitAudioFromObject.Add("castle_rock_lion_roar");
	}

	// Token: 0x06003543 RID: 13635 RVA: 0x001F0708 File Offset: 0x001EEB08
	protected override void Die()
	{
		AudioManager.Play("castle_rock_lion_death");
		this.emitAudioFromObject.Add("castle_rock_lion_death");
		base.animator.SetBool("WindOn", false);
		this.speed = 0f;
		this.fanOn = false;
		this.StopAllCoroutines();
		base.animator.SetTrigger("Death");
		base.Dead = true;
	}

	// Token: 0x04003D6B RID: 15723
	private float speed;

	// Token: 0x04003D6C RID: 15724
	private float offset = 50f;

	// Token: 0x04003D6D RID: 15725
	public bool fanOn;
}
