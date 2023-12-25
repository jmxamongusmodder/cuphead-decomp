using System;
using UnityEngine;

// Token: 0x0200089C RID: 2204
public class CircusPlatformingLevelBallRunner : PlatformingLevelPathMovementEnemy
{
	// Token: 0x0600334C RID: 13132 RVA: 0x001DDC60 File Offset: 0x001DC060
	protected override void Die()
	{
		AudioManager.Play("circus_generic_death_fun");
		this.emitAudioFromObject.Add("circus_generic_death_fun");
		this.ball.transform.parent = null;
		this.ball.isMoving = true;
		this.ball.direction = new Vector3((float)this._direction, 0f, 0f);
		base.Die();
	}

	// Token: 0x0600334D RID: 13133 RVA: 0x001DDCCC File Offset: 0x001DC0CC
	private void IdleSFX()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
		{
			AudioManager.Play("circus_ball_runner_idle");
			this.emitAudioFromObject.Add("circus_ball_runner_idle");
		}
	}

	// Token: 0x04003B98 RID: 15256
	private const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04003B99 RID: 15257
	[SerializeField]
	private CircusPlatformingLevelBallRunnerBall ball;

	// Token: 0x04003B9A RID: 15258
	[SerializeField]
	private Transform ballRoot;
}
