using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000524 RID: 1316
public class BeeLevelSecurityGuardBomb : AbstractProjectile
{
	// Token: 0x060017A9 RID: 6057 RVA: 0x000D52A4 File Offset: 0x000D36A4
	public BeeLevelSecurityGuardBomb Create(Vector2 pos, int direction, float idleTime, float warningTime, float childSpeed, int childCount)
	{
		BeeLevelSecurityGuardBomb beeLevelSecurityGuardBomb = base.Create() as BeeLevelSecurityGuardBomb;
		beeLevelSecurityGuardBomb.direction = direction;
		beeLevelSecurityGuardBomb.idleTime = idleTime;
		beeLevelSecurityGuardBomb.warningTime = warningTime;
		beeLevelSecurityGuardBomb.childSpeed = childSpeed;
		beeLevelSecurityGuardBomb.childCount = childCount;
		beeLevelSecurityGuardBomb.transform.position = pos;
		return beeLevelSecurityGuardBomb;
	}

	// Token: 0x060017AA RID: 6058 RVA: 0x000D52F5 File Offset: 0x000D36F5
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x060017AB RID: 6059 RVA: 0x000D530A File Offset: 0x000D370A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060017AC RID: 6060 RVA: 0x000D5334 File Offset: 0x000D3734
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		float num = (float)(360 / this.childCount);
		for (int i = 0; i < this.childCount; i++)
		{
			BasicProjectile basicProjectile = this.childPrefab.Create(base.transform.position, num * (float)i, Vector2.one, this.childSpeed);
			basicProjectile.SetParryable(i % 2 != 0);
		}
	}

	// Token: 0x060017AD RID: 6061 RVA: 0x000D53AC File Offset: 0x000D37AC
	private IEnumerator go_cr()
	{
		float time = 0.3f;
		Vector2 pos = base.transform.position + new Vector2((float)(50 * this.direction), 100f);
		yield return base.TweenPosition(base.transform.position, pos, time, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, this.idleTime);
		AudioManager.PlayLoop("bee_guard_bomb_warning");
		this.emitAudioFromObject.Add("bee_guard_bomb_warning");
		base.animator.Play("Warning");
		yield return CupheadTime.WaitForSeconds(this, this.warningTime);
		AudioManager.Stop("bee_guard_bomb_warning");
		AudioManager.Play("bee_guard_bomb_explode");
		this.emitAudioFromObject.Add("bee_guard_bomb_explode");
		this.Die();
		yield break;
	}

	// Token: 0x060017AE RID: 6062 RVA: 0x000D53C7 File Offset: 0x000D37C7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.childPrefab = null;
	}

	// Token: 0x040020D7 RID: 8407
	[SerializeField]
	private BasicProjectile childPrefab;

	// Token: 0x040020D8 RID: 8408
	private int direction;

	// Token: 0x040020D9 RID: 8409
	private float idleTime;

	// Token: 0x040020DA RID: 8410
	private float warningTime;

	// Token: 0x040020DB RID: 8411
	private float childSpeed;

	// Token: 0x040020DC RID: 8412
	private int childCount;
}
