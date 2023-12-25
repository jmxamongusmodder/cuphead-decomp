using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200081F RID: 2079
public class TrainLevelLollipopGhoulLightning : AbstractCollidableObject
{
	// Token: 0x06003042 RID: 12354 RVA: 0x001C7704 File Offset: 0x001C5B04
	private void Start()
	{
		base.StartCoroutine(this.start_cr());
		this.damageDealer = DamageDealer.NewEnemy(0.2f);
	}

	// Token: 0x06003043 RID: 12355 RVA: 0x001C7723 File Offset: 0x001C5B23
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003044 RID: 12356 RVA: 0x001C773B File Offset: 0x001C5B3B
	public void End()
	{
		base.StartCoroutine(this.end_cr());
	}

	// Token: 0x06003045 RID: 12357 RVA: 0x001C774A File Offset: 0x001C5B4A
	private void GoAway()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003046 RID: 12358 RVA: 0x001C775D File Offset: 0x001C5B5D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer == null)
		{
			return;
		}
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003047 RID: 12359 RVA: 0x001C7780 File Offset: 0x001C5B80
	private IEnumerator start_cr()
	{
		base.animator.SetTrigger("OnStart");
		yield return base.animator.WaitForAnimationToStart(this, "Loop", false);
		base.animator.SetBool("isFX", true);
		base.animator.SetTrigger("OnDustStart");
		yield break;
	}

	// Token: 0x06003048 RID: 12360 RVA: 0x001C779C File Offset: 0x001C5B9C
	private IEnumerator end_cr()
	{
		base.animator.SetTrigger("OnEnd");
		base.animator.SetBool("isFX", false);
		yield return base.animator.WaitForAnimationToStart(this, "Init", false);
		base.animator.SetTrigger("OnDustEnd");
		yield return base.animator.WaitForAnimationToStart(this, "Init", 2, false);
		this.GoAway();
		yield break;
	}

	// Token: 0x04003901 RID: 14593
	[SerializeField]
	private Transform spark1;

	// Token: 0x04003902 RID: 14594
	[SerializeField]
	private Transform spark2;

	// Token: 0x04003903 RID: 14595
	private DamageDealer damageDealer;
}
