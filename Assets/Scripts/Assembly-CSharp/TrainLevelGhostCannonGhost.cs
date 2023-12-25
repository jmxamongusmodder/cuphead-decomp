using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000819 RID: 2073
public class TrainLevelGhostCannonGhost : HomingProjectile
{
	// Token: 0x1700041E RID: 1054
	// (get) Token: 0x0600300F RID: 12303 RVA: 0x001C6569 File Offset: 0x001C4969
	protected override float DestroyLifetime
	{
		get
		{
			return 1000f;
		}
	}

	// Token: 0x06003010 RID: 12304 RVA: 0x001C6570 File Offset: 0x001C4970
	public TrainLevelGhostCannonGhost Create(Vector3 pos, float delay, float speed, float aimSpeed, float health, float skullSpeed)
	{
		TrainLevelGhostCannonGhost trainLevelGhostCannonGhost = base.Create(pos, -90f, speed, speed, aimSpeed, float.MaxValue, 2f, PlayerManager.GetNext()) as TrainLevelGhostCannonGhost;
		trainLevelGhostCannonGhost.HomingEnabled = false;
		trainLevelGhostCannonGhost.transform.position = pos;
		trainLevelGhostCannonGhost.delay = delay;
		trainLevelGhostCannonGhost.health = health;
		trainLevelGhostCannonGhost.skullSpeed = skullSpeed;
		trainLevelGhostCannonGhost.GetComponent<Collider2D>().enabled = false;
		return trainLevelGhostCannonGhost;
	}

	// Token: 0x06003011 RID: 12305 RVA: 0x001C65DE File Offset: 0x001C49DE
	protected override void Start()
	{
		base.Start();
		this.damageable = false;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06003012 RID: 12306 RVA: 0x001C661D File Offset: 0x001C4A1D
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06003013 RID: 12307 RVA: 0x001C663B File Offset: 0x001C4A3B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.Die();
		}
	}

	// Token: 0x06003014 RID: 12308 RVA: 0x001C6654 File Offset: 0x001C4A54
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (!this.damageable || this.health <= 0f)
		{
			return;
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06003015 RID: 12309 RVA: 0x001C66A8 File Offset: 0x001C4AA8
	protected override void Die()
	{
		AudioManager.Play("train_lollipop_cannon_ghost_death");
		this.emitAudioFromObject.Add("train_lollipop_cannon_ghost_death");
		this.StopAllCoroutines();
		this.health = -1f;
		this.damageable = false;
		base.animator.Play("Die");
	}

	// Token: 0x06003016 RID: 12310 RVA: 0x001C66F7 File Offset: 0x001C4AF7
	private void DropSkull()
	{
		this.skullPrefab.Create(base.transform.position, this.skullSpeed);
	}

	// Token: 0x06003017 RID: 12311 RVA: 0x001C6716 File Offset: 0x001C4B16
	private void OnDieAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003018 RID: 12312 RVA: 0x001C6724 File Offset: 0x001C4B24
	private IEnumerator start_cr()
	{
		yield return base.StartCoroutine(this.up_cr());
		yield return CupheadTime.WaitForSeconds(this, this.delay);
		base.animator.Play("Attack");
		this.damageable = true;
		base.HomingEnabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x06003019 RID: 12313 RVA: 0x001C6740 File Offset: 0x001C4B40
	private IEnumerator up_cr()
	{
		yield return base.TweenPositionY(base.transform.position.y, 500f, 0.4f, EaseUtils.EaseType.linear);
		yield break;
	}

	// Token: 0x0600301A RID: 12314 RVA: 0x001C675B File Offset: 0x001C4B5B
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.skullPrefab = null;
	}

	// Token: 0x040038E5 RID: 14565
	[SerializeField]
	private TrainLevelGhostCannonGhostSkull skullPrefab;

	// Token: 0x040038E6 RID: 14566
	private float delay;

	// Token: 0x040038E7 RID: 14567
	private float health;

	// Token: 0x040038E8 RID: 14568
	private float skullSpeed;

	// Token: 0x040038E9 RID: 14569
	private bool damageable;

	// Token: 0x040038EA RID: 14570
	private DamageReceiver damageReceiver;
}
