using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200061B RID: 1563
public class FlyingBirdLevelEnemy : AbstractProjectile
{
	// Token: 0x06001FCA RID: 8138 RVA: 0x00123D58 File Offset: 0x00122158
	public FlyingBirdLevelEnemy Create(Vector2 pos, FlyingBirdLevelEnemy.Properties properties)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		FlyingBirdLevelEnemy component = gameObject.GetComponent<FlyingBirdLevelEnemy>();
		component.transform.position = pos;
		component.properties = properties;
		component.Init();
		return component;
	}

	// Token: 0x06001FCB RID: 8139 RVA: 0x00123D97 File Offset: 0x00122197
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001FCC RID: 8140 RVA: 0x00123DB6 File Offset: 0x001221B6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001FCD RID: 8141 RVA: 0x00123DE0 File Offset: 0x001221E0
	private void Init()
	{
		this.startPos = base.transform.position;
		this.health = (float)this.properties.health;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06001FCE RID: 8142 RVA: 0x00123E2F File Offset: 0x0012222F
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001FCF RID: 8143 RVA: 0x00123E5A File Offset: 0x0012225A
	private void Shoot()
	{
	}

	// Token: 0x06001FD0 RID: 8144 RVA: 0x00123E5C File Offset: 0x0012225C
	public override void OnParryDie()
	{
		this.Die();
	}

	// Token: 0x06001FD1 RID: 8145 RVA: 0x00123E64 File Offset: 0x00122264
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		AudioManager.Play("level_flying_bird_smallbird_death");
		this.emitAudioFromObject.Add("level_flying_bird_smallbird_death");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001FD2 RID: 8146 RVA: 0x00123E98 File Offset: 0x00122298
	private IEnumerator y_cr()
	{
		float start = this.startPos.y + this.properties.floatRange / 2f;
		float end = this.startPos.y - this.properties.floatRange / 2f;
		base.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		for (;;)
		{
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < this.properties.floatTime)
			{
				float val2 = t / this.properties.floatTime;
				base.transform.SetPosition(null, new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, end, start, val2)), null);
				t += CupheadTime.Delta;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001FD3 RID: 8147 RVA: 0x00123EB4 File Offset: 0x001222B4
	private IEnumerator x_cr()
	{
		for (;;)
		{
			base.transform.AddPosition(-this.properties.speed * CupheadTime.Delta, 0f, 0f);
			if (base.transform.position.x < -740f)
			{
				this.Die();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001FD4 RID: 8148 RVA: 0x00123ED0 File Offset: 0x001222D0
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.projectileDelay);
			this.Shoot();
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400284A RID: 10314
	[SerializeField]
	private FlyingBirdLevelEnemyProjectile projectilePrefab;

	// Token: 0x0400284B RID: 10315
	private FlyingBirdLevelEnemy.Properties properties;

	// Token: 0x0400284C RID: 10316
	private Vector2 startPos;

	// Token: 0x0400284D RID: 10317
	private float health;

	// Token: 0x0200061C RID: 1564
	public class Properties
	{
		// Token: 0x06001FD5 RID: 8149 RVA: 0x00123EEB File Offset: 0x001222EB
		public Properties(int health, float speed, float floatRange, float floatTime, float projectileHeight, float projectileFallTime, float projectileDelay)
		{
			this.health = health;
			this.speed = speed;
			this.floatRange = floatRange;
			this.floatTime = floatTime;
			this.projectileHeight = projectileHeight;
			this.projectileFallTime = projectileFallTime;
			this.projectileDelay = projectileDelay;
		}

		// Token: 0x0400284E RID: 10318
		public readonly int health;

		// Token: 0x0400284F RID: 10319
		public readonly float speed;

		// Token: 0x04002850 RID: 10320
		public readonly float floatRange;

		// Token: 0x04002851 RID: 10321
		public readonly float floatTime;

		// Token: 0x04002852 RID: 10322
		public readonly float projectileHeight;

		// Token: 0x04002853 RID: 10323
		public readonly float projectileFallTime;

		// Token: 0x04002854 RID: 10324
		public readonly float projectileDelay;
	}
}
