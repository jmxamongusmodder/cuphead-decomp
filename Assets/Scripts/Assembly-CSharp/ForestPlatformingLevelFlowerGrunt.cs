using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200087F RID: 2175
public class ForestPlatformingLevelFlowerGrunt : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x0600327C RID: 12924 RVA: 0x001D6494 File Offset: 0x001D4894
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.idle_audio_delayer_cr("level_flowergrunt", 2f, 4f));
		this.emitAudioFromObject.Add("level_flowergrunt");
		this.emitAudioFromObject.Add("level_flowergrunt_float");
	}

	// Token: 0x0600327D RID: 12925 RVA: 0x001D64E3 File Offset: 0x001D48E3
	protected override void OnStart()
	{
		base.OnStart();
		if (this.floating)
		{
			AudioManager.Play("level_flowergrunt_float");
			base.StartCoroutine(this.handle_float_cr());
		}
	}

	// Token: 0x0600327E RID: 12926 RVA: 0x001D6510 File Offset: 0x001D4910
	private IEnumerator handle_float_cr()
	{
		while (this.floating)
		{
			yield return null;
		}
		AudioManager.Play("level_flowergrunt_land");
		this.emitAudioFromObject.Add("level_flowergrunt_land");
		yield break;
	}

	// Token: 0x0600327F RID: 12927 RVA: 0x001D652B File Offset: 0x001D492B
	protected override void Die()
	{
		AudioManager.Play("level_flowergrunt_death");
		this.emitAudioFromObject.Add("level_flowergrunt_death");
		base.FrameDelayedCallback(new Action(this.Kill), 1);
	}

	// Token: 0x06003280 RID: 12928 RVA: 0x001D655B File Offset: 0x001D495B
	private void Kill()
	{
		base.Die();
	}

	// Token: 0x06003281 RID: 12929 RVA: 0x001D6564 File Offset: 0x001D4964
	private float adjustSpeed(float speed)
	{
		return UnityEngine.Random.Range(speed * 0.12f, speed);
	}
}
