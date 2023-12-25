using System;
using UnityEngine;

// Token: 0x02000880 RID: 2176
public class ForestPlatformingLevelLobber : PlatformingLevelShootingEnemy
{
	// Token: 0x06003283 RID: 12931 RVA: 0x001D663E File Offset: 0x001D4A3E
	protected override void Awake()
	{
		base.Awake();
		base.animator.Play("Idle", 0, UnityEngine.Random.Range(0f, 1f));
	}

	// Token: 0x06003284 RID: 12932 RVA: 0x001D6666 File Offset: 0x001D4A66
	protected override void Shoot()
	{
		base.Shoot();
	}

	// Token: 0x06003285 RID: 12933 RVA: 0x001D666E File Offset: 0x001D4A6E
	private void PlayLobberSound()
	{
		AudioManager.Play("level_forestlobber_shoot");
		this.emitAudioFromObject.Add("level_forestlobber_shoot");
	}

	// Token: 0x06003286 RID: 12934 RVA: 0x001D668A File Offset: 0x001D4A8A
	protected override void Die()
	{
		AudioManager.Play("level_mermaid_turtle_shell_pop");
		this.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop");
		base.FrameDelayedCallback(new Action(this.Kill), 1);
	}

	// Token: 0x06003287 RID: 12935 RVA: 0x001D66BA File Offset: 0x001D4ABA
	private void Kill()
	{
		base.Die();
	}

	// Token: 0x06003288 RID: 12936 RVA: 0x001D66C2 File Offset: 0x001D4AC2
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this._shootEffect = null;
	}
}
