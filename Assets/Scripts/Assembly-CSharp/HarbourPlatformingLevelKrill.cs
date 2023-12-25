using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CB RID: 2251
public class HarbourPlatformingLevelKrill : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600349A RID: 13466 RVA: 0x001E88B0 File Offset: 0x001E6CB0
	protected override void Start()
	{
		base.Start();
		this.gravity = base.Properties.krillGravity;
		this.velocity.x = UnityEngine.Random.Range(-base.Properties.krillVelocityX.min, -base.Properties.krillVelocityX.max);
		this.velocity.y = UnityEngine.Random.Range(base.Properties.krillVelocityY.min, base.Properties.krillVelocityY.max);
		this._canParry = this.isParryable;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600349B RID: 13467 RVA: 0x001E894F File Offset: 0x001E6D4F
	protected override void OnStart()
	{
	}

	// Token: 0x0600349C RID: 13468 RVA: 0x001E8951 File Offset: 0x001E6D51
	public void SetType(string type)
	{
		base.GetComponent<PlatformingLevelEnemyAnimationHandler>().SelectAnimation(type);
	}

	// Token: 0x0600349D RID: 13469 RVA: 0x001E8960 File Offset: 0x001E6D60
	private IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.Properties.krillLaunchDelay);
		this.JumpSFX();
		for (;;)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600349E RID: 13470 RVA: 0x001E897B File Offset: 0x001E6D7B
	private void JumpSFX()
	{
		AudioManager.Play("harbour_shrimp_jump");
		this.emitAudioFromObject.Add("harbour_shrimp_jump");
	}

	// Token: 0x0600349F RID: 13471 RVA: 0x001E8997 File Offset: 0x001E6D97
	protected override void Die()
	{
		AudioManager.Play("harbour_krill_death");
		this.emitAudioFromObject.Add("harbour_krill_death");
		base.Die();
	}

	// Token: 0x04003CC5 RID: 15557
	private Vector2 velocity;

	// Token: 0x04003CC6 RID: 15558
	private float gravity;

	// Token: 0x04003CC7 RID: 15559
	public bool isParryable;
}
