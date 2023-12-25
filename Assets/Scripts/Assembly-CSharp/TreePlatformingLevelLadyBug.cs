using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200088E RID: 2190
public class TreePlatformingLevelLadyBug : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x060032EE RID: 13038 RVA: 0x001D9874 File Offset: 0x001D7C74
	protected override void Awake()
	{
		base.Awake();
		this.manuallySetJumpX = true;
	}

	// Token: 0x060032EF RID: 13039 RVA: 0x001D9883 File Offset: 0x001D7C83
	protected override void Start()
	{
		this.Setup();
		base.Start();
	}

	// Token: 0x060032F0 RID: 13040 RVA: 0x001D9894 File Offset: 0x001D7C94
	public TreePlatformingLevelLadyBug Spawn(Vector3 pos, PlatformingLevelGroundMovementEnemy.Direction dir, bool destroy, TreePlatformingLevelLadyBug.Type type)
	{
		TreePlatformingLevelLadyBug treePlatformingLevelLadyBug = base.Spawn(pos, dir, destroy) as TreePlatformingLevelLadyBug;
		treePlatformingLevelLadyBug.type = type;
		return treePlatformingLevelLadyBug;
	}

	// Token: 0x060032F1 RID: 13041 RVA: 0x001D98BC File Offset: 0x001D7CBC
	public void Setup()
	{
		switch (this.type)
		{
		case TreePlatformingLevelLadyBug.Type.GroundFast:
			base.GoToGround(true, "Fast_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_fast_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_fast_loop");
			this.SetMoveSpeed(base.Properties.fastMovement);
			this.noTurn = true;
			base.StartCoroutine(this.no_y_cr());
			break;
		case TreePlatformingLevelLadyBug.Type.GroundSlow:
			base.GoToGround(true, "Slow_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			this.noTurn = true;
			base.StartCoroutine(this.no_y_cr());
			break;
		case TreePlatformingLevelLadyBug.Type.BounceFast:
			base.animator.Play("Fast_Bounce");
			AudioManager.PlayLoop("level_platform_ladybug_bounce_fast_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce_fast_loop");
			this.SetMoveSpeed(base.Properties.fastMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		case TreePlatformingLevelLadyBug.Type.BounceSlow:
			base.animator.Play("Slow_Bounce");
			AudioManager.PlayLoop("level_platform_ladybug_bounce_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		case TreePlatformingLevelLadyBug.Type.BouncePink:
			this._canParry = true;
			base.animator.Play("Pink_Slow_Ground");
			AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop");
			this.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop");
			this.SetMoveSpeed(base.Properties.slowMovement);
			base.StartCoroutine(this.y_cr());
			this.noTurn = true;
			break;
		}
	}

	// Token: 0x060032F2 RID: 13042 RVA: 0x001D9A98 File Offset: 0x001D7E98
	private IEnumerator y_cr()
	{
		this.floating = false;
		yield return null;
		for (;;)
		{
			while (!base.Grounded)
			{
				yield return null;
			}
			base.Jump();
			AudioManager.Play("level_platform_ladybug_bounce");
			this.emitAudioFromObject.Add("level_platform_ladybug_bounce");
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032F3 RID: 13043 RVA: 0x001D9AB4 File Offset: 0x001D7EB4
	private IEnumerator no_y_cr()
	{
		for (;;)
		{
			if (!base.Grounded)
			{
				this.fallInPit = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032F4 RID: 13044 RVA: 0x001D9AD0 File Offset: 0x001D7ED0
	protected override void Die()
	{
		base.Die();
		AudioManager.Play("level_platform_ladybug_death");
		this.emitAudioFromObject.Add("level_platform_ladybug_death");
		switch (this.type)
		{
		case TreePlatformingLevelLadyBug.Type.GroundFast:
			AudioManager.Stop("level_platform_ladybug_ground_fast_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.GroundSlow:
			AudioManager.Stop("level_platform_ladybug_ground_slow_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BounceFast:
			AudioManager.Stop("level_platform_ladybug_bounce_fast_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BounceSlow:
			AudioManager.Stop("level_platform_ladybug_bounce_slow_loop");
			break;
		case TreePlatformingLevelLadyBug.Type.BouncePink:
			AudioManager.Stop("level_platform_ladybug_ground_slow_loop");
			break;
		}
	}

	// Token: 0x04003B13 RID: 15123
	public TreePlatformingLevelLadyBug.Type type;

	// Token: 0x0200088F RID: 2191
	public enum Type
	{
		// Token: 0x04003B15 RID: 15125
		GroundFast,
		// Token: 0x04003B16 RID: 15126
		GroundSlow,
		// Token: 0x04003B17 RID: 15127
		BounceFast,
		// Token: 0x04003B18 RID: 15128
		BounceSlow,
		// Token: 0x04003B19 RID: 15129
		BouncePink
	}
}
