using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200080C RID: 2060
public class TrainLevelBlindSpecterEyeProjectile : AbstractProjectile
{
	// Token: 0x06002FB1 RID: 12209 RVA: 0x001C4128 File Offset: 0x001C2528
	public TrainLevelBlindSpecterEyeProjectile Create(Vector2 pos, Vector2 time, float y, bool flipped, float health)
	{
		TrainLevelBlindSpecterEyeProjectile trainLevelBlindSpecterEyeProjectile = base.Create() as TrainLevelBlindSpecterEyeProjectile;
		trainLevelBlindSpecterEyeProjectile.transform.position = pos;
		trainLevelBlindSpecterEyeProjectile.time = time;
		trainLevelBlindSpecterEyeProjectile.end = y;
		trainLevelBlindSpecterEyeProjectile.health = health;
		if (flipped)
		{
			trainLevelBlindSpecterEyeProjectile.sprite.transform.SetScale(new float?(-1f), null, null);
		}
		return trainLevelBlindSpecterEyeProjectile;
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x001C419C File Offset: 0x001C259C
	protected override void Start()
	{
		base.Start();
		this.startPos = base.transform.position.y;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
		TrainLevel trainLevel = Level.Current as TrainLevel;
		if (trainLevel != null)
		{
			this.handCarCollider = trainLevel.handCarCollider;
		}
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x001C4228 File Offset: 0x001C2628
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002FB4 RID: 12212 RVA: 0x001C4251 File Offset: 0x001C2651
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002FB5 RID: 12213 RVA: 0x001C427C File Offset: 0x001C267C
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002FB6 RID: 12214 RVA: 0x001C428F File Offset: 0x001C268F
	protected override void Die()
	{
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06002FB7 RID: 12215 RVA: 0x001C42B0 File Offset: 0x001C26B0
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<TrainLevelPlatform>() != null)
		{
			this.start = hit.transform.position.y + 20f;
			this.t = 1000f;
		}
	}

	// Token: 0x06002FB8 RID: 12216 RVA: 0x001C4308 File Offset: 0x001C2708
	private IEnumerator x_cr()
	{
		float start = base.transform.position.x;
		float t = 0f;
		while (t < this.time.x)
		{
			float val = t / this.time.x;
			float x = Mathf.Lerp(start, -740f, val);
			base.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.End();
		yield break;
	}

	// Token: 0x06002FB9 RID: 12217 RVA: 0x001C4324 File Offset: 0x001C2724
	private IEnumerator y_cr()
	{
		int counter = 0;
		int maxCounter = 2;
		float frameTime = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.start = base.transform.position.y;
		for (;;)
		{
			AudioManager.Play("train_blindspector_eye_bounce");
			this.emitAudioFromObject.Add("train_blindspector_eye_bounce");
			this.t = 0f;
			float newY = this.start;
			if (this.handCarCollider != null)
			{
				Physics2D.IgnoreCollision(this.handCarCollider, this.eyeCollider, true);
			}
			while (this.t < this.time.y)
			{
				float val = this.t / this.time.y;
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.start, this.end, val);
				base.transform.SetPosition(null, new float?(newY), null);
				this.t += CupheadTime.FixedDelta;
				yield return wait;
			}
			base.transform.SetPosition(null, new float?(this.end), null);
			this.start = this.startPos;
			yield return null;
			if (this.handCarCollider != null)
			{
				Physics2D.IgnoreCollision(this.handCarCollider, this.eyeCollider, false);
			}
			this.t = 0f;
			while (this.t < this.time.y)
			{
				float val2 = this.t / this.time.y;
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, this.end, this.start, val2);
				base.transform.SetPosition(null, new float?(newY), null);
				this.t += CupheadTime.FixedDelta;
				yield return wait;
			}
			base.transform.SetPosition(null, new float?(this.start), null);
			this.effectPrefab.Create(base.transform.position);
			while (counter < maxCounter)
			{
				frameTime += CupheadTime.FixedDelta;
				if (frameTime > 0.041666668f)
				{
					counter++;
					frameTime -= 0.041666668f;
					if (counter >= 2)
					{
						base.transform.SetScale(null, new float?(0.3f), null);
						break;
					}
					base.transform.SetScale(null, new float?(0.5f), null);
				}
				yield return wait;
			}
			counter = 0;
			base.transform.SetScale(null, new float?(1f), null);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002FBA RID: 12218 RVA: 0x001C433F File Offset: 0x001C273F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x0400388C RID: 14476
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x0400388D RID: 14477
	[SerializeField]
	private Effect effectPrefab;

	// Token: 0x0400388E RID: 14478
	[SerializeField]
	private Transform sprite;

	// Token: 0x0400388F RID: 14479
	[SerializeField]
	private Collider2D eyeCollider;

	// Token: 0x04003890 RID: 14480
	private DamageReceiver damageReceiver;

	// Token: 0x04003891 RID: 14481
	private float health;

	// Token: 0x04003892 RID: 14482
	private float startPos;

	// Token: 0x04003893 RID: 14483
	private float t;

	// Token: 0x04003894 RID: 14484
	private float start;

	// Token: 0x04003895 RID: 14485
	private float end;

	// Token: 0x04003896 RID: 14486
	private Vector2 time;

	// Token: 0x04003897 RID: 14487
	private Collider2D handCarCollider;
}
