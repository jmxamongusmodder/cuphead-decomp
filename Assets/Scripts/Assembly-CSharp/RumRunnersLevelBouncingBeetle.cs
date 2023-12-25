using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000787 RID: 1927
public class RumRunnersLevelBouncingBeetle : AbstractProjectile
{
	// Token: 0x170003EA RID: 1002
	// (get) Token: 0x06002A7B RID: 10875 RVA: 0x0018D1DA File Offset: 0x0018B5DA
	// (set) Token: 0x06002A7C RID: 10876 RVA: 0x0018D1E2 File Offset: 0x0018B5E2
	public bool leaveScreen { get; set; }

	// Token: 0x170003EB RID: 1003
	// (get) Token: 0x06002A7D RID: 10877 RVA: 0x0018D1EB File Offset: 0x0018B5EB
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002A7E RID: 10878 RVA: 0x0018D1F2 File Offset: 0x0018B5F2
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.initialScale = this.visualTransform.localScale;
	}

	// Token: 0x06002A7F RID: 10879 RVA: 0x0018D22E File Offset: 0x0018B62E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002A80 RID: 10880 RVA: 0x0018D24C File Offset: 0x0018B64C
	public virtual RumRunnersLevelBouncingBeetle Init(Vector2 pos, Vector3 velocity, float initialSpeed, float timeToSlowdown, float targetSpeed, float hp)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.SetParryable(false);
		base.transform.position = pos;
		this.velocity = velocity;
		this.currentSpeed = initialSpeed;
		this.initialSpeed = initialSpeed;
		this.targetSpeed = targetSpeed;
		this.currentSpeed = targetSpeed;
		this.slowdownDuration = timeToSlowdown;
		this.isMoving = true;
		this.hp = hp;
		this.offset = base.GetComponent<Collider2D>().bounds.size.x / 2f;
		this.Move();
		this.leaveScreen = false;
		RumRunnersLevelBouncingBeetle.LastSortingIndex--;
		if (RumRunnersLevelBouncingBeetle.LastSortingIndex < 10)
		{
			RumRunnersLevelBouncingBeetle.LastSortingIndex = 15;
		}
		this.visualTransform.GetComponent<SpriteRenderer>().sortingOrder = RumRunnersLevelBouncingBeetle.LastSortingIndex;
		return this;
	}

	// Token: 0x06002A81 RID: 10881 RVA: 0x0018D324 File Offset: 0x0018B724
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			Level.Current.RegisterMinionKilled();
			this.Die();
		}
	}

	// Token: 0x06002A82 RID: 10882 RVA: 0x0018D35C File Offset: 0x0018B75C
	protected override void Die()
	{
		this.SFX_RUMRUN_CaterpillarBall_DeathExplosion();
		this.explosionPrefab.Create(base.transform.position);
		for (int i = 0; i < UnityEngine.Random.Range(3, 5); i++)
		{
			float f = UnityEngine.Random.Range(0f, 360f);
			Vector3 b = new Vector3(Mathf.Cos(f) * 50f, Mathf.Sin(f) * 50f);
			SpriteDeathParts spriteDeathParts = this.shrapnelPrefab.CreatePart(base.transform.position + b);
			spriteDeathParts.animator.Update(0f);
			spriteDeathParts.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
		}
		for (int j = 0; j < UnityEngine.Random.Range(3, 5); j++)
		{
			float f2 = UnityEngine.Random.Range(0f, 360f);
			Vector3 b2 = new Vector3(Mathf.Cos(f2) * 50f, Mathf.Sin(f2) * 50f);
			SpriteDeathParts spriteDeathParts2 = this.shrapnelPrefab.CreatePart(base.transform.position + b2);
			spriteDeathParts2.animator.Update(0f);
			spriteDeathParts2.animator.Play(0, 0, UnityEngine.Random.Range(0f, 1f));
			spriteDeathParts2.transform.SetScale(new float?(0.75f), new float?(0.75f), null);
			SpriteRenderer component = spriteDeathParts2.GetComponent<SpriteRenderer>();
			component.sortingLayerName = "Background";
			component.sortingOrder = 95;
			component.color = new Color(0.7f, 0.7f, 0.7f, 1f);
		}
		base.Die();
		this.Recycle<RumRunnersLevelBouncingBeetle>();
	}

	// Token: 0x06002A83 RID: 10883 RVA: 0x0018D525 File Offset: 0x0018B925
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002A84 RID: 10884 RVA: 0x0018D534 File Offset: 0x0018B934
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float elapsedTime = 0f;
		for (;;)
		{
			yield return wait;
			if (this.isMoving)
			{
				if (elapsedTime <= this.slowdownDuration)
				{
					elapsedTime += CupheadTime.FixedDelta;
					this.currentSpeed = Mathf.Lerp(this.initialSpeed, this.targetSpeed, elapsedTime / this.slowdownDuration);
				}
				base.transform.position += this.velocity * this.currentSpeed * CupheadTime.FixedDelta;
				this.CheckBounds();
			}
		}
		yield break;
	}

	// Token: 0x06002A85 RID: 10885 RVA: 0x0018D550 File Offset: 0x0018B950
	private void CheckBounds()
	{
		bool flag = false;
		Vector3 vector = Vector3.zero;
		Vector3 one = Vector3.one;
		float z = 0f;
		if (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - this.offset && this.velocity.y > 0f)
		{
			flag = true;
			this.velocity.y = -Mathf.Abs(this.velocity.y);
			vector = Vector2.up;
			one.x = ((this.velocity.x <= 0f) ? -1f : 1f);
			z = 180f;
		}
		if (base.transform.position.y < (float)Level.Current.Ground + this.offset && this.velocity.y < 0f)
		{
			flag = true;
			this.velocity.y = Mathf.Abs(this.velocity.y);
			vector = Vector2.down;
			one.x = ((this.velocity.x >= 0f) ? -1f : 1f);
			one.y = 1f;
			z = 0f;
		}
		if (!this.leaveScreen)
		{
			if (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax - this.offset && this.velocity.x > 0f)
			{
				flag = true;
				this.velocity.x = -Mathf.Abs(this.velocity.x);
				vector = Vector2.right;
				z = 90f;
				one.x = ((this.velocity.y >= 0f) ? -1f : 1f);
			}
			if (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin + this.offset && this.velocity.x < 0f)
			{
				flag = true;
				this.velocity.x = Mathf.Abs(this.velocity.x);
				vector = Vector2.left;
				one.x = ((this.velocity.y <= 0f) ? -1f : 1f);
				z = 270f;
			}
		}
		else if (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - 100f || base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + 100f)
		{
			base.Die();
		}
		if (flag)
		{
			Effect effect = this.wallPoofEffect.Create(base.transform.position + vector * this.offset);
			effect.transform.rotation = Quaternion.Euler(0f, 0f, z);
			Vector3 localScale = effect.transform.localScale;
			localScale.x *= one.x;
			effect.transform.localScale = localScale;
			this.SFX_RUMRUN_CaterpillarBall_Bounce();
			if (this.squashCoroutine != null)
			{
				base.StopCoroutine(this.squashCoroutine);
			}
			this.squashCoroutine = base.StartCoroutine(this.squash_cr(vector));
		}
	}

	// Token: 0x06002A86 RID: 10886 RVA: 0x0018D928 File Offset: 0x0018BD28
	private IEnumerator squash_cr(Vector2 normal)
	{
		Vector3 scale = this.initialScale;
		Vector3 visualOffset;
		if (normal.x != 0f)
		{
			scale.x *= this.squashAmount;
			scale.y *= this.squashAmountPerpendicular;
			visualOffset = new Vector3(this.offset * (1f - this.squashAmount) * Mathf.Sign(normal.x), 0f);
		}
		else
		{
			scale.y *= this.squashAmount;
			scale.x *= this.squashAmountPerpendicular;
			visualOffset = new Vector3(0f, this.offset * (1f - this.squashAmount) * Mathf.Sign(normal.y));
			this.SFX_RUMRUN_CaterpillarBall_Bounce();
		}
		this.visualTransform.localScale = scale;
		this.visualTransform.localPosition = visualOffset;
		for (float elapsedTime = 0f; elapsedTime < 0.041666668f; elapsedTime += CupheadTime.Delta)
		{
			yield return null;
		}
		this.visualTransform.localScale = this.initialScale;
		this.visualTransform.localPosition = Vector3.zero;
		this.squashCoroutine = null;
		yield break;
	}

	// Token: 0x06002A87 RID: 10887 RVA: 0x0018D94A File Offset: 0x0018BD4A
	private void SFX_RUMRUN_CaterpillarBall_Bounce()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_bounce");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_bounce");
	}

	// Token: 0x06002A88 RID: 10888 RVA: 0x0018D966 File Offset: 0x0018BD66
	private void SFX_RUMRUN_CaterpillarBall_DeathExplosion()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_deathexplosion");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_caterpillarball_deathexplosion");
	}

	// Token: 0x04003343 RID: 13123
	private const float DESTROY_RANGE = 100f;

	// Token: 0x04003344 RID: 13124
	private static int LastSortingIndex;

	// Token: 0x04003346 RID: 13126
	[SerializeField]
	private Effect wallPoofEffect;

	// Token: 0x04003347 RID: 13127
	[SerializeField]
	private Transform visualTransform;

	// Token: 0x04003348 RID: 13128
	[SerializeField]
	private float squashAmount;

	// Token: 0x04003349 RID: 13129
	[SerializeField]
	private float squashAmountPerpendicular;

	// Token: 0x0400334A RID: 13130
	private bool isMoving;

	// Token: 0x0400334B RID: 13131
	private float initialSpeed;

	// Token: 0x0400334C RID: 13132
	private float targetSpeed;

	// Token: 0x0400334D RID: 13133
	private float currentSpeed;

	// Token: 0x0400334E RID: 13134
	private float slowdownDuration;

	// Token: 0x0400334F RID: 13135
	private float hp;

	// Token: 0x04003350 RID: 13136
	private float offset;

	// Token: 0x04003351 RID: 13137
	private Vector3 velocity;

	// Token: 0x04003352 RID: 13138
	private Vector3 initialScale;

	// Token: 0x04003353 RID: 13139
	private Coroutine squashCoroutine;

	// Token: 0x04003354 RID: 13140
	private DamageReceiver damageReceiver;

	// Token: 0x04003355 RID: 13141
	[SerializeField]
	private Effect explosionPrefab;

	// Token: 0x04003356 RID: 13142
	[SerializeField]
	private SpriteDeathPartsDLC shrapnelPrefab;
}
