using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006C0 RID: 1728
public class FrogsLevelTallFirefly : AbstractProjectile
{
	// Token: 0x170003B5 RID: 949
	// (get) Token: 0x060024B8 RID: 9400 RVA: 0x00158578 File Offset: 0x00156978
	protected override float DestroyLifetime
	{
		get
		{
			return 10000000f;
		}
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x00158580 File Offset: 0x00156980
	public FrogsLevelTallFirefly Create(Vector2 pos, Vector2 target, float speed, int hp, float followDelay, float followTime, float followDistance, float invincibleDuration, AbstractPlayerController player, int layer)
	{
		FrogsLevelTallFirefly frogsLevelTallFirefly = this.Create(pos) as FrogsLevelTallFirefly;
		frogsLevelTallFirefly.Health = hp;
		frogsLevelTallFirefly.Speed = speed;
		frogsLevelTallFirefly.DamagesType.OnlyPlayer();
		frogsLevelTallFirefly.CollisionDeath.OnlyPlayer();
		frogsLevelTallFirefly.CollisionDeath.PlayerProjectiles = true;
		frogsLevelTallFirefly.Init(pos, target, followDelay, followTime, followDistance, player, layer, invincibleDuration);
		frogsLevelTallFirefly.DestroyDistance = 10000000f;
		return frogsLevelTallFirefly;
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x001585EC File Offset: 0x001569EC
	protected override void Awake()
	{
		base.Awake();
		if (Level.Current == null || !Level.Current.Started)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x00158620 File Offset: 0x00156A20
	private void Init(Vector2 pos, Vector2 target, float delay, float followTime, float followDistance, AbstractPlayerController player, int layer, float invincibleDuration)
	{
		base.transform.position = pos;
		this.target = target;
		this.followDelay = delay;
		this.followTime = followTime;
		this.followDistance = followDistance;
		this.currentHp = (float)this.Health;
		this.player = player;
		this.sprite.sortingOrder = layer;
		this.invincibleDuration = invincibleDuration;
		base.GetComponent<CircleCollider2D>().enabled = false;
		base.StartCoroutine(this.firefly_cr());
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x001586A0 File Offset: 0x00156AA0
	protected override void Start()
	{
		base.Start();
		this.damageDealer.SetDamageFlags(true, false, false);
		this.damageDealer.SetDamage(1f);
		this.damageDealer.SetDamageSource(DamageDealer.DamageSource.Enemy);
		this.damageDealer.SetRate(0.3f);
		DamageReceiver component = base.GetComponent<DamageReceiver>();
		component.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x00158706 File Offset: 0x00156B06
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.currentHp -= info.damage;
		this.Die();
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x00158721 File Offset: 0x00156B21
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(this.target, 20f);
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x00158740 File Offset: 0x00156B40
	protected override void Die()
	{
		if (this.currentHp > 0f)
		{
			return;
		}
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.Play("level_frogs_tall_firefly_death");
		this.emitAudioFromObject.Add("level_frogs_tall_firefly_death");
		base.Die();
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x001587B1 File Offset: 0x00156BB1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.currentHp = 0f;
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x001587D8 File Offset: 0x00156BD8
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.currentHp = 0f;
		this.Die();
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x001587EB File Offset: 0x00156BEB
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		this.currentHp = 0f;
		this.Die();
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x00158800 File Offset: 0x00156C00
	private void SetMovementPose()
	{
		Vector2 vector = this.target - this.aim.transform.position;
		if (vector.x > 0f)
		{
			base.transform.SetScale(new float?(-1f), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(1f), null, null);
		}
		if (Mathf.Abs(vector.x) >= Mathf.Abs(vector.y))
		{
			if (vector.y < 0f)
			{
				base.animator.SetTrigger("OnMoveDown");
			}
			else
			{
				base.animator.SetTrigger("OnMoveForward");
			}
		}
		else
		{
			base.animator.SetTrigger("OnMoveForward");
		}
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x001588FC File Offset: 0x00156CFC
	private IEnumerator firefly_cr()
	{
		yield return base.StartCoroutine(this.initialMove_cr());
		for (;;)
		{
			yield return base.StartCoroutine(this.follow_cr());
		}
		yield break;
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x00158918 File Offset: 0x00156D18
	private IEnumerator initialMove_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.transform.SetScale(new float?(1f), null, null);
		float falloffDistance = 200f;
		int t = 0;
		int falloffFrames = (int)(falloffDistance * 2f / (this.Speed / 60f));
		Vector3 direction = this.target - this.aim.transform.position;
		direction.Normalize();
		float speed = this.Speed;
		this.SetMovementPose();
		while (Vector2.Distance(base.transform.position, this.target) > falloffDistance)
		{
			base.transform.position += direction * speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		while (t < falloffFrames)
		{
			if (PauseManager.state != PauseManager.State.Paused)
			{
				float value = (float)t / (float)falloffFrames;
				speed = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.Speed, 0f, value);
				base.transform.position += direction * speed * CupheadTime.FixedDelta;
				t++;
			}
			yield return wait;
		}
		base.animator.SetTrigger("OnIdle");
		yield break;
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x00158934 File Offset: 0x00156D34
	private IEnumerator follow_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		base.animator.SetTrigger("OnIdle");
		yield return CupheadTime.WaitForSeconds(this, this.followDelay);
		Vector2 start = base.transform.position;
		this.aim.LookAt2D(this.player.center);
		this.target = base.transform.position + this.aim.right * this.followDistance;
		this.SetMovementPose();
		float t = 0f;
		while (t < this.followTime)
		{
			float val = t / this.followTime;
			float x = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.x, this.target.x, val);
			float y = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start.y, this.target.y, val);
			base.transform.SetPosition(new float?(x), new float?(y), null);
			t += CupheadTime.FixedDelta;
			yield return wait;
		}
		base.transform.SetPosition(new float?(this.target.x), new float?(this.target.y), null);
		base.animator.SetTrigger("OnIdle");
		yield break;
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x00158950 File Offset: 0x00156D50
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.invincibleDuration > 0f)
		{
			this.invincibleDuration -= CupheadTime.FixedDelta;
			if (this.invincibleDuration <= 0f)
			{
				base.GetComponent<CircleCollider2D>().enabled = true;
			}
		}
	}

	// Token: 0x04002D62 RID: 11618
	[SerializeField]
	private Transform aim;

	// Token: 0x04002D63 RID: 11619
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04002D64 RID: 11620
	private int Health;

	// Token: 0x04002D65 RID: 11621
	private float Speed;

	// Token: 0x04002D66 RID: 11622
	private float followDelay;

	// Token: 0x04002D67 RID: 11623
	private float followTime;

	// Token: 0x04002D68 RID: 11624
	private float followDistance;

	// Token: 0x04002D69 RID: 11625
	private Vector2 target;

	// Token: 0x04002D6A RID: 11626
	private float currentHp;

	// Token: 0x04002D6B RID: 11627
	private AbstractPlayerController player;

	// Token: 0x04002D6C RID: 11628
	private float invincibleDuration;
}
