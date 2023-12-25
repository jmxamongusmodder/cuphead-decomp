using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008BC RID: 2236
public class FunhousePlatformingLevelRocket : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x0600342A RID: 13354 RVA: 0x001E457A File Offset: 0x001E297A
	protected override void Update()
	{
		base.Update();
	}

	// Token: 0x0600342B RID: 13355 RVA: 0x001E4584 File Offset: 0x001E2984
	protected override void Start()
	{
		base.Start();
		this.collisionChild = base.GetComponentInChildren<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.collisionDamageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600342C RID: 13356 RVA: 0x001E45D4 File Offset: 0x001E29D4
	public void Init(Vector2 pos, bool gravityReversed, bool onRight)
	{
		base.transform.position = pos;
		AudioManager.PlayLoop("funhouse_rocket_idle_loop");
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE++;
		this.emitAudioFromObject.Add("funhouse_rocket_idle_loop");
		this.gravityReversed = gravityReversed;
		if (gravityReversed)
		{
			base.transform.SetScale(null, new float?(-1f), null);
		}
		this._direction = ((!onRight) ? PlatformingLevelGroundMovementEnemy.Direction.Right : PlatformingLevelGroundMovementEnemy.Direction.Left);
		base.StartCoroutine(this.launch_cr());
	}

	// Token: 0x0600342D RID: 13357 RVA: 0x001E466C File Offset: 0x001E2A6C
	private IEnumerator launch_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		AbstractPlayerController player = PlayerManager.GetNext();
		float dist = player.transform.position.x - base.transform.position.x;
		while (Mathf.Abs(dist) > this.distToLaunch)
		{
			player = PlayerManager.GetNext();
			dist = player.transform.position.x - base.transform.position.x;
			yield return null;
		}
		FunhousePlatformingLevelRocket.ROCKETS_ALIVE--;
		if (FunhousePlatformingLevelRocket.ROCKETS_ALIVE == 0)
		{
			AudioManager.Stop("funhouse_rocket_idle_loop");
		}
		this.landing = true;
		this.launched = true;
		base.animator.SetTrigger("OnShoot");
		while (this.launched)
		{
			if (this.gravityReversed)
			{
				base.transform.position += Vector3.down * this.launchSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				base.transform.position += Vector3.up * this.launchSpeed * CupheadTime.FixedDelta;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600342E RID: 13358 RVA: 0x001E4687 File Offset: 0x001E2A87
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (this.launched && phase == CollisionPhase.Enter)
		{
			this.Die();
		}
	}

	// Token: 0x0600342F RID: 13359 RVA: 0x001E46A8 File Offset: 0x001E2AA8
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (this.launched && phase == CollisionPhase.Enter)
		{
			this.Die();
		}
	}

	// Token: 0x06003430 RID: 13360 RVA: 0x001E46CC File Offset: 0x001E2ACC
	protected override void Die()
	{
		AudioManager.Stop("funhouse_rocket_trans_to_spin");
		AudioManager.Play("funhouse_rocket_explode");
		this.emitAudioFromObject.Add("funhouse_rocket_explode");
		this.explosion.Create(this.sprite.transform.position);
		base.Die();
	}

	// Token: 0x06003431 RID: 13361 RVA: 0x001E471F File Offset: 0x001E2B1F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (!this.launched)
		{
			FunhousePlatformingLevelRocket.ROCKETS_ALIVE--;
		}
		if (FunhousePlatformingLevelRocket.ROCKETS_ALIVE == 0)
		{
			AudioManager.Stop("funhouse_rocket_idle_loop");
		}
	}

	// Token: 0x06003432 RID: 13362 RVA: 0x001E4752 File Offset: 0x001E2B52
	private void SoundRocketTransToSpin()
	{
		AudioManager.Play("funhouse_rocket_trans_to_spin");
		this.emitAudioFromObject.Add("funhouse_rocket_trans_to_spin");
		AudioManager.Play("funhouse_rocket_explode");
		this.emitAudioFromObject.Add("funhouse_rocket_explode");
	}

	// Token: 0x04003C67 RID: 15463
	private static int ROCKETS_ALIVE;

	// Token: 0x04003C68 RID: 15464
	[SerializeField]
	private Transform sprite;

	// Token: 0x04003C69 RID: 15465
	[SerializeField]
	private FunhousePlatformingLevelExplosionFX explosion;

	// Token: 0x04003C6A RID: 15466
	[SerializeField]
	private float distToLaunch;

	// Token: 0x04003C6B RID: 15467
	[SerializeField]
	private float launchSpeed;

	// Token: 0x04003C6C RID: 15468
	private bool launched;

	// Token: 0x04003C6D RID: 15469
	private CollisionChild collisionChild;

	// Token: 0x04003C6E RID: 15470
	[SerializeField]
	private DamageReceiver collisionDamageReceiver;
}
