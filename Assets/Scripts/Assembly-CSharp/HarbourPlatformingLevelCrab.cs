using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C6 RID: 2246
public class HarbourPlatformingLevelCrab : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x0600347D RID: 13437 RVA: 0x001E7AE4 File Offset: 0x001E5EE4
	protected override void Awake()
	{
		base.Awake();
		base.animator.SetBool("goingLeft", base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right);
		base.GetComponent<DamageReceiver>().enabled = false;
		this.walkingBack = false;
		this.SetTurnTarget(this.target);
	}

	// Token: 0x0600347E RID: 13438 RVA: 0x001E7B39 File Offset: 0x001E5F39
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.play_loop_SFX());
	}

	// Token: 0x0600347F RID: 13439 RVA: 0x001E7B4E File Offset: 0x001E5F4E
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<HarbourPlatformingLevelCrab>())
		{
			base.StartCoroutine(this.prepare_turn_cr(hit));
		}
	}

	// Token: 0x06003480 RID: 13440 RVA: 0x001E7B7C File Offset: 0x001E5F7C
	protected override void CalculateDirection()
	{
	}

	// Token: 0x06003481 RID: 13441 RVA: 0x001E7B80 File Offset: 0x001E5F80
	private IEnumerator prepare_turn_cr(GameObject hit)
	{
		float dist = Vector3.Distance(hit.transform.position, base.transform.position);
		while (dist > 670f)
		{
			dist = Vector3.Distance(hit.transform.position, base.transform.position);
			yield return null;
		}
		this.Turn();
		yield return null;
		yield break;
	}

	// Token: 0x06003482 RID: 13442 RVA: 0x001E7BA4 File Offset: 0x001E5FA4
	protected override Coroutine Turn()
	{
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 1000f)))
		{
			AudioManager.Play("harbour_crab_turn");
			this.emitAudioFromObject.Add("harbour_crab_turn");
		}
		this.walkingBack = !this.walkingBack;
		base.animator.SetBool("walkingBack", this.walkingBack);
		base.animator.SetBool("goingLeft", base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right);
		this.target = ((base.direction != PlatformingLevelGroundMovementEnemy.Direction.Right) ? "Turn_Right" : "Turn_Left");
		this.SetTurnTarget(this.target);
		if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING))
		{
			CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		}
		return base.Turn();
	}

	// Token: 0x06003483 RID: 13443 RVA: 0x001E7CB0 File Offset: 0x001E60B0
	private IEnumerator play_loop_SFX()
	{
		bool playerLeft = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(1000f, 1000f)))
			{
				playerLeft = false;
				if (!AudioManager.CheckIfPlaying("harbour_crab_walk") && !AudioManager.CheckIfPlaying("harbour_crab_turn"))
				{
					AudioManager.PlayLoop("harbour_crab_walk");
					this.emitAudioFromObject.Add("harbour_crab_walk");
				}
			}
			else if (!playerLeft)
			{
				AudioManager.Stop("harbour_crab_walk");
				playerLeft = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003CA5 RID: 15525
	private const float ON_SCREEN_SOUND_PADDING = 1000f;

	// Token: 0x04003CA6 RID: 15526
	private string target;

	// Token: 0x04003CA7 RID: 15527
	private bool walkingBack;
}
