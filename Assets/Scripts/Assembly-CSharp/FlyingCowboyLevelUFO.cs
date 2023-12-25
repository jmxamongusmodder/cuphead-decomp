using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000661 RID: 1633
public class FlyingCowboyLevelUFO : AbstractProjectile
{
	// Token: 0x17000391 RID: 913
	// (get) Token: 0x060021FD RID: 8701 RVA: 0x0013C92F File Offset: 0x0013AD2F
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x0013C936 File Offset: 0x0013AD36
	public virtual FlyingCowboyLevelUFO Init(Vector3 pos, LevelProperties.FlyingCowboy.UFOEnemy properties, float health)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.startPos = pos;
		base.transform.position = pos;
		this.properties = properties;
		this.Health = health;
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x0013C973 File Offset: 0x0013AD73
	protected override void Start()
	{
		base.Start();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002200 RID: 8704 RVA: 0x0013C99E File Offset: 0x0013AD9E
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.isDead)
		{
			Level.Current.RegisterMinionKilled();
			this.Respawn();
		}
	}

	// Token: 0x06002201 RID: 8705 RVA: 0x0013C9DE File Offset: 0x0013ADDE
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002202 RID: 8706 RVA: 0x0013C9FC File Offset: 0x0013ADFC
	private IEnumerator move_cr()
	{
		this.isDead = false;
		float leftEdge = -540f;
		float initialLeftEdge = leftEdge - 100f;
		float rightEdge = -640f + this.properties.ufoPathLength;
		float initialX = base.transform.position.x;
		float travelDistance = Mathf.Abs(initialX - initialLeftEdge);
		float travelTime = travelDistance / this.properties.introUFOSpeed;
		float elapsedTime = 0f;
		while (elapsedTime < travelTime)
		{
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x = EaseUtils.Ease(EaseUtils.EaseType.easeOutQuad, initialX, initialLeftEdge, elapsedTime / travelTime);
			base.transform.position = position;
			yield return new WaitForFixedUpdate();
		}
		this.movingLeft = false;
		base.StartCoroutine(this.shoot_cr());
		float currentLeftEdge = initialLeftEdge;
		travelDistance = Mathf.Abs(rightEdge - leftEdge);
		travelTime = travelDistance / this.properties.topUFOSpeed;
		elapsedTime = 0f;
		for (;;)
		{
			if (elapsedTime < travelTime)
			{
				elapsedTime += CupheadTime.FixedDelta;
				Vector3 position2 = base.transform.position;
				position2.x = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, (!this.movingLeft) ? currentLeftEdge : rightEdge, (!this.movingLeft) ? rightEdge : currentLeftEdge, elapsedTime / travelTime);
				base.transform.position = position2;
			}
			else
			{
				currentLeftEdge = leftEdge;
				elapsedTime = 0f;
				this.movingLeft = !this.movingLeft;
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002203 RID: 8707 RVA: 0x0013CA18 File Offset: 0x0013AE18
	private IEnumerator shoot_cr()
	{
		PatternString shootString = new PatternString(this.properties.topUFOShootString, true, true);
		PatternString parryString = new PatternString(this.properties.bulletParryString, true);
		for (;;)
		{
			MinMax spreadAngle = new MinMax(0f, this.properties.spreadAngle);
			yield return CupheadTime.WaitForSeconds(this, shootString.PopFloat());
			for (int i = 0; i < this.properties.bulletCount; i++)
			{
				float num = spreadAngle.GetFloatAt((float)i / ((float)this.properties.bulletCount - 1f));
				float num2 = spreadAngle.max / 2f;
				num -= num2;
				float rotation = num + -90f;
				BasicProjectile basicProjectile = this.projectilePrefab.Create(base.transform.position, rotation, this.properties.bulletSpeed);
				bool flag = parryString.PopLetter() == 'P';
				basicProjectile.SetParryable(flag);
				if (flag)
				{
					basicProjectile.GetComponent<SpriteRenderer>().color = Color.magenta;
				}
			}
		}
		yield break;
	}

	// Token: 0x06002204 RID: 8708 RVA: 0x0013CA33 File Offset: 0x0013AE33
	private void Respawn()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.respawn_cr());
	}

	// Token: 0x06002205 RID: 8709 RVA: 0x0013CA48 File Offset: 0x0013AE48
	private IEnumerator respawn_cr()
	{
		this.isDead = true;
		base.transform.position = new Vector3(1000f, 1000f);
		float waitTime = this.properties.topUFORespawnDelay;
		yield return CupheadTime.WaitForSeconds(this, waitTime);
		this.Health = this.properties.UFOHealth;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.move_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002206 RID: 8710 RVA: 0x0013CA63 File Offset: 0x0013AE63
	public void Dead()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002AAD RID: 10925
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04002AAE RID: 10926
	private const float LEFT_OFFSET = 100f;

	// Token: 0x04002AAF RID: 10927
	private const float INITIAL_LEFT_OFFSET = 100f;

	// Token: 0x04002AB0 RID: 10928
	private LevelProperties.FlyingCowboy.UFOEnemy properties;

	// Token: 0x04002AB1 RID: 10929
	private DamageReceiver damageReceiver;

	// Token: 0x04002AB2 RID: 10930
	private Vector3 startPos;

	// Token: 0x04002AB3 RID: 10931
	private bool isDead;

	// Token: 0x04002AB4 RID: 10932
	private bool movingLeft;

	// Token: 0x04002AB5 RID: 10933
	private float Health;
}
