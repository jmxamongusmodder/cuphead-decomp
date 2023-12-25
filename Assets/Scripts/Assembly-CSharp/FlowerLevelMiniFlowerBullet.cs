using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060C RID: 1548
public class FlowerLevelMiniFlowerBullet : AbstractProjectile
{
	// Token: 0x06001F25 RID: 7973 RVA: 0x0011E2D0 File Offset: 0x0011C6D0
	public void OnBulletSpawned(Vector3 target, int speed, float damage, bool friendlyFireDamage = false)
	{
		this.friendlyFire = friendlyFireDamage;
		this.targetDirection = (target - base.transform.position).normalized;
		this.bulletSpeed = speed;
		this.damage = damage;
		base.StartCoroutine(this.spawn_fx_cr());
	}

	// Token: 0x06001F26 RID: 7974 RVA: 0x0011E31F File Offset: 0x0011C71F
	protected override void Awake()
	{
		this.initDamage = false;
		base.Awake();
	}

	// Token: 0x06001F27 RID: 7975 RVA: 0x0011E32E File Offset: 0x0011C72E
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06001F28 RID: 7976 RVA: 0x0011E338 File Offset: 0x0011C738
	protected override void Update()
	{
		if (!this.initDamage)
		{
			this.damageDealer.SetDamage(this.damage);
			this.damageDealer.SetDamageFlags(true, this.friendlyFire, false);
			this.initDamage = true;
		}
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.Update();
	}

	// Token: 0x06001F29 RID: 7977 RVA: 0x0011E398 File Offset: 0x0011C798
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		base.transform.position += this.targetDirection * (float)this.bulletSpeed * CupheadTime.FixedDelta;
		base.transform.up = -this.targetDirection;
	}

	// Token: 0x06001F2A RID: 7978 RVA: 0x0011E3F3 File Offset: 0x0011C7F3
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06001F2B RID: 7979 RVA: 0x0011E418 File Offset: 0x0011C818
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (this.friendlyFire && hit.GetComponent<FlowerLevelFlower>() != null)
		{
			base.OnCollisionEnemy(hit, phase);
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06001F2C RID: 7980 RVA: 0x0011E464 File Offset: 0x0011C864
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		this.Die();
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06001F2D RID: 7981 RVA: 0x0011E474 File Offset: 0x0011C874
	protected override void Die()
	{
		this.bulletSpeed = 0;
		base.transform.Rotate(Vector3.forward, 360f);
		this.StopAllCoroutines();
		AudioManager.Play("flower_minion_simple_deathpop_high");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_high");
		base.Die();
	}

	// Token: 0x06001F2E RID: 7982 RVA: 0x0011E4C4 File Offset: 0x0011C8C4
	private IEnumerator spawn_fx_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		for (;;)
		{
			this.puff.Create(base.transform.position).transform.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(this.targetDirection)));
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x040027BF RID: 10175
	[SerializeField]
	private Effect puff;

	// Token: 0x040027C0 RID: 10176
	private bool friendlyFire;

	// Token: 0x040027C1 RID: 10177
	private bool initDamage;

	// Token: 0x040027C2 RID: 10178
	private float damage;

	// Token: 0x040027C3 RID: 10179
	private int bulletSpeed;

	// Token: 0x040027C4 RID: 10180
	private Vector3 targetDirection;
}
