using System;
using System.Collections;

// Token: 0x0200087B RID: 2171
public class ForestPlatformingLevelBlobRunner : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x0600326A RID: 12906 RVA: 0x001D5A03 File Offset: 0x001D3E03
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.idle_audio_delayer_cr("level_blobrunner", 2f, 4f));
		this.emitAudioFromObject.Add("level_blobrunner");
	}

	// Token: 0x0600326B RID: 12907 RVA: 0x001D5A37 File Offset: 0x001D3E37
	protected override void FixedUpdate()
	{
		if (!this.melted)
		{
			base.FixedUpdate();
		}
	}

	// Token: 0x0600326C RID: 12908 RVA: 0x001D5A4A File Offset: 0x001D3E4A
	protected override void Die()
	{
		this.IdleSounds = false;
		this.melted = true;
		this.collider.enabled = false;
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x0600326D RID: 12909 RVA: 0x001D5A74 File Offset: 0x001D3E74
	private IEnumerator melt_cr()
	{
		AudioManager.Stop("level_blobrunner");
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			AudioManager.Play("level_frogs_tall_firefly_death");
		}
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.BlobRunnerMeltDelay.RandomFloat());
		base.animator.SetTrigger("Continue");
		AudioManager.Play("level_blobrunner_reform");
		this.emitAudioFromObject.Add("level_blobrunner_reform");
		yield return CupheadTime.WaitForSeconds(this, base.Properties.BlobRunnerUnmeltLoopTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.collider.enabled = true;
		base.Health = base.Properties.Health;
		this.turning = false;
		this.timeSinceTurn = 10000f;
		this.IdleSounds = true;
		yield break;
	}

	// Token: 0x04003ACB RID: 15051
	private bool melted;
}
