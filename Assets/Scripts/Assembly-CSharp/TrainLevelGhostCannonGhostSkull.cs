using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200081A RID: 2074
public class TrainLevelGhostCannonGhostSkull : AbstractProjectile
{
	// Token: 0x0600301C RID: 12316 RVA: 0x001C6930 File Offset: 0x001C4D30
	public TrainLevelGhostCannonGhostSkull Create(Vector3 pos, float speed)
	{
		TrainLevelGhostCannonGhostSkull trainLevelGhostCannonGhostSkull = UnityEngine.Object.Instantiate<TrainLevelGhostCannonGhostSkull>(this);
		trainLevelGhostCannonGhostSkull.transform.position = pos;
		trainLevelGhostCannonGhostSkull.maxSpeed = speed;
		return trainLevelGhostCannonGhostSkull;
	}

	// Token: 0x1700041F RID: 1055
	// (get) Token: 0x0600301D RID: 12317 RVA: 0x001C6958 File Offset: 0x001C4D58
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x0600301E RID: 12318 RVA: 0x001C695F File Offset: 0x001C4D5F
	protected override void Start()
	{
		base.Start();
		this.SetParryable(true);
		base.StartCoroutine(this.speed_cr());
	}

	// Token: 0x0600301F RID: 12319 RVA: 0x001C697C File Offset: 0x001C4D7C
	protected override void Update()
	{
		base.Update();
		if (!base.dead && base.transform.position.y < -325f)
		{
			this.Die();
		}
		base.transform.AddPosition(0f, -this.speed * CupheadTime.Delta, 0f);
	}

	// Token: 0x06003020 RID: 12320 RVA: 0x001C69E4 File Offset: 0x001C4DE4
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

	// Token: 0x06003021 RID: 12321 RVA: 0x001C6A72 File Offset: 0x001C4E72
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06003022 RID: 12322 RVA: 0x001C6A9C File Offset: 0x001C4E9C
	private IEnumerator speed_cr()
	{
		yield return base.TweenPositionY(base.transform.position.y, base.transform.position.y + 100f, 0.4f, EaseUtils.EaseType.easeOutCubic);
		yield return base.StartCoroutine(this.tweenSpeed_cr(0f, this.maxSpeed, 0.4f, EaseUtils.EaseType.linear));
		yield break;
	}

	// Token: 0x06003023 RID: 12323 RVA: 0x001C6AB8 File Offset: 0x001C4EB8
	private IEnumerator tweenSpeed_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.speed = EaseUtils.Ease(ease, start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.speed = this.maxSpeed;
		yield break;
	}

	// Token: 0x040038EB RID: 14571
	private const float DEATH_Y = -325f;

	// Token: 0x040038EC RID: 14572
	private float maxSpeed;

	// Token: 0x040038ED RID: 14573
	private float speed;
}
