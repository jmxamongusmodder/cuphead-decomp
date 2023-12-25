using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200058A RID: 1418
public class DevilLevelBombExplosion : Effect
{
	// Token: 0x06001B16 RID: 6934 RVA: 0x000F9057 File Offset: 0x000F7457
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001B17 RID: 6935 RVA: 0x000F906A File Offset: 0x000F746A
	private void Start()
	{
		base.StartCoroutine(this.timer_cr());
		AudioManager.Play("bat_bomb_explo");
	}

	// Token: 0x06001B18 RID: 6936 RVA: 0x000F9083 File Offset: 0x000F7483
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x000F909B File Offset: 0x000F749B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x000F90C4 File Offset: 0x000F74C4
	private IEnumerator timer_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.loopTime);
		base.animator.SetTrigger("Continue");
		yield break;
	}

	// Token: 0x04002455 RID: 9301
	[SerializeField]
	private float loopTime;

	// Token: 0x04002456 RID: 9302
	private DamageDealer damageDealer;
}
