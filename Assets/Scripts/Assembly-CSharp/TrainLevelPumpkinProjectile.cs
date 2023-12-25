using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000827 RID: 2087
public class TrainLevelPumpkinProjectile : AbstractProjectile
{
	// Token: 0x06003078 RID: 12408 RVA: 0x001C8DC2 File Offset: 0x001C71C2
	protected override void Start()
	{
		base.Start();
		this.SetParryable(true);
		base.StartCoroutine(this.float_cr());
	}

	// Token: 0x06003079 RID: 12409 RVA: 0x001C8DE0 File Offset: 0x001C71E0
	protected override void Update()
	{
		base.Update();
		if (!this.hasDied && base.transform.position.y < -325f)
		{
			this.Die();
		}
	}

	// Token: 0x0600307A RID: 12410 RVA: 0x001C8E24 File Offset: 0x001C7224
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			if (hit.tag == "ParrySwitch")
			{
				ParrySwitch component = hit.GetComponent<ParrySwitch>();
				if (component.name != "Right" && component.name != "Left")
				{
					return;
				}
				component.ActivateFromOtherSource();
				this.Die();
			}
			else if (hit.name == "HandCar")
			{
				this.Die();
			}
		}
	}

	// Token: 0x0600307B RID: 12411 RVA: 0x001C8EB2 File Offset: 0x001C72B2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600307C RID: 12412 RVA: 0x001C8EDB File Offset: 0x001C72DB
	public void Drop()
	{
		base.transform.SetParent(null);
		this.StopAllCoroutines();
		base.animator.Play("Fall");
		base.StartCoroutine(this.drop_cr());
	}

	// Token: 0x0600307D RID: 12413 RVA: 0x001C8F0C File Offset: 0x001C730C
	protected override void Die()
	{
		if (this.hasDied)
		{
			return;
		}
		this.hasDied = true;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x0600307E RID: 12414 RVA: 0x001C8F30 File Offset: 0x001C7330
	private IEnumerator float_cr()
	{
		float top = base.transform.localPosition.y;
		float bottom = top - 20f;
		float time = 0.4f;
		for (;;)
		{
			yield return base.TweenLocalPositionY(top, bottom, time, EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionY(bottom, top, time, EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x0600307F RID: 12415 RVA: 0x001C8F4C File Offset: 0x001C734C
	private IEnumerator drop_cr()
	{
		float top = base.transform.position.y;
		yield return base.TweenPositionY(top, -340f, this.fallTime, EaseUtils.EaseType.easeInSine);
		this.Die();
		yield break;
	}

	// Token: 0x04003925 RID: 14629
	private const float DEATH_Y = -325f;

	// Token: 0x04003926 RID: 14630
	public float fallTime;

	// Token: 0x04003927 RID: 14631
	private bool hasDied;
}
